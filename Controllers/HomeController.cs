using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebProg.Models;

namespace WebProg.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<ListDemo> _listDemo;

        public HomeController()
        {
            _listDemo = new List<ListDemo>
            {
                new ListDemo { ListItemValue = 1, ListItemText = "Item 1" },
                new ListDemo { ListItemValue = 2, ListItemText = "Item 2" },
                new ListDemo { ListItemValue = 3, ListItemText = "Item 3" }
            };
        }

        public IActionResult Index()
        {
            ViewData["Text"] = "Лабораторная работа 4";
            ViewData["Lst"] = new SelectList(
                _listDemo,
                nameof(ListDemo.ListItemValue),
                nameof(ListDemo.ListItemText));
            return View();
        }

        // Используется обработчиком ошибок из Program.cs (app.UseExceptionHandler("/Home/Error"))
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
