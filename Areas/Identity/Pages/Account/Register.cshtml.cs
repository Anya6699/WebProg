using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProg.DAL.Entities;
using WebProg.Services;

namespace WebProg.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Регистрация нового пользователя.
    /// Код, использующий IEmailSender (подтверждение email), удален по заданию ЛР 4.
    /// </summary>
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const int MinPasswordLength = 6;
        private const int MaxPasswordLength = 100;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageFileReader _imageFileReader;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IImageFileReader imageFileReader,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageFileReader = imageFileReader;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ReturnUrl { get; set; } = string.Empty;

        public class InputModel
        {
            [Required(ErrorMessage = "Введите email")]
            [EmailAddress(ErrorMessage = "Некорректный email")]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите пароль")]
            [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength,
                ErrorMessage = "Пароль должен содержать от {2} до {1} символов")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; } = string.Empty;

            /// <summary>Файл аватара (ЛР 5, п. 5.2.2). Необязательный.</summary>
            [Display(Name = "Аватар")]
            public IFormFile? Avatar { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
            if (!await TryAttachAvatarAsync(user))
            {
                return Page();
            }

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            _logger.LogInformation("Создан новый пользователь");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Сохраняет выбранный файл аватара в свойствах пользователя.
        /// Возвращает false, если файл не прошел проверку.
        /// </summary>
        private async Task<bool> TryAttachAvatarAsync(ApplicationUser user)
        {
            if (Input.Avatar == null)
            {
                return true;
            }

            var image = await _imageFileReader.ReadAsync(Input.Avatar, HttpContext.RequestAborted);
            if (!image.Succeeded)
            {
                ModelState.AddModelError($"{nameof(Input)}.{nameof(InputModel.Avatar)}", image.Error!);
                return false;
            }

            user.AvatarImage = image.Content;
            user.AvatarMimeType = image.MimeType;
            return true;
        }
    }
}
