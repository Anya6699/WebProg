using Microsoft.AspNetCore.Mvc;

namespace WebProg.Components
{
    /// <summary>
    /// Компонент корзины заказа (ЛР 3, п. 5.6).
    /// На данном этапе просто возвращает представление без обработки данных.
    /// </summary>
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
