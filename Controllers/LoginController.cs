using System;
using CollectionTrackerAPI.Models;
using CollectionTrackerAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CollectionTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<CollectionUser> _signInManager;
        private readonly UserManager<CollectionUser> _userManager;
        private readonly IConfiguration _config;

        public LoginController(ILogger<LoginController> logger
            , SignInManager<CollectionUser> signInManager
            , UserManager<CollectionUser> userManager
            , IConfiguration configuration)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = configuration;
        }

        [HttpPost]
        public ActionResult Create([FromBody] LoginViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (model.Register)
                    {
                        CollectionUser user = _userManager.FindByEmailAsync(model.Email).Result;

                        if (user == null)
                        {
                            user = new CollectionUser()
                            {
                                FirstName = model.FirstName
                                ,
                                LastName = model.LastName
                                ,
                                Email = model.Email
                                ,
                                UserName = model.Email
                            };
                        }

                        var result = _userManager.CreateAsync(user, model.Password).Result;
                        if (!result.Succeeded)
                        {
                            return BadRequest("Failed to create a new user");
                        }
                    }
                    else
                    {
                    var result = _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).Result;

                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Failed to login");
                        return BadRequest(ModelState);
                    }
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error on creating a new user: {ex.Message}");
                return BadRequest($"Error on creating a new user: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                _signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error signing out: {ex.Message}");
                return BadRequest($"Error signing out: {ex.Message}");
            }
        }
    }
}
