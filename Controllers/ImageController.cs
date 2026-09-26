using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProg.DAL.Entities;
using WebProg.Services;

namespace WebProg.Controllers
{
    /// <summary>
    /// Передача изображений клиенту (ЛР 5, п. 5.3.1).
    /// </summary>
    public class ImageController : Controller
    {
        // Общий аватар для пользователей без собственного изображения
        private const string DefaultAvatarPath = "images/avatar.png";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImageController> _logger;

        public ImageController(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env,
            ILogger<ImageController> logger)
        {
            _userManager = userManager;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Аватар текущего пользователя из БД, а при его отсутствии — общий аватар.
        /// Кэширование отключено: адрес одинаков для всех пользователей.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetAvatar()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.AvatarImage is { Length: > 0 } avatar)
            {
                return File(avatar, user.AvatarMimeType ?? ImageContentTypes.Fallback);
            }

            return GetDefaultAvatar();
        }

        private IActionResult GetDefaultAvatar()
        {
            var fileInfo = _env.WebRootFileProvider.GetFileInfo(DefaultAvatarPath);
            if (!fileInfo.Exists)
            {
                _logger.LogError("Не найден файл общего аватара {Path}", DefaultAvatarPath);
                return NotFound();
            }

            return File(fileInfo.CreateReadStream(), ImageContentTypes.Resolve(fileInfo.Name));
        }
    }
}
