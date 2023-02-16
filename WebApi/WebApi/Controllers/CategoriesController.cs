using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : Controller
    {
        private WebApiDbContext _webApiDbContext;

        public CategoriesController(WebApiDbContext webApiDbContext)
        {
            _webApiDbContext = webApiDbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _webApiDbContext.Categories;
            return Ok(categories);
        }
    }


}
