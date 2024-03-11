using System.ComponentModel.DataAnnotations;

namespace Backend.Utils.Request;

public class AuthenticateRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}