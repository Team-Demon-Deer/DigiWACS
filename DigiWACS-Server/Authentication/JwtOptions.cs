// Authentication/JwtOptions.cs
namespace DigiWACS.Authentication {

public record class JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int ExpirationSeconds
);
}
