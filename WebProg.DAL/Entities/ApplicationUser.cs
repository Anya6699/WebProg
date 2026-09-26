using Microsoft.AspNetCore.Identity;

namespace WebProg.DAL.Entities
{
    /// <summary>
    /// Пользователь приложения (ЛР 4, п. 5.2.1).
    /// Используется вместо стандартного IdentityUser.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
    }
}
