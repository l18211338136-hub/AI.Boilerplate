using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Shared.Features.Identity.Dtos;
using AI.Boilerplate.Shared.Infrastructure.Services;
using AI.Boilerplate.Server.Api.Infrastructure.Data;
using AI.Boilerplate.Server.Api.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace AI.Boilerplate.Server.Api.Features.Identity;

public class IdentitySeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        // 1. Seed Roles
        var superAdminRoleId = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7");
        var demoRoleId = Guid.Parse("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8");

        var roles = new List<Role>
        {
            new() { Id = superAdminRoleId, Name = AppRoles.SuperAdmin, NormalizedName = AppRoles.SuperAdmin.ToUpperInvariant(), ConcurrencyStamp = superAdminRoleId.ToString() },
            new() { Id = demoRoleId, Name = AppRoles.Demo, NormalizedName = AppRoles.Demo.ToUpperInvariant(), ConcurrencyStamp = demoRoleId.ToString() }
        };

        foreach (var role in roles)
        {
            if (await dbContext.Roles.AnyAsync(r => r.Id == role.Id, cancellationToken) is false)
            {
                await dbContext.Roles.AddAsync(role);
            }
        }
        await dbContext.SaveChangesAsync(cancellationToken);

        // 2. Seed Users
        var defaultTestUserId = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7");
        const string userName = "test";
        const string email = "761516331@qq.com";

        if (await dbContext.Users.AnyAsync(u => u.Id == defaultTestUserId, cancellationToken) is false)
        {
            var user = new User
            {
                Id = defaultTestUserId,
                EmailConfirmed = true,
                LockoutEnabled = true,
                Gender = Gender.Other,
                BirthDate = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
                FullName = "超级管理员",
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                EmailTokenRequestedOn = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
                PhoneNumber = "+8618211338136",
                PhoneNumberConfirmed = true,
                SecurityStamp = "959ff4a9-4b07-4cc1-8141-c5fc033daf83",
                ConcurrencyStamp = "315e1a26-5b3a-4544-8e91-2760cd28e231",
                PasswordHash = "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", // 123456
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // 3. Seed UserRoles
        if (await dbContext.UserRoles.AnyAsync(ur => ur.UserId == defaultTestUserId && ur.RoleId == superAdminRoleId, cancellationToken) is false)
        {
            await dbContext.UserRoles.AddAsync(new UserRole { RoleId = superAdminRoleId, UserId = defaultTestUserId });
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // 4. Seed RoleClaims
        if (await dbContext.RoleClaims.AnyAsync(rc => rc.RoleId == superAdminRoleId && rc.ClaimType == AppClaimTypes.MAX_PRIVILEGED_SESSIONS, cancellationToken) is false)
        {
            await dbContext.RoleClaims.AddAsync(new RoleClaim
            {
                ClaimType = AppClaimTypes.MAX_PRIVILEGED_SESSIONS,
                ClaimValue = "-1",
                RoleId = superAdminRoleId
            });
        }

        var demoFeatures = AppFeatures.GetAll()
            .Where(f => f.Group != typeof(AppFeatures.System)
                     && f.Group != typeof(AppFeatures.Management))
            .ToList();

        foreach (var feature in demoFeatures)
        {
            if (await dbContext.RoleClaims.AnyAsync(rc => rc.RoleId == demoRoleId && rc.ClaimType == AppClaimTypes.FEATURES && rc.ClaimValue == feature.Value, cancellationToken) is false)
            {
                await dbContext.RoleClaims.AddAsync(new RoleClaim
                {
                    ClaimType = AppClaimTypes.FEATURES,
                    ClaimValue = feature.Value,
                    RoleId = demoRoleId
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
