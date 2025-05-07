using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Authentication.Models;

namespace PWAApi.ApiService.Authentication.Data
{

    //To run migrations for the AuthDbContext, run from the main PWAApi.ApiService folder:
    //dotnet ef migrations add [MigrationName] --context AuthDbContext --output-dir Authentication/Migrations

    //To apply AuthDbContext migrations
    //dotnet ef database update --context AuthDbContext
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    }
}
