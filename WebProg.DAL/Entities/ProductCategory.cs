namespace WebProg.DAL.Entities
{
    /// <summary>
    /// Категория товаров (ЛР 5, задание 3).
    /// Одна категория описывает много товаров (один-ко-многим).
    /// </summary>
    public class ProductCategory
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Навигационное свойство 1-ко-многим.
        /// </summary>
        public List<Product> Products { get; set; } = new();
    }
}
