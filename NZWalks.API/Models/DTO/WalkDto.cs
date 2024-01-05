using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string NameTrail { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public string? WalkImageUrl { get; set; }

        //Navigation properties - Entity framework association between entities
        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
