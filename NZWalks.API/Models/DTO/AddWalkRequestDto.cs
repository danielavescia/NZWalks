namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDto
    {
        public string NameTrail { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }

}
