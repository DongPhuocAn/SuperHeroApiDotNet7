using System;
namespace SuperHeroApiDotNet7.DTO
{
	public class AuthServiceResponseDto
	{
		public bool IsSucceed { get; set; }
		public string? Message { get; set; }
		public string? RefreshToken { get; set; }
	}
}

