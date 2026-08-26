namespace RealEstate.Application.DTOs.Auth
{
    public record TokenServiceResult(string Token, DateTime ExpiresAt);
}
