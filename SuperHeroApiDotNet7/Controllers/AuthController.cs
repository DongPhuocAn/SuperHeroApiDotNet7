﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperHeroApiDotNet7.DTO;
using SuperHeroApiDotNet7.OtherObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperHeroApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Route for seeding my roles to db
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            if (isAdminRoleExists && isOwnerRoleExists && isUserRoleExists)
                return Ok("Roles Seeding is Already Done");

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));

            return Ok("Role Seeding done successfully");
        }

        // Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.Username);

            if (isExistsUser != null)
                return BadRequest("UserName Already Exists!!");

            IdentityUser newUser = new IdentityUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation failed Because: ";
                foreach(var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return BadRequest(errorString);
            }

            //Add a default USER Role to All users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

            return Ok("user Created Successfully");
        }


        //Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is null)
                return Unauthorized("Invalid Credentials");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!isPasswordCorrect)
                return Unauthorized("Invalid Credentials");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
            };

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return Ok(token);
        }


        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(

                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }





        





    }
}

