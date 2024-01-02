﻿using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        //GET METHOD THAT WILL BE USED BY THE CONTROLLER
       Task<List<Region>> GetAllAsync();

       Task<Region> GetRegionByIdAsync( Guid id);

        Task<Region> CreateRegionAsync( Region region);

        Task<Region?> UpdateRegionAsync( Guid id, Region region );

        Task<Region?> DeleteRegionAsync( Guid id );
    }
}
