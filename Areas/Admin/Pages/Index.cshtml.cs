using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProg.DAL.Constants;

namespace WebProg.Areas.Admin.Pages
{
    /// <summary>
    /// Страница администрирования: пример ограничения доступа по роли (ЛР 4, задание 1).
    /// </summary>
    [Authorize(Roles = RoleNames.Admin)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
