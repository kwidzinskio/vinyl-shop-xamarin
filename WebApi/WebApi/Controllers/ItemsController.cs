using AuthenticationPlugin;
using ImageUploader;
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
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private WebApiDbContext _webApiDbContext;

        public ItemController(WebApiDbContext webApiDbContext)
        {
            _webApiDbContext = webApiDbContext;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(Item item)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (userEmail == null)
            {
                return NotFound();
            }

            var itemObj = new Item()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Band = item.Band,
                Condition = item.Condition,
                DatePosted = item.DatePosted,
                ReleaseYear = item.ReleaseYear,
                Price = item.Price,
                Album = item.Album,
                Location = item.Location,
                Longitude = item.Longitude,
                Latitude = item.Latitude,
                CategoryId = item.CategoryId,
                IsFeatured = false,
                IsHotAndNew = false,
                UserId = user.Id,
            };

            _webApiDbContext.Items.Add(itemObj);
            _webApiDbContext.SaveChanges();

            return Ok(new { itemId = itemObj.Id, message = "Item has been added" });
        }

        [Authorize]
        [HttpGet("[action]")]
        public IQueryable<object> SearchItems(string search)
        {
            var items = from v in _webApiDbContext.Items
                           where v.Title.StartsWith(search) || v.Album.StartsWith(search) || v.Band.StartsWith(search)
                           select new
                           {
                               Id = v.Id,
                               Title = v.Title,
                               Album = v.Album,
                               Band = v.Band,
                           };

            return items.Take(15);
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetItems(int categoryId)
        {
            var items = from v in _webApiDbContext.Items
                        where v.CategoryId == categoryId
                        select new
                        {
                            Id = v.Id,
                            Title = v.Title,
                            Price = v.Price,
                            Album = v.Album,
                            Location = v.Location,
                            Band = v.Band,
                            DatePosted = v.DatePosted,
                            IsFeatured = v.IsFeatured,
                            ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                        };

            return Ok(items);
        }

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult GetItemsMap()
        {
            var items = from v in _webApiDbContext.Items
                        select new
                        {
                            Id = v.Id,
                            Title = v.Title,
                            Price = v.Price,
                            Album = v.Album,
                            Location = v.Location,
                            Band = v.Band,
                            DatePosted = v.DatePosted,
                            IsFeatured = v.IsFeatured,
                            Longitude = v.Longitude,
                            Latitude = v.Latitude,
                            ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                        };

            return Ok(items);
        }


        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> ItemDetails(int id)
        {
            var foundItem = await _webApiDbContext.Items.FindAsync(id);

            if (foundItem == null)
            {
                return NoContent();
            }

            var item = (from v in _webApiDbContext.Items
                        join u in _webApiDbContext.Users on v.UserId equals u.Id
                        where v.Id == id
                        select new
                        {
                            Id = v.Id,
                            Title = v.Title,
                            Description = v.Description,
                            Price = v.Price,
                            Album = v.Album,
                            ReleaseYear = v.ReleaseYear,
                            Band = v.Band,
                            DatePosted = v.DatePosted,
                            Condition = v.Condition,
                            IsHotAndNew = v.IsHotAndNew,
                            IsFeatured = v.IsFeatured,
                            Location = v.Location,
                            Latitude = v.Latitude,
                            Longitude = v.Longitude,
                            Images = v.Images,
                            Email = u.Email,
                            Phone = u.Phone,
                            ImageUrl = u.ImageUrl,
                            Username = u.Name
                        }).FirstOrDefault();

            return Ok(item);
        }


        [Authorize]
        [HttpGet("[action]")]
        public IActionResult MyAds()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _webApiDbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (userEmail == null)
            {
                return NotFound();
            }

            var items = from v in _webApiDbContext.Items
                        where v.UserId == user.Id
                        select new
                        {
                            Id = v.Id,
                            Title = v.Title,
                            Price = v.Price,
                            Album = v.Album,
                            Location = v.Location,
                            Band = v.Band,
                            DatePosted = v.DatePosted,
                            IsFeatured = v.IsFeatured,
                            ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                        };

            return Ok(items);
        }

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult HotAndNewAds()
        {
            var items = from v in _webApiDbContext.Items
                        where v.IsHotAndNew == true
                        select new
                        {
                            Id = v.Id,
                            Title = v.Title,
                            Price = v.Price,
                            Album = v.Album,
                            Band = v.Band,
                            IsFeatured = v.IsFeatured,
                            ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                        };

            return Ok(items);
        }

        [HttpGet("[action]")]
        [Authorize]
        public IQueryable<object> FilterItems(int categoryId, string condition, string sort, double price)
        {
            IQueryable<object> items;

            switch (sort)
            {
                case "desc":
                    items = from v in _webApiDbContext.Items
                            join u in _webApiDbContext.Users on v.UserId equals u.Id
                            join c in _webApiDbContext.Categories on v.CategoryId equals c.Id
                            where v.Price >= price && c.Id == categoryId && v.Condition == condition
                            orderby v.Price descending
                            select new
                            {
                                Id = v.Id,
                                Title = v.Title,
                                Price = v.Price,
                                Album = v.Album,
                                Location = v.Location,
                                Band = v.Band,
                                DatePosted = v.DatePosted,
                                IsFeatured = v.IsFeatured,
                                ImageUrl = v.Images.FirstOrDefault().ImageUrl,

                            };
                    break;

                case "asc":
                    items = from v in _webApiDbContext.Items
                            join u in _webApiDbContext.Users on v.UserId equals u.Id
                            join c in _webApiDbContext.Categories on v.CategoryId equals c.Id
                            where v.Price >= price && c.Id == categoryId && v.Condition == condition
                            orderby v.Price ascending
                            select new
                            {
                                Id = v.Id,
                                Title = v.Title,
                                Price = v.Price,
                                Album = v.Album,
                                Location = v.Location,
                                Band = v.Band,
                                DatePosted = v.DatePosted,
                                IsFeatured = v.IsFeatured,
                                ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                            };
                    break;

                default:
                    items = from v in _webApiDbContext.Items
                            where v.CategoryId == categoryId
                            select new
                            {
                                Id = v.Id,
                                Title = v.Title,
                                Price = v.Price,
                                Album = v.Album,
                                Location = v.Location,
                                Band = v.Band,
                                DatePosted = v.DatePosted,
                                IsFeatured = v.IsFeatured,
                                ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                            };
                    break;
            }

            return items;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Item item)
        {
            var product = _webApiDbContext.Items.Find(id);
            if (product == null)
            {
                return NotFound("No product found against this id...");
            }
            else
            {
                product.Location = item.Location;
                product.Latitude = item.Latitude;
                product.Longitude = item.Longitude;
                _webApiDbContext.SaveChanges();
                return Ok("Product Updated Successfully...");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _webApiDbContext.Items.Find(id);
            if (product == null)
            {
                return NotFound("No product found against this id...");
            }
            else
            {
                _webApiDbContext.Items.Remove(product);
                _webApiDbContext.SaveChanges();
                return Ok("Product deleted...");
            }
        }


    }



}
