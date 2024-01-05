using AutoMapper;
using Catel.Data;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]

    public class RegionsController : ControllerBase
    {

        private readonly NzWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        //constructor+ dependecy injection
        public RegionsController( NzWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        //GET ALL REGIONS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            //Mapping RegionDomain to RegionDto
            var regionsDto = mapper.Map<List<RegionDto>>( regionsDomain);

            return Ok( regionsDto );

        }

        //GET ONE SPECIFIC REGION BY ID
        [HttpGet]
        [Route( "{id:Guid}" )]
        public async Task<IActionResult> GetRegionById( [FromRoute] Guid id )
        {
            var regionDomain = await regionRepository.GetRegionByIdAsync( id );

            if ( regionDomain == null )
            {
                return NotFound();
            }

            //Map Region Domain Model to Region DTO
            var regionDto = mapper.Map<RegionDto>( regionDomain );

            return Ok( regionDto );
        }

        //POST: CREATES NEW REGION
        [HttpPost]
        [ValidateModelAtributte] // Model Validation
        public async Task<IActionResult> CreateRegion( [FromBody] AddRegionDto regionDto )
        {
  
            //Converting DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>( regionDto );

            //Using Domain Model to create a new Region to dbContext
            regionDomainModel = await regionRepository.CreateRegionAsync( regionDomainModel );

            //creates DTO to show to the client
            var regionDTO = mapper.Map<RegionDto>( regionDomainModel );

            return CreatedAtAction( nameof( GetRegionById ), new { id = regionDTO.Id }, regionDTO );
          
        }


        //UPDATE DATA OF SPECIFIC REGION
        [HttpPut]
        [Route( "{id:Guid}" )]
        [ValidateModelAtributte] // Model Validation
        public async Task<IActionResult> UpdateRegionAsync( [FromRoute] Guid id, [FromBody] UpdateRegionRequestDto regionDto )
        {
           
            //Convert Dto to Domain Model
            var regionDomainModel = mapper.Map<Region>( regionDto );

            //check if regions exists
            regionDomainModel = await regionRepository.UpdateRegionAsync( id, regionDomainModel );

            if ( regionDomainModel == null )
            {
                return NotFound();
            }

            //Convert Domain Model to Dto  
            var updateRegionDTO = mapper.Map<RegionDto>( regionDomainModel );

            return Ok( updateRegionDTO );
        }
        
        //DELETE REGION BY ID
        [HttpDelete]
        [Route( "{id:Guid}" )]
        public async Task<IActionResult> DeleteRegion( [FromRoute] Guid id )
        {
            var regionDomainModel = await regionRepository.DeleteRegionAsync( id );

            if ( regionDomainModel == null )
            {
                return NotFound();
            }

            //return deleted regions back
            var deleteRegionDTO = mapper.Map<RegionDto>( regionDomainModel );

            return Ok( deleteRegionDTO );
        }

        }
    }





