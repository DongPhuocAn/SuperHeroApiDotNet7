using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperHeroApiDotNet7.OtherObjects;

namespace SuperHeroApiDotNet7.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  

    [HttpGet(Name = "Get")]
    public IActionResult Get()
    {
        return Ok(Summaries);
    }

    [HttpGet]
    [Route("UserRole")]
    [Authorize(Roles = StaticUserRoles.USER)]
    public IActionResult GetUserRole()
    {
        return Ok(Summaries);
    }

    
    [HttpGet]
    [Route("AdminRole")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public IActionResult GetAdminRole()
    {
        return Ok(Summaries);
    }

    [HttpGet]
    [Route("OwnerRole")]
    [Authorize(Roles = StaticUserRoles.OWNER)]
    public IActionResult GetOwnerRole()
    {
        return Ok(Summaries);
    }


}

