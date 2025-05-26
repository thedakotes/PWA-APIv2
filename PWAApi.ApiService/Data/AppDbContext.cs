using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.Models;
using PWAApi.ApiService.Models.Events;
using PWAApi.ApiService.Models.Events.Reminder;
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

        #region DbSet

        public DbSet<CalendarEvent> CalendarEvents { get; set; } = null!;
        public DbSet<Reminder> Reminders { get; set; } = null!;
        public DbSet<ReminderItem> ReminderItems { get; set; } = null!;
        public DbSet<ReminderTask> ReminderTasks { get; set; } = null!;
        public DbSet<Taxonomy> Taxonomy { get; set; } = null!;
        public DbSet<VernacularName> VernacularNames { get; set; } = null!;

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public override int SaveChanges()
        {
            UpdateEntities();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        #region Updates

        private void UpdateEntities()
        {
            UpdateCompletedOn();
            UpdateTimestamps();
            UpdateUserIDs();
        }

        private void UpdateCompletedOn()
        {
            var entries = ChangeTracker.Entries<ICompletable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.CompletedOn = DateTime.UtcNow;
                }
            }
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow; // Set CreatedAt for new entities
                    entry.Entity.UpdatedOn = entry.Entity.CreatedOn; //Since we don't allow nulls for UpdatedAt, we'll make it equal to the CreatedAt date
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = DateTime.UtcNow; // Update UpdatedAt for modified entities
                }
            }
        }

        private void UpdateUserIDs()
        {
            var entries = ChangeTracker.Entries<IUserAssociated>();
            foreach (var entry in entries)
            {
                entry.Entity.UserId = _currentUser.UserID;
            }
        }

        #endregion
    }
}