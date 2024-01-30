using System;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroApiDotNet7.DTO
{
	public class RegisterDto
	{
        [Required(ErrorMessage = "FirstName is required!!!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required!!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User name is required!!!")]
		public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!!!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!!!")]
        public string Password { get; set; }
       
    }
}

