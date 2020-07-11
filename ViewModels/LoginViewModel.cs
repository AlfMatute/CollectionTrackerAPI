using CollectionTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionTrackerAPI.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public CollectionUser CollectionUser { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Register { get; set; }
    }
}
