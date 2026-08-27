using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Identity;

namespace RealEstate.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> UserProfiles => Set<User>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<PropertyImg> PropertyImages => Set<PropertyImg>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}

