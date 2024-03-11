using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
public class BaseController : ControllerBase
{
    public User Account => (User)HttpContext.Items["Account"];
}