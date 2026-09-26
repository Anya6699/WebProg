using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProg.DAL.Data;
using WebProg.DAL.Entities;
using WebProg.Services;

const string ConnectionStringName = "DefaultConnection";
const string LoginPath = "/Identity/Account/Login";
const string LogoutPath = "/Identity/Account/Logout";
const string AccessDeniedPath = "/Identity/Account/AccessDenied";

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)
    ?? throw new InvalidOperationException($"Строка подключения '{ConnectionStringName}' не найдена.");

// Контекст БД (ЛР 4, п. 5.3)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity: свой класс пользователя, роли и простые пароли
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Пути к страницам аутентификации в куки (ЛР 4, п. 5.7)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = LoginPath;
    options.LogoutPath = LogoutPath;
    options.AccessDeniedPath = AccessDeniedPath;
});

// Чтение загружаемых изображений (аватар при регистрации, ЛР 5)
builder.Services.AddSingleton<IImageFileReader, ImageFileReader>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Инициализация БД начальными данными — перед настройкой конечных точек (ЛР 4, п. 5.6)
await SeedDatabaseAsync(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Scoped-сервисы можно получить только внутри scope (вариант 2 из методички)
static async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");

    try
    {
        await DbInitializer.Seed(
            services.GetRequiredService<ApplicationDbContext>(),
            services.GetRequiredService<UserManager<ApplicationUser>>(),
            services.GetRequiredService<RoleManager<IdentityRole>>(),
            logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка при инициализации базы данных");
        throw;
    }
}
