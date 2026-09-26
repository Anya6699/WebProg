namespace WebProg.DAL.Entities
{
    /// <summary>
    /// Товар фермерской лавки — сущность предметной области (ЛР 5, задание 3).
    /// </summary>
    public class Product
    {
        /// <summary>Уникальный номер товара.</summary>
        public int ProductId { get; set; }

        /// <summary>Короткое название товара.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Дополнительное описание товара.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>Цена за 1 кг, руб.</summary>
        public decimal Price { get; set; }

        /// <summary>Имя файла изображения (из папки wwwroot/images).</summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>MIME-тип изображения.</summary>
        public string MimeType { get; set; } = string.Empty;

        // Навигационные свойства
        /// <summary>
        /// Категория товара (например, фрукты, ягоды, овощи).
        /// </summary>
        public int CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
    }
}
