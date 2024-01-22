using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;


namespace NZWalks.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //Upload images into db
        [HttpPost]
        [Route( "Upload" )]
        public async Task<IActionResult> UploadImage( [FromForm] ImageUploadDto imageUploaded )
        {
            ValidateFileUpload( imageUploaded );

            if ( ModelState.IsValid )
            {
                var imageDomainModel = new Image
                {
                    File = imageUploaded.File,
                    FileExtension = Path.GetExtension( imageUploaded.File.FileName ),
                    FileSizeBytes = imageUploaded.File.Length,
                    FileName = imageUploaded.FileName,
                    FileDescription = imageUploaded.FileDescription

                };

                await imageRepository.UploadImage( imageDomainModel );

                return Ok( imageDomainModel );
               
            }

            return BadRequest( ModelState );
        }

        private void ValidateFileUpload( ImageUploadDto imageUploaded )
        {
            var allowedExtensions = new string [] { ".jpg", ".jpeg", ".png" };

            if ( !allowedExtensions.Contains( Path.GetExtension( imageUploaded.File.FileName ) ) )
            {
                ModelState.AddModelError( "file", "Unsupported file extension" );
            }

            if ( imageUploaded.File.Length > 10485760 ) 
            {
                ModelState.AddModelError( "file", "File size is more than 10MB, please upload a smaller size file" );
            }
        }
    }
}