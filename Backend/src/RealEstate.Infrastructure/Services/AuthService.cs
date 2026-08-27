using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealEstate.Application.Common.Settings;
using RealEstate.Application.DTOs.Auth;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Identity;

namespace RealEstate.Infrastructure.Services;

public class AuthService : IAuthService
{
    private const string DefaultRole = "Customer";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IApplicationDbContext context,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Email is already registered.");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var identityUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var result = await _userManager.CreateAsync(identityUser, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        var roleResult = await _userManager.AddToRoleAsync(identityUser, DefaultRole);
        if (!roleResult.Succeeded)
            throw new InvalidOperationException(string.Join(", ", roleResult.Errors.Select(e => e.Description)));

        _context.UserProfiles.Add(new User
        {
            Id = identityUser.Id, // shared PK
            FirstName = request.FirstName,
            LastName = request.LastName
        });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return await IssueTokensAsync(identityUser.Id, identityUser.Email!, new[] { DefaultRole });
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailOrPhone)
            ?? await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.EmailOrPhone);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var activeTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && rt.RevokedAt == null)
            .ToListAsync();

        foreach (var rt in activeTokens)
            rt.RevokedAt = DateTime.UtcNow;

        var roles = await _userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user.Id, user.Email!, roles);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var incomingHash = _tokenService.HashToken(refreshToken);

        var stored = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == incomingHash);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        var user = await _userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        stored.RevokedAt = DateTime.UtcNow; // rotation

        var roles = await _userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user.Id, user.Email!, roles);
    }



    public async Task LogoutAsync(int userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync();

        foreach (var rt in tokens)
            rt.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    private async Task<AuthResponse> IssueTokensAsync(int userId, string email, IEnumerable<string> roles)
    {
        var accessTokenResult = _tokenService.GenerateAccessToken(userId, email, roles.ToList());
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.HashToken(refreshTokenValue);

        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            TokenHash = refreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });

        await _context.SaveChangesAsync();

        return new AuthResponse(accessTokenResult.Token, refreshTokenValue, accessTokenResult.ExpiresAt);
    }

}