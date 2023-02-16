using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private WebApiDbContext _webApiDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public AccountsController(WebApiDbContext webApiDbContext, IConfiguration configuration)
        {
            _webApiDbContext = webApiDbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            var userWithSameEmail = _webApiDbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists");
            }
            var userObj = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
            };

            _webApiDbContext.Users.Add(userObj);
            _webApiDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var userEmail = _webApiDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Email, user.Email),
              new Claim(ClaimTypes.Email, user.Email),
            };

            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (userEmail == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(changePasswordModel.OldPassword, user.Password))
            {
                return Unauthorized("Sorry you can't change the password");
            }

            user.Password = SecurePasswordHasherHelper.Hash(changePasswordModel.NewPassword);
            _webApiDbContext.SaveChanges();
            return Ok("Your password has been changed");
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditPhoneNumber([FromBody] ChangePhoneModel changePhoneModel)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            user.Phone = changePhoneModel.PhoneNumber;
            _webApiDbContext.SaveChanges();
            return Ok("Your phone number has been updated");
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditUserProfile([FromBody] byte[] ImageArray)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (userEmail == null)
            {
                return NotFound();
            }

            var stream = new MemoryStream(ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot/ProfileImages";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FilesHelper.UploadPhoto(stream, folder, file);
            if (!response)
            {
                return BadRequest();
            }
            else
            {
                user.ImageUrl = imageFullPath;
                _webApiDbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);    
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserProfileImage()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return NotFound();
            var responseResult = _webApiDbContext.Users
                .Where(x => x.Email == userEmail)
                .Select(x => new
                {
                    x.ImageUrl,
                })
                .SingleOrDefault();
            return Ok(responseResult);
        }

    }
}
