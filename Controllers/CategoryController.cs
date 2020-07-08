using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CollectionTrackerAPI.Models;
using CollectionTrackerAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CollectionTrackerAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public ActionResult<CategoryViewModel> Get(int id)
        {
            try
            {
                var category = _context.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
                if(category != null)
                {
                    return Ok(_mapper.Map<Category, CategoryViewModel>(category));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                string error = $"Error when getting the category: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpGet("{active:bool}")]
        public ActionResult<IEnumerable<CategoryViewModel>> Get(bool? active)
        {
            try
            {
                if (active.HasValue)
                {
                    var categories = _context.Categories.Where(c => c.Active == active).ToList();
                    if (categories != null)
                    {
                        return Ok(_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var categories = _context.Categories.ToList();
                    if(categories != null)
                    {
                        return Ok(_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch(Exception ex)
            {
                string errror = $"Error when getting the Categories: {ex.Message}";
                _logger.LogError(errror);
                return BadRequest(errror);
            }
        }

        [HttpPut]
        public ActionResult<CategoryViewModel> Put([FromBody] CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var updateCategory = _mapper.Map<CategoryViewModel, Category>(model);
                    _context.Update(updateCategory);
                    if (_context.SaveChanges() == 0)
                    {
                        return Ok(_mapper.Map<Category, CategoryViewModel>(updateCategory));
                    }
                    else
                    {
                        return BadRequest($"Failed to update the Category");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                string error = $"Error when updating Category: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpPost]
        public ActionResult<CategoryViewModel> Create([FromBody]CategoryViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newCategory = _mapper.Map<CategoryViewModel, Category>(model);
                    _context.Add(newCategory);
                    if(_context.SaveChanges() == 0)
                    {
                        return Created($"/api/Category/{newCategory.CategoryId}", _mapper.Map<Category, CategoryViewModel>(newCategory));
                    }
                    else
                    {
                        return BadRequest("Failed to save the new Category");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when creating a new Category: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpDelete("id:int")]
        public ActionResult<CategoryViewModel> Delete(int id)
        {
            try
            {
                var deleteCategory = _context.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
                if(deleteCategory != null)
                {
                    _context.Remove(deleteCategory);
                    if(_context.SaveChanges() == 0)
                    {
                        return Ok(_mapper.Map<Category, CategoryViewModel>(deleteCategory));
                    }
                    else
                    {
                        return BadRequest("Failed to delete the Category");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when deleting the Category: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }
    }
}
