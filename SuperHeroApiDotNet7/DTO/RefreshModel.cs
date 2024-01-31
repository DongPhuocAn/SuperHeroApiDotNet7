using System;
namespace SuperHeroApiDotNet7.DTO
{
	public class RefreshModel
	{
		public required string AccessToken { get; set; }
		public required string RefreshToken { get; set; }
	}
}

