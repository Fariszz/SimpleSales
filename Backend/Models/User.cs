namespace Backend.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    
    // Navigation properti
    public Role? Role { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    
    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
}