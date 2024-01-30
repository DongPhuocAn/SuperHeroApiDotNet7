using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperHeroApiDotNet7.DTO;
using SuperHeroApiDotNet7.OtherObjects;
using SuperHeroApiDotNet7.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperHeroApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Route for seeding my roles to db
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedrole = await _authService.SeedRolesAsync();
            return Ok(seedrole);
        }

        // Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var listErr = new Dictionary<string, string>();
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    //foreach (var error in modelStateVal.Errors)
                    //{
                    //    var key = modelStateKey;
                    //    var errorMessage = error.ErrorMessage;
                    //}
                    listErr.Add(modelStateKey, modelStateVal.Errors[0].ErrorMessage);
                }
                
                string errorMsg = "";
                foreach(KeyValuePair<string, string> map in listErr)
                {
                    errorMsg += map.Key + ": " + map.Value + Environment.NewLine;
                }
                return BadRequest(new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorMsg

                });
            }
            var registerResult = await _authService.RegisterAsync(registerDto);
            if (registerResult.IsSucceed)
                return Ok(registerResult);

            return BadRequest(registerResult);
        }


        //Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);
            if (loginResult.IsSucceed)
                return Ok(loginResult);

            return Unauthorized(loginResult);
        }


        // Route -> make user -> admin
        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var makeAdminResult = await _authService.MakeAdminAsync(updatePermissionDto);
            if (makeAdminResult.IsSucceed)
                return Ok(makeAdminResult);

            return BadRequest(makeAdminResult);
        }

        // Route -> make user -> owner
        [HttpPost]
        [Route("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var makeOwnerResult = await _authService.MakeOwnerAsync(updatePermissionDto);
            if (makeOwnerResult.IsSucceed)
                return Ok(makeOwnerResult);

            return BadRequest(makeOwnerResult);
        }








    }
}

