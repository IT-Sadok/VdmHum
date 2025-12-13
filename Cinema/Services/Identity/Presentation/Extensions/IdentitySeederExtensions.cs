namespace Presentation.Extensions;

using Application.Options;
using Application.Errors.Constants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public static class IdentitySeederExtensions
{
    public static async Task SeedAdminAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminUserOptions>>().Value;

        const string adminRoleName = RoleNames.Admin;

        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(adminRoleName));
        }

        var adminUser = await userManager.FindByEmailAsync(adminOptions.Email);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminOptions.Email,
                Email = adminOptions.Email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, adminOptions.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
        {
            await userManager.AddToRoleAsync(adminUser, adminRoleName);
        }
    }
}