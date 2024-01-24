﻿using Microsoft.AspNetCore.Mvc;
using NzWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController( IHttpClientFactory httpClientFactory )
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync( "https://localhost:7081/api/regions" );

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange( await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>() );
            }
            catch ( Exception ex )
            {
                // Log the exception
            }

            return View( response );
        }
    }
}