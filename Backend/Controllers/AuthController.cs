﻿using Backend.Core;
using Backend.Utils;
using Backend.Utils.Request;
using Backend.Utils.Response;
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
    
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest model)
    {
        var response = _accountRepository.Authenticate(model, ipAddress());
        
        setTokenCookie(response.RefreshToken);
        
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public ActionResult<AuthenticateResponse> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = _accountRepository.RefreshToken(refreshToken, ipAddress());
        
        setTokenCookie(response.RefreshToken);
        
        return Ok(response);
    }
    
    [HttpPost("logout")]
    public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        
        if (!Account.OwnsToken(token)) 
        {
            return BadRequest(new { message = "Token is required " });
        }
        _accountRepository.RevokeToken(token, ipAddress());
        
        return Ok(new { message = "Token revoked" });
    }
    
    private void setTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
    
    private string ipAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}