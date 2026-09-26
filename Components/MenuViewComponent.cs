using Microsoft.AspNetCore.Mvc;
using WebProg.Models;

namespace WebProg.Components
{
    /// <summary>
    /// Компонент главного меню сайта (ЛР 3, п. 5.2).
    /// Помечает активный пункт меню по текущему контроллеру или области.
    /// </summary>
    public class MenuViewComponent : ViewComponent
    {
        private const string ActiveCssClass = "active";
        private const string ControllerRouteKey = "controller";
        private const string AreaRouteKey = "area";

        // Исходные данные главного меню
        private readonly List<MenuItem> _menuItems = new List<MenuItem>
        {
            new MenuItem { Controller = "Home", Action = "Index", Text = "Lab 3" },
            new MenuItem { Controller = "Product", Action = "Index", Text = "Каталог" },
            new MenuItem { IsPage = true, Area = "Admin", Page = "/Index", Text = "Администрирование" }
        };

        public IViewComponentResult Invoke()
        {
            // Получение значений сегментов маршрута
            var controller = ViewContext.RouteData.Values[ControllerRouteKey]?.ToString();
            var area = ViewContext.RouteData.Values[AreaRouteKey]?.ToString();

            foreach (var item in _menuItems)
            {
                if (IsActive(item, controller, area))
                {
                    item.Active = ActiveCssClass;
                }
            }

            return View(_menuItems);
        }

        /// <summary>
        /// Пункт активен, если совпадает область (для страниц области)
        /// или контроллер (для обычных пунктов вне области).
        /// </summary>
        private static bool IsActive(MenuItem item, string? controller, string? area)
        {
            if (!string.IsNullOrEmpty(item.Area))
            {
                return IsMatch(item.Area, area);
            }

            return string.IsNullOrEmpty(area) && IsMatch(item.Controller, controller);
        }

        private static bool IsMatch(string expected, string? actual) =>
            !string.IsNullOrEmpty(actual)
            && string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
    }
}
