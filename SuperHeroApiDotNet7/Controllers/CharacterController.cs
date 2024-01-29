using Microsoft.AspNetCore.Mvc;
using SuperHeroApiDotNet7.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperHeroApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CharacterController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _dataContext.Characters
                .Where(c => c.UserId == userId)
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .ToListAsync();

            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create([FromBody] Character character)
        {
            _dataContext.Characters.Add(character);
            await _dataContext.SaveChangesAsync();

            return await Get(character.UserId);
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon([FromBody] AddWeaponDto request)
        {
            var character = await _dataContext.Characters.FindAsync(request.CharacterId);
            if(character == null)
            {
                return NotFound();
            }

            var newWeapon = new Weapon
            {
                Name = request.Name,
                Damage = request.Damage,
                Character = character
            };

            _dataContext.Weapons.Add(newWeapon);
            await _dataContext.SaveChangesAsync();

            return character;
        }


        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill([FromBody] AddCharacterSkillDto request)
        {
            var character = await _dataContext.Characters
                .Where(c => c.Id == request.CharacterId)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync();
            if (character == null)
            {
                return NotFound();
            }

            var skill = await _dataContext.Skills.FindAsync(request.SkillId);
            if (character == null)
            {
                return NotFound();
            }

            character.Skills.Add(skill);
            await _dataContext.SaveChangesAsync();

            return character;
        }
    }
}

