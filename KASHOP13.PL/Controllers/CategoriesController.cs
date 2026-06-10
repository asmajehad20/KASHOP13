using KASHOP13.BLL.Service;
using KASHOP13.DAL.Data;
using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using KASHOP13.DAL.Repository;
using KASHOP13.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Text.Json;

namespace KASHOP13.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;
        public CategoriesController(IStringLocalizer<SharedResources> localizer, ICategoryService categoryService)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }


        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            
            await _categoryService.CreateCategory(request);

            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(new
            {
                data = categories,
                _localizer["Success"].Value
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _categoryService.GetCategory(c => c.Id == id));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);
            if (!deleted)
            {
                return NotFound(new { message = _localizer["NotFound"].Value });
            }
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
