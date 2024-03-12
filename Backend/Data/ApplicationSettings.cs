namespace Backend.Data;

public class ApplicationSettings
{
    public string Secret { get; set; }
    public int RefreshTokenTTL { get; set; }
}