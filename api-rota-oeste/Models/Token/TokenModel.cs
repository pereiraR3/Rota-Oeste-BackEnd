namespace api_rota_oeste.Models.Token;

public class TokenModel
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string Username { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
}