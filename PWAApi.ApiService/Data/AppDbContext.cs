using API.Models;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.Models;
using PWAApi.ApiService.Models.Taxonomy;

namespace EventApi.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentUser _currentUser;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUser currentUSer) : base(options)
        {
            _currentUser = currentUSer;
        }

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Reminder> Reminders { get; set; } = null!;
        public DbSet<Taxonomy> Taxonomy { get; set; } = null!;
        public DbSet<VernacularName> VernacularNames { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            UpdateUserIDs();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            UpdateUserIDs();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow; // Set CreatedAt for new entities
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow; // Update UpdatedAt for modified entities
                }
            }
        }

        private void UpdateUserIDs()
        {
            var entries = ChangeTracker.Entries<IUserAssociated>();
            foreach (var entry in entries)
            {
                entry.Entity.UserID = _currentUser.UserID;
            }
        }
    }
}