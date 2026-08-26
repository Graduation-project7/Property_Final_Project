namespace RealEstate.Application.DTOs.Auth;

public record RegisterRequest(string FirstName, string LastName, string Email, string PhoneNumber, string Password);
public record LoginRequest(string EmailOrPhone, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record AuthResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);