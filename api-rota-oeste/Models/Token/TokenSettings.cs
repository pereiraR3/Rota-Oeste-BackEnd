namespace api_rota_oeste.Models.Token;

public class TokenSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public int TokenExpirationInMinutes { get; set; }
    
    public int RefreshTokenExpirationInDays { get; set; }
}