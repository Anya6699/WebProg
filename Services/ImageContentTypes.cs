using Microsoft.AspNetCore.StaticFiles;

namespace WebProg.Services
{
    /// <summary>
    /// Определение MIME-типа файла по его расширению (ЛР 5, п. 3.2.1).
    /// </summary>
    public static class ImageContentTypes
    {
        public const string Fallback = "application/octet-stream";

        // Чтение сопоставлений потокобезопасно, поэтому экземпляр общий
        private static readonly FileExtensionContentTypeProvider Provider = new();

        public static string Resolve(string fileName) =>
            Provider.TryGetContentType(fileName, out var contentType) ? contentType : Fallback;
    }
}
