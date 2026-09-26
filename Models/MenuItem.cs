namespace WebProg.Models
{
    /// <summary>
    /// Описание элемента главного меню сайта (ЛР 3, п. 5.1).
    /// </summary>
    public class MenuItem
    {
        /// <summary>Является ли ссылка страницей Razor Page (иначе — метод контроллера).</summary>
        public bool IsPage { get; set; } = false;

        /// <summary>Имя области (Area).</summary>
        public string Area { get; set; } = string.Empty;

        /// <summary>Имя действия контроллера.</summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>Имя контроллера.</summary>
        public string Controller { get; set; } = string.Empty;

        /// <summary>Имя страницы Razor Page.</summary>
        public string Page { get; set; } = string.Empty;

        /// <summary>CSS-класс текущего (активного) пункта меню.</summary>
        public string Active { get; set; } = string.Empty;

        /// <summary>Текст надписи.</summary>
        public string Text { get; set; } = string.Empty;
    }
}
