using System.Text.Json.Serialization;

namespace Backend.Utils.Response;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string JwtToken { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
}