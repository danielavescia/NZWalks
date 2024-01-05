﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength( 3, ErrorMessage = "Code has to be a maximum of 3 characteres" )]
        [MinLength( 3, ErrorMessage = "Code has to be a minimum of 3 characteres" )]
        public string Code { get; set; }

        [Required]
        [MaxLength( 100, ErrorMessage = "Nome has to be a maximum of 100 characteres" )]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }  //? = nullable value

        public UpdateRegionRequestDto() { }
    }
}
