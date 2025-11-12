using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;
using MiniEcom.Services;

namespace MiniEcom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesRepository _imagesRepository;

        public ImagesController(IImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
        }

        [HttpPost("{productId}/upload")]
        public async Task<IActionResult> UploadImages(int productId, List<IFormFile> files)
        {
         

           var images = await _imagesRepository.UploadProductImagesAsync(productId, files, false);
            if(images == null) { NotFound("Failed to upload images"); }
            return Ok(images);
        }

        //[HttpDelete("{productId}/delete")]
        //public async Task<IActionResult> DeleteImage(int productId, Guid imageId)
        //{
        //    var image = await _imagesRepository.DeleteProductImageAsync(imageId);
        //}
    }
}
