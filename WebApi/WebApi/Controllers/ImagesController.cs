using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using VinylProjectApi.Helpers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private WebApiDbContext _webApiDbContext;

        public ImagesController(WebApiDbContext webApiDbContext)
        {
            _webApiDbContext = webApiDbContext;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Image imageModel)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (userEmail == null)
            {
                return NotFound();
            }

            var stream = new MemoryStream(imageModel.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot/images";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FilesHelper.UploadPhoto(stream, folder, file);
            if (!response)
            {
                return BadRequest();
            }
            else
            {
                imageModel.ImageUrl = imageFullPath;
                var image = new Image()
                {
                    ImageUrl = imageModel.ImageUrl,
                    ItemId = imageModel.ItemId,
                };
                _webApiDbContext.Images.Add(image);
                _webApiDbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }

        }
    }
}
