using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("test")]
[Authorize(Roles = nameof(Client))]
[ApiController]
public class TestController : ControllerBase
{
    public TestController()
    {
        
    }
    
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok("You got there!");
    }
}