namespace BuberDinner.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public required string Secret { get; init; }

    public uint ExpiryMinutes { get; init; }

    public required string Issuer { get; init; }

    public required string Audience { get; init; }
}