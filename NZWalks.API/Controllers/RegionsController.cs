using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        //constructor+ dependecy injection
        public RegionsController( NzWalksDbContext dbContext, IRegionRepository regionRepository )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;

        }


        //GET ALL REGIONS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            var regionsDto = new List<RegionDto>();

            foreach ( var regionDomain in regionsDomain )
            {
                regionsDto.Add( new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,

                } );
            }

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

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,

            };

            return Ok( regionDto );
        }

        //POST: CREATES NEW REGION
        [HttpPost]
        [Route( "id:Guid" )]
        public async Task<IActionResult> CreateRegion( [FromBody] AddRegionDto regionDto )
        {

            //Converting DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = regionDto.Code,
                Name = regionDto.Name,
                RegionImageUrl = regionDto.RegionImageUrl,

            };

            //Using Domain Model to create a new Region to dbContext
            regionDomainModel = await regionRepository.CreateRegionAsync( regionDomainModel );

            //creates DTO to show to the client
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };


            return CreatedAtAction( nameof( GetRegionById ), new { id = regionDTO.Id }, regionDTO );
        }


        //UPDATE DATA OF SPECIFIC REGION
        [HttpPut]
        [Route( "{id:Guid}" )]

        public async Task<IActionResult> UpdateRegionAsync( [FromRoute] Guid id, [FromBody] UpdateRegionRequestDto regionDto )
        {
            //Convert Dto to Domain Model
            var regionDomainModel = new Region
            {
                Code = regionDto.Code,
                Name = regionDto.Name,
                RegionImageUrl = regionDto.RegionImageUrl
            };

            //check if regions exists
            regionDomainModel = await regionRepository.UpdateRegionAsync( id, regionDomainModel );

            if ( regionDomainModel == null )
            {
                return NotFound();
            }


            //Convert Domain Model to Dto  
            var updateRegionDTO = new RegionDto
             {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
             };

          
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
                var deleteRegionDTO = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };

                return Ok( deleteRegionDTO );
            }

        }
    }





