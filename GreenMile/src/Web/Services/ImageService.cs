using System.Net;

using Microsoft.AspNetCore.Identity;

using Web.Data;
using Web.Models;
using Web.Utils;

using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;

namespace Web.Services
{
    public class ImageService : IImageService
    {
       public static readonly long UploadSize = 10 * 1024 * 1024;
        public static readonly string destinationFolder = "uploads";
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        private readonly DataContext _dataContext;
       
        public ImageService(IWebHostEnvironment env, UserManager<User> userManager, DataContext dataContext)
        {
            _env = env;
            _userManager = userManager;
    
            _dataContext = dataContext;
        }

        async Task<Result<string>> IImageService.RetrieveImage(User user)
        {
            if(user is null)
            {
                return Result<string>.Failure("User cannot be null!");
            }
            return Result<string>.Success("Image successfullly retrieved!", string.IsNullOrEmpty(user.ImageURL) ? "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Default_pfp.svg/2048px-Default_pfp.svg.png" : user.ImageURL);
        }

         async Task<Result<string>> IImageService.StoreImage(IFormFile image, User user)
        {
            if(image == null)
            {
                return Result<string>.Failure("Image is null!");
            }
            else if (image.Length > UploadSize)
            {
                return Result<string>.Failure("Upload size is too big!");
            }

            var imagePath = $"/{destinationFolder}/{await BaseStoreImage(image)}";


                user.ImageURL = imagePath;
            await _userManager.UpdateAsync(user);
                return Result<string>.Success("Upload successful!", imagePath);
            
        }

        

        async Task IImageService.StoreImageFromUrl(string url, User user)
        {

            using (WebClient webClient = new WebClient())
            {
                string imageFile = Guid.NewGuid() + ".png";
                string imagePath = Path.Combine(_env.ContentRootPath, "wwwroot", destinationFolder, imageFile);
                webClient.DownloadFile(new Uri(url), imagePath);
                user.ImageURL = $"/{destinationFolder}/{imageFile}";
                await _userManager.UpdateAsync(user);


            }
        }

        async Task<string> IImageService.StoreImage(IFormFile image)
        {
            if (image == null)
            {
                return "Image is null!";
            }
            else if (image.Length > UploadSize)
            {
                return "Upload size is too big!";
            }

            var imagePath = $"/{destinationFolder}/{await BaseStoreImage(image)}";


           
            return  imagePath;
        }

        private async Task<string> BaseStoreImage(IFormFile image)
        {
            string imageFile = Guid.NewGuid() + Path.GetExtension(image.FileName);
            string imagePath = Path.Combine(_env.ContentRootPath, "wwwroot", destinationFolder, imageFile);
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            return imageFile;

        }

     
    }

  
}
