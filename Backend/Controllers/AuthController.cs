using Backend.Core;
using Backend.Utils.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IAccountRepository _accountRepository;
    
    public AuthController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest model)
    {
        try
        {
            _accountRepository.Register(model, Request.Headers["origin"]);
            
            return Ok(new { message = "Registration successful" });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}