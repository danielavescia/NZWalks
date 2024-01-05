using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly NzWalksDbContext dbContext;
        private readonly IWalksRepository walkRepository;

        public WalksController(IMapper mapper, NzWalksDbContext dbContext, IWalksRepository walkRepository )
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.walkRepository = walkRepository;
        }

        //GET ALL WALKS
        [HttpGet]
        public async Task<IActionResult> GetAllAsync( ) 
        {
            var walksDomainModel =  await walkRepository.GetAllAsync( );
   
            return Ok( mapper.Map<List<WalkDto>>( walksDomainModel ) );
        }

        //CREATE A NEW WALK
        [HttpPost]
        public async Task<IActionResult> CreateWalk( [FromBody] AddWalkRequestDto addWalkRequestDto ) 
        {
           var walkDomainModel =  mapper.Map <Walk> ( addWalkRequestDto );

           await walkRepository.CreateWalkAsync( walkDomainModel );

           var walkDTO = mapper.Map<WalkDto>( walkDomainModel );

           return Ok(mapper.Map<WalkDto>(walkDTO));
        }
    }
}
