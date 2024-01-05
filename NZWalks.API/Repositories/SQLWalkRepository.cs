using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.DTO;

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
        public async Task<List<Walk>> GetAllAsync()

        {
            //include method get Difficulty and Region data by the navigation property define in WALK Domain Model
            return await dbContext.Walks.Include("Difficulty").Include( "Region" ).ToListAsync();
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
