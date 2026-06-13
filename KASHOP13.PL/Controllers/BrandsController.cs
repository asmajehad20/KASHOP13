using KASHOP13.BLL.Service;
using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.Migrations;
using KASHOP13.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP13.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IBrandService _brandService;
        public BrandsController(IStringLocalizer<SharedResources> localizer, IBrandService brandService)
        {
            _localizer = localizer;
            _brandService = brandService;
        }


        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] BrandRequest request)
        {

            await _brandService.CreateBrand(request);

            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAllBrands();
            return Ok(new
            {
                data = brands,
                _localizer["Success"].Value
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var brand = await _brandService.GetBrand(c => c.Id == id);
            if (brand == null) return NotFound();

            return Ok(new { data = brand  });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _brandService.DeleteBrand(id);
            if (!deleted)
            {
                return NotFound(new { message = _localizer["NotFound"].Value });
            }
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
