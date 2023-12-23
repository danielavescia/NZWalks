using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]

    public class RegionsController : ControllerBase
    {

        private readonly NzWalksDbContext dbContext;

        //constructor
        public RegionsController( NzWalksDbContext dbContext )
        {
            this.dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain = dbContext.Regions.ToList();

            var regionsDto = new List<RegionDto>();
            foreach ( var regionDomain in regionsDomain ) 
            {
                regionsDto.Add( new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,

                });
            }

            return Ok( regionsDto );

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id )
        {
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

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
    }
}
