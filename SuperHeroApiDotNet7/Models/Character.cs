using System;
using System.Text.Json.Serialization;

namespace SuperHeroApiDotNet7.Models
{
	public class Character
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

        public string RggClass { get; set; } = "Knight";

		[JsonIgnore]
        public User User { get; set; }

		public int UserId { get; set; }

		public Weapon Weapon { get; set; }

		public List<Skill> Skills { get; set; }
	}
}

