using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Drawing;

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
        public async Task<Walk> CreateWalkAsync( Walk walk )
        {
            await dbContext.Walks.AddAsync( walk );
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public Task<Walk?> DeleteWalkAsync( Guid id )
        {
            throw new NotImplementedException();
        }

        public Task<List<Walk>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Walk> GetWalknByIdAsync( Guid id )
        {
            throw new NotImplementedException();
        }

        public Task<Walk?> UpdateWalknAsync( Guid id, Walk walk )
        {
            throw new NotImplementedException();
        }
    }
}
