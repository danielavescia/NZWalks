using NZWalks.API.Models.Domain;
using NZWalks.API.Data;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NzWalksDbContext dbContext;

        //constructor/ dependecy injection
        public SQLRegionRepository( NzWalksDbContext dbContext )
        {
            this.dbContext = dbContext;

        }

        //METHODS

        //GET ALL
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        //GET BY ID
        public async Task<Region?> GetRegionByIdAsync( Guid id )
        {
            return await dbContext.Regions.FirstOrDefaultAsync();
        }

        //POST 
        public async Task<Region> CreateRegionAsync( Region region )
        {

            await dbContext.Regions.AddAsync( region );
            await dbContext.SaveChangesAsync();
            return region;
        }

        //PUT
        public async Task<Region> UpdateRegionAsync( Guid id, Region region )
        {
            var regionDb = await dbContext.Regions.FirstOrDefaultAsync( x => x.Id == id );

            if ( regionDb == null )
            {
                return null;
            }

            regionDb.Code = region.Code;
            regionDb.Name = region.Name;
            regionDb.RegionImageUrl = region.RegionImageUrl;

            //update region and save the changes
            await dbContext.SaveChangesAsync();

            return regionDb;
        }

        //DELETE
        public async Task<Region> DeleteRegionAsync( Guid id )
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync( y => y.Id == id );

            if ( existingRegion == null ) 
            {
                return null;
            }
            
            dbContext.Regions.Remove( existingRegion );
            await dbContext.SaveChangesAsync();
           
            return existingRegion;
        }
    }
}
