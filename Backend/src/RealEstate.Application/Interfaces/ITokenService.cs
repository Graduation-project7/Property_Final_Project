namespace RealEstate.Application.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateAccessToken(int userId, string email, IList<string> roles);
        string GenerateRefreshToken();
    }
}
