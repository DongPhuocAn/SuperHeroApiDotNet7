using System;
using System.ComponentModel.DataAnnotations;

namespace SuperHeroApiDotNet7.DTO
{
	public class UpdatePermissionDto
	{
        [Required(ErrorMessage = "User name is required!!!")]
        public string Username { get; set; }

	}
}

