using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProg.DAL.Constants;
using WebProg.DAL.Data;
using WebProg.DAL.Entities;

namespace WebProg.Services
{
    /// <summary>
    /// Заполнение базы данных начальными данными (ЛР 4, п. 5.6):
    /// роль «admin» и два пользователя, один из которых — администратор.
    /// </summary>
    public static class DbInitializer
    {
        // Учебные учетные записи из методички. Пароли простые намеренно —
        // в реальном проекте их нельзя хранить в коде.
        private const string UserEmail = "user@mail.ru";
        private const string AdminEmail = "admin@mail.ru";
        private const string DefaultPassword = "123456";

        public static async Task Seed(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger logger)
        {
            await EnsureDatabaseAsync(context, logger);
            await EnsureRoleAsync(roleManager, RoleNames.Admin, logger);
            await EnsureUserAsync(userManager, UserEmail, role: null, logger);
            await EnsureUserAsync(userManager, AdminEmail, RoleNames.Admin, logger);
        }

        /// <summary>
        /// Создает БД, если ее еще нет. Если в DAL есть миграции — применяет их,
        /// иначе создает схему напрямую (EnsureCreated).
        /// </summary>
        private static async Task EnsureDatabaseAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Database.GetMigrations().Any())
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Миграции базы данных применены");
                return;
            }

            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("База данных создана без миграций (EnsureCreated)");
        }

        private static async Task EnsureRoleAsync(
            RoleManager<IdentityRole> roleManager, string roleName, ILogger logger)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                return;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            ThrowIfFailed(result, $"создание роли «{roleName}»");
            logger.LogInformation("Создана роль {Role}", roleName);
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager, string email, string? role, ILogger logger)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { Email = email, UserName = email };
                var createResult = await userManager.CreateAsync(user, DefaultPassword);
                ThrowIfFailed(createResult, $"создание пользователя {email}");
                logger.LogInformation("Создан пользователь {Email}", email);
            }

            if (role == null || await userManager.IsInRoleAsync(user, role))
            {
                return;
            }

            var roleResult = await userManager.AddToRoleAsync(user, role);
            ThrowIfFailed(roleResult, $"назначение роли «{role}» пользователю {email}");
            logger.LogInformation("Пользователю {Email} назначена роль {Role}", email, role);
        }

        private static void ThrowIfFailed(IdentityResult result, string operation)
        {
            if (result.Succeeded)
            {
                return;
            }

            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Ошибка при операции «{operation}»: {errors}");
        }
    }
}
