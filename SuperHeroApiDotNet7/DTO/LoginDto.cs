using System;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroApiDotNet7.DTO
{
	public class LoginDto
	{
        [Required(ErrorMessage = "User name is required!!!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!!!")]
        public string Password { get; set; }
    }
}

