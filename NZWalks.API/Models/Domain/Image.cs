using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        
        [NotMapped] // it doesnt save into db
        public IFormFile File { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeBytes { get; set; }
        public string FilePath { get; set; }
    }
}
