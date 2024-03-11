using Backend.Models;

namespace Backend.Core;

public interface IJwtRepository
{
    public string GenerateToken(User user);
    public int? ValidateToken(string? token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}