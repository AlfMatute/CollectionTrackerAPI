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
    [ApiController]
    [Route("api/[Controller]")]
    public class BrandController : ControllerBase
    {
        private readonly ILogger<BrandController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BrandController(ILogger<BrandController> logger, ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _logger = logger;
            _context = applicationDbContext;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public ActionResult<BrandViewModel> Get(int id)
        {
            try
            {
                var brand = _context.Brands.Where(b => b.BrandId == id).FirstOrDefault();
                if(brand != null)
                {
                    return Ok(_mapper.Map<Brand, BrandViewModel>(brand));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting the brand: {ex.Message}");
                return BadRequest($"Error when getting the brand: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<BrandViewModel>> Get()
        {
            try
            {
                var brands = _context.Brands.ToList();
                if(brands != null)
                {
                    return Ok(_mapper.Map<IEnumerable<Brand>, IEnumerable<BrandViewModel>>(brands));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when getting brands: {ex.Message}");
                return BadRequest($"Error when getting brands: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<BrandViewModel> Create([FromBody]BrandViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newBrand = _mapper.Map<BrandViewModel, Brand>(model);
                    _context.Add(newBrand);
                    if (_context.SaveChanges() == 1)
                    {
                        return Created($"/api/Brand/{newBrand.BrandId}", _mapper.Map<Brand, BrandViewModel>(newBrand));
                    }
                    else
                    {
                        return BadRequest("Failed to create the brand");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when creating a new brand: {ex.Message}");
                return BadRequest($"Error when creating a new brand: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<BrandViewModel> Put([FromBody]BrandViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var updateBrand = _mapper.Map<BrandViewModel, Brand>(model);
                    _context.Update(updateBrand);
                    if(_context.SaveChanges() == 1)
                    {
                        return Ok(_mapper.Map<Brand, BrandViewModel>(updateBrand));
                    }
                    else
                    {
                        return BadRequest("Failed to update the brand");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when updating the brand: {ex.Message}");
                return BadRequest($"Error when updating the brand: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<BrandViewModel> Delete(int id)
        {
            try
            {
                var deleteBrand = _context.Brands.Where(b => b.BrandId == id).FirstOrDefault();
                if(deleteBrand != null) 
                {
                    _context.Remove(deleteBrand);
                    if(_context.SaveChanges() == 1)
                    {
                        return Ok(_mapper.Map<Brand, BrandViewModel>(deleteBrand));
                    }
                    else
                    {
                        return BadRequest("Failed to delete the brand");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when deleting the brand: {ex.Message}");
                return BadRequest($"Error when deleting the brand: {ex.Message}");
            }
        }
    }
}
