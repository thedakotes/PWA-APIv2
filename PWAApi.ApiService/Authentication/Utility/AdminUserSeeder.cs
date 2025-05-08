using Microsoft.AspNetCore.Identity;
using PWAApi.ApiService.Authentication.DataTransferObjects;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.Middleware;

namespace PWAApi.ApiService.Authentication.Utility
{
   
    public class AdminUserSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminUserSeeder> _logger;
        private readonly IConfiguration _config;

        public AdminUserSeeder(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               ILogger<AdminUserSeeder> logger,
                               IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _config = configuration;
        }

        public async Task SeedAdminUserAsync()
        {
            // Define admin credentials (better to store these in appsettings.json or environment variables)
            var adminUsers = _config.GetSection("AdminUsers").Get<List<AdminUserDTO>>();
            if (adminUsers == null || !adminUsers.Any())
            {
                _logger.LogError("Could not retrieve admin users from config.");
                return;
            }

            // Define admin roles
            string[] adminRoles = new[] { "Admin", "SystemManager", "ContentManager" };

            // Create roles if they don't exist
            foreach (var roleName in adminRoles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    _logger.LogInformation($"Created role: {roleName}");
                }
            }

            foreach (AdminUserDTO a in adminUsers)
            {
                // Check if admin user exists
                var adminUser = await _userManager.FindByEmailAsync(a.Email);

                if (adminUser == null)
                {
                    // Create the admin user
                    adminUser = new ApplicationUser
                    {
                        UserName = a.Email,
                        Email = a.Email,
                        EmailConfirmed = true,
                        Name = a.Name ?? "System Administrator",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    var result = await _userManager.CreateAsync(adminUser, a.Password);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Admin user created: {a.Email}");

                        // Assign all admin roles to the user
                        foreach (var role in adminRoles)
                        {
                            await _userManager.AddToRoleAsync(adminUser, role);
                            _logger.LogInformation($"Added role {role} to admin user {adminUser.Email}");
                        }
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        _logger.LogError($"Failed to create admin user {adminUser.Email}: {errors}");
                    }
                }
                else
                {
                    if(!string.IsNullOrWhiteSpace(adminUser.Provider) || !string.IsNullOrWhiteSpace(adminUser.ProviderId))
                    {
                        //Our admin user already exists and was logged in with Google auth. We don't want to use this as an admin user
                        //I suppose we could just delete the existing user and recreate it with the new admin credentials. I'm not sure 
                        //how we want to handle this.
                        _logger.LogError($"User {adminUser.Email} already exists but was previously logged in with Google Auth. Can not make this user an admin.");
                        return;
                    }

                    //Update anything that needs updating:
                    adminUser.EmailConfirmed = true;
                    adminUser.Name = a.Name ?? "System Administrator";
                    await _userManager.UpdateAsync(adminUser);

                    // Ensure admin has all the required roles
                    foreach (var role in adminRoles)
                    {
                        if (!await _userManager.IsInRoleAsync(adminUser, role))
                        {
                            await _userManager.AddToRoleAsync(adminUser, role);
                            _logger.LogInformation($"Added missing role {role} to existing admin user {adminUser.Email}");
                        }
                    }
                }
            }
        }
    }
}
