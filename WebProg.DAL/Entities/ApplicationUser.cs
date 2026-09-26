using Microsoft.AspNetCore.Identity;

namespace WebProg.DAL.Entities
{
    /// <summary>
    /// Пользователь приложения (ЛР 4, п. 5.2.1).
    /// Используется вместо стандартного IdentityUser.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>Изображение аватара пользователя (ЛР 5, п. 5.2.1).</summary>
        public byte[]? AvatarImage { get; set; }

        /// <summary>MIME-тип изображения аватара, например «image/png».</summary>
        public string? AvatarMimeType { get; set; }
    }
}
