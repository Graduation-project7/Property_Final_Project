using RealEstate.Application.DTOs.Auth;

namespace RealEstate.Application.Interfaces
{
    public interface ITokenService
    {
        TokenServiceResult GenerateAccessToken(int userId, string email, IList<string> roles);
        string GenerateRefreshToken();
        string HashToken(string token);

    }
}
