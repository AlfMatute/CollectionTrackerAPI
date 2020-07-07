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
    public class CollectionController : ControllerBase
    {
        private readonly ILogger<CollectionViewModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CollectionController(ILogger<CollectionViewModel> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public ActionResult<CollectionViewModel> Get(int id)
        {
            try
            {
                var collection = _context.Collections.Where(c => c.CollectionId == id).FirstOrDefault();
                if(collection != null)
                {
                    return Ok(_mapper.Map<Collection, CollectionViewModel>(collection));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when getting the Collection: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        //[HttpGet("{username:string}")]
        [HttpGet]
        public ActionResult<IEnumerable<CollectionViewModel>> Get(string username)
        {
            try
            {
                var collection = _context.Collections.Where(c => c.CollectionUser.UserName == username).ToList();
                if (collection != null)
                {
                    return Ok(_mapper.Map<IEnumerable<Collection>, IEnumerable<CollectionViewModel>>(collection));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                string error = $"Error when getting the users collection: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpPut]
        public ActionResult<CollectionViewModel> Put(CollectionViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var updateCollection = _mapper.Map<CollectionViewModel, Collection>(model);
                    _context.Update(updateCollection);
                    if(_context.SaveChanges() == 0)
                    {
                        return Ok(_mapper.Map<Collection, CollectionViewModel>(updateCollection));
                    }
                    else
                    {
                        return BadRequest("Failed to update the collection");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when updating the Collection: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpPost]
        public ActionResult<CollectionViewModel> Create(CollectionViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newCollection = _mapper.Map<CollectionViewModel, Collection>(model);
                    _context.Add(newCollection);
                    if(_context.SaveChanges() == 0)
                    {
                        return Created($"/api/Collection/{newCollection.CollectionId}", _mapper.Map<Collection, CollectionViewModel>(newCollection));
                    }
                    else
                    {
                        return BadRequest("Failed to create the new collection");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when creating the Collection: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CollectionViewModel> Delete(int id)
        {
            try
            {
                var deleteCollection = _context.Collections.Where(c => c.CollectionId == id).FirstOrDefault();
                if(deleteCollection != null)
                {
                    _context.Remove(deleteCollection);
                    if(_context.SaveChanges() == 0)
                    {
                        return Ok(_mapper.Map<Collection, CollectionViewModel>(deleteCollection));
                    }
                    else
                    {
                        return BadRequest("Failed to delete the Collection");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                string error = $"Error when deleting the Collection: {ex.Message}";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }
    }
}
