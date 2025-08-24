namespace TeamTask_API.Infrastructure.Security;
public class JwtSettings
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public int ExpiryMinutes { get; set; } = 60;
}