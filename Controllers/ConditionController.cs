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

        [HttpGet("{active:bool?}")]
        public ActionResult<ConditionViewModel> Get(bool? active)
        {
            try
            {
                if (active.HasValue)
                {
                    var conditions = _context.Conditions.Where(c => c.Active == active.Value).ToList();
                    if (conditions != null)
                    {
                        return Ok(_mapper.Map<IEnumerable<Condition>, IEnumerable<ConditionViewModel>>(conditions));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var conditions = _context.Conditions.ToList();
                    if(conditions != null)
                    {
                        return Ok(_mapper.Map <IEnumerable<Condition>, IEnumerable<ConditionViewModel>>(conditions));
                    }
                    else
                    {
                        return NotFound();
                    }
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
                    if(_context.SaveChanges() == 1)
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
        public ActionResult<ConditionViewModel> Create([FromBody]ConditionViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newCondition = _mapper.Map<ConditionViewModel, Condition>(model);
                    _context.Add(newCondition);
                    if(_context.SaveChanges() == 1)
                    {
                        return Created($"/api/Condition/{newCondition.ConditionId}", _mapper.Map<Condition, ConditionViewModel>(newCondition));
                    }
                    else
                    {
                        return BadRequest("Failed to create the new Condition");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when creating a new Condition: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ConditionViewModel> Delete(int id)
        {
            try
            {
                var deleteCondition = _context.Conditions.Where(c => c.ConditionId == id).FirstOrDefault();
                if(deleteCondition != null)
                {
                    _context.Remove(deleteCondition);
                    if(_context.SaveChanges() == 1)
                    {
                        return Ok(_mapper.Map<Condition, ConditionViewModel>(deleteCondition));
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
                string error = $"Error when deleting the condition: {ex.Message}";
                _logger.LogWarning(error);
                return BadRequest(error);
            }
        }
    }
}
