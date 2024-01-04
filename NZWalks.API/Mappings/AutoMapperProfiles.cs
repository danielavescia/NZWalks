using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Region = NZWalks.API.Models.Domain.Region;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap <Region, RegionDto> ().ReverseMap();
            CreateMap <AddRegionDto, Region> ().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk> ().ReverseMap();
            CreateMap<Walk, WalkDto> ().ReverseMap();
        }
    }
}
