using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalksRepository
    {
        private readonly NzWalksDbContext dbContext;

        //constructor/ dependecy injection
        public SQLWalkRepository( NzWalksDbContext dbContext )
        {
            this.dbContext = dbContext;

        }

        //GET ALL WALKS METHOD
        public async Task<List<Walk>> GetAllAsync( string? filterOn = null, string? filterQuery = null, 
            [FromQuery] string? sortBy = null, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000 )

        {
            //include method get Difficulty and Region data by the navigation property define in WALK Domain Model
            var walks = dbContext.Walks.Include( "Difficulty" ).Include( "Region" ).AsQueryable();

            //FILTERING
            if ( string.IsNullOrWhiteSpace(filterOn) == false  && string.IsNullOrWhiteSpace( filterQuery ) == false ) 
            {
                if ( filterOn.Equals( "NameTrail", StringComparison.OrdinalIgnoreCase ) ) 
                {
                    walks = walks.Where( x => x.NameTrail.Contains( filterQuery ));
                }
            }

            //SORTING
            if ( string.IsNullOrWhiteSpace( sortBy ) == false ) 
            {
                if ( sortBy.Equals( "NameTrail", StringComparison.OrdinalIgnoreCase ) )
                {
                    walks = isAscending ? walks.OrderBy( x => x.NameTrail ) : walks.OrderByDescending( x => x.NameTrail );
                }
                else if ( sortBy.Equals( "Length", StringComparison.OrdinalIgnoreCase ) ) 
                {
                    walks = isAscending ? walks.OrderBy( x => x.Length ) : walks.OrderByDescending( x => x.Length );
                }
            }

            //PAGINATION
            var skipResults = ( pageNumber - 1 ) * pageSize;

            return await walks.Skip( skipResults ).Take(pageSize).ToListAsync();
        }

        //GET WALK BY ID METHOD
        public async Task<Walk?> GetWalkByIdAsync( Guid id )
        {
            return await dbContext.Walks.Include( "Difficulty" ).Include( "Region" ).FirstOrDefaultAsync(x => x.Id == id);
        }

        //CREATE METHOD
        public async Task<Walk> CreateWalkAsync( Walk walk )
        {
            await dbContext.Walks.AddAsync( walk );
            await dbContext.SaveChangesAsync();
            return walk;
        }

        //PUT WALK BY ID METHOD
        public async Task<Walk?> UpdateWalknAsync( Guid id, Walk walk )
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync( x => x.Id == id );

            if ( existingWalk == null ) 
            {
                return null;
            }

            existingWalk.NameTrail = walk.NameTrail;
            existingWalk.Description = walk.Description;
            existingWalk.Length = walk.Length;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            dbContext.SaveChangesAsync();

            return existingWalk;
        }

        //DELETE METHOD
        public async Task<Walk?> DeleteWalkAsync( Guid id )
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync( y => y.Id == id );

            if ( existingWalk == null )
            {
                return null;
            }

            dbContext.Walks.Remove( existingWalk );

            await dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
