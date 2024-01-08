using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalksRepository
    {
        //GET METHOD THAT WILL BE USED BY THE CONTROLLER
        Task<List <Walk>> GetAllAsync(string? filterOn = null, string? filterQuery= null);

        Task<Walk?> GetWalkByIdAsync( Guid id );

        Task<Walk> CreateWalkAsync( Walk walk );

        Task<Walk?> UpdateWalknAsync( Guid id, Walk walk );

        Task<Walk?> DeleteWalkAsync( Guid id );
    }
}
