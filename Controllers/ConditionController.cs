using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CollectionTrackerAPI.Models;
using CollectionTrackerAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CollectionTrackerAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ConditionController : ControllerBase
    {
        private readonly ILogger<ConditionController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ConditionController(ILogger<ConditionController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ConditionViewModel> Get(int id)
        {
            try
            {
                var condition = _context.Conditions.Where(c => c.ConditionId == id).FirstOrDefault();
                if(condition != null)
                {
                    return Ok(_mapper.Map<Condition, ConditionViewModel>(condition));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when getting the Condition: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpGet("{active:bool}")]
        public ActionResult<ConditionViewModel> Get(bool active)
        {
            try
            {
                var conditions = _context.Conditions.Where(c => c.Active == active).ToList();
                if(conditions != null)
                {
                    return Ok(_mapper.Map<IEnumerable<Condition>, IEnumerable<ConditionViewModel>>(conditions));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when getting active conditions: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }


        [HttpPut]
        public ActionResult<ConditionViewModel> Put([FromBody]ConditionViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var updateCondition = _mapper.Map<ConditionViewModel, Condition>(model);
                    _context.Update(updateCondition);
                    if(_context.SaveChanges() == 0)
                    {
                        return Ok(_mapper.Map<Condition, ConditionViewModel>(updateCondition));
                    }
                    else
                    {
                        return BadRequest("Failed to update the Condition");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when updating the condition: {ex.Message}";
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
                        return BadRequest("Failed to create the new Category");
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

        [HttpDelete("{id:int}")]
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
                string error = $"Error when deleting the category: {ex.Message}";
                _logger.LogWarning(error);
                return BadRequest(error);
            }
        }
    }
}
