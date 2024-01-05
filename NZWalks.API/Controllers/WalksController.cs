using AutoMapper;
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
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly NzWalksDbContext dbContext;
        private readonly IWalksRepository walkRepository;

        public WalksController( IMapper mapper, NzWalksDbContext dbContext, IWalksRepository walkRepository )
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.walkRepository = walkRepository;
        }

        //GET ALL WALKS
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var walksDomainModel = await walkRepository.GetAllAsync();

            return Ok( mapper.Map<List<WalkDto>>( walksDomainModel ) );
        }

        //GET ONE WALK BY ID
        [HttpGet]
        [Route( "{id:Guid}" )]
        public async Task<IActionResult> GetWalknByIdAsync( [FromRoute] Guid id )
        {

            var walkDomainModel = await walkRepository.GetWalkByIdAsync( id );

            if ( walkDomainModel == null )
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<WalkDto>( walkDomainModel );

            return Ok( walkDTO );
        }


        //CREATE A NEW WALK
        [HttpPost]
        [ValidateModelAtributte] // Model Validation
        public async Task<IActionResult> CreateWalk( [FromBody] AddWalkRequestDto addWalkRequestDto )
        {
  
            var walkDomainModel = mapper.Map<Walk>( addWalkRequestDto );

            await walkRepository.CreateWalkAsync( walkDomainModel );

            var walkDTO = mapper.Map<WalkDto>( walkDomainModel );

            return Ok( walkDTO );
        }

        //UPDATE A WALK
        [HttpPost]
        [Route( "{id:Guid}" )]
        [ValidateModelAtributte] // Model Validation

        public async Task<IActionResult> UpdateWalknAsync( [FromRoute] Guid id, [FromBody] UpdateWalkDto walkDTO )
        {
           
            var walkDomainModel = mapper.Map<Walk>( walkDTO );

            walkDomainModel = await walkRepository.UpdateWalknAsync( id, walkDomainModel );

            if ( walkDomainModel == null )
            {
                return NotFound();
            }

            var walkupdateDTO = mapper.Map<WalkDto>( walkDomainModel );

            return Ok( walkupdateDTO );
  
        }


        //DELETE WALK BY ID
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete( [FromRoute] Guid id )
        {
            
            var walkDomainModel = await walkRepository.DeleteWalkAsync( id );

            if ( walkDomainModel == null )
            {
                return NotFound();
            }

            var walkDeleteDTO = mapper.Map<WalkDto>( walkDomainModel );

            return Ok( walkDeleteDTO );
        }

    }

}


