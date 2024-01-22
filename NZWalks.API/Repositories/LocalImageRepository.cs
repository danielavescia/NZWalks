using Microsoft.AspNetCore.Hosting;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;


namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NzWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnviroment, IHttpContextAccessor httpContextAccessor, NzWalksDbContext dbContext) 
        { 
            this.webHostEnviroment = webHostEnviroment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> UploadImage( Image imageUploaded )
        {
            var localFilePath = Path.Combine( webHostEnviroment.ContentRootPath, "Images",
                $"{imageUploaded.FileName}{imageUploaded.FileExtension}" );

            // Upload Image to Local Path
            using var stream = new FileStream( localFilePath, FileMode.Create );

            await imageUploaded.File.CopyToAsync( stream ); 


            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{imageUploaded.FileName}{imageUploaded.FileExtension}";

            imageUploaded.FilePath = urlFilePath;

            //Add images info into Images table
            await dbContext.Images.AddAsync( imageUploaded );
            await dbContext.SaveChangesAsync();

            return imageUploaded;

        }
    }
}
