using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> UserProfiles { get; }
        DbSet<Property> Properties { get; }
        DbSet<PropertyImg> PropertyImages { get; }
        DbSet<Favorite> Favorites { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

