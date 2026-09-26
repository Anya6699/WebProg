using Microsoft.AspNetCore.Mvc;
using WebProg.DAL.Entities;
using WebProg.Services;

namespace WebProg.Controllers
{
    /// <summary>
    /// Каталог товаров фермерской лавки (ЛР 5, задание 4).
    /// На данном этапе данные хранятся в списках внутри контроллера.
    /// </summary>
    public class ProductController : Controller
    {
        private List<ProductCategory> _categories = new();
        private List<Product> _products = new();

        public ProductController()
        {
            SetupData();
        }

        public IActionResult Index()
        {
            return View(_products);
        }

        /// <summary>
        /// Инициализация списков.
        /// </summary>
        private void SetupData()
        {
            _categories = new List<ProductCategory>
            {
                new ProductCategory { CategoryId = 1, CategoryName = "Фрукты" },
                new ProductCategory { CategoryId = 2, CategoryName = "Ягоды" },
                new ProductCategory { CategoryId = 3, CategoryName = "Овощи" }
            };

            _products = new List<Product>
            {
                CreateProduct(1, "Яблоко «Антоновка»", "Кисло-сладкое, из сада под Минском", 3.20m, 1, "apple.svg"),
                CreateProduct(2, "Апельсин", "Сочный, без косточек", 5.90m, 1, "orange.svg"),
                CreateProduct(3, "Клубника", "Садовая, собрана утром", 12.50m, 2, "strawberry.svg"),
                CreateProduct(4, "Виноград «Кишмиш»", "Сладкий, без косточек", 8.40m, 2, "grapes.svg"),
                CreateProduct(5, "Морковь", "Молодая, мытая", 1.80m, 3, "carrot.svg")
            };
        }

        private Product CreateProduct(
            int id, string name, string description, decimal price, int categoryId, string image) =>
            new Product
            {
                ProductId = id,
                Name = name,
                Description = description,
                Price = price,
                CategoryId = categoryId,
                Category = _categories.FirstOrDefault(c => c.CategoryId == categoryId),
                Image = image,
                MimeType = ImageContentTypes.Resolve(image)
            };
    }
}
