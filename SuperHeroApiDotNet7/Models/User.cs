﻿using System;
namespace SuperHeroApiDotNet7.Models
{
	public class User
	{
		public int Id { get; set; }
		public string UserName { get; set; } = string.Empty;
		public List<Character> Characters { get; set; }
	}
}

