namespace WebProg.Services
{
    /// <summary>Результат чтения загруженного изображения.</summary>
    public sealed class ImageReadResult
    {
        private ImageReadResult(byte[]? content, string? mimeType, string? error)
        {
            Content = content;
            MimeType = mimeType;
            Error = error;
        }

        public byte[]? Content { get; }
        public string? MimeType { get; }
        public string? Error { get; }
        public bool Succeeded => Error == null;

        public static ImageReadResult Success(byte[] content, string mimeType) => new(content, mimeType, null);
        public static ImageReadResult Failure(string error) => new(null, null, error);
    }

    /// <summary>Проверка и чтение изображения, загруженного через форму.</summary>
    public interface IImageFileReader
    {
        Task<ImageReadResult> ReadAsync(IFormFile file, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Читает изображение из IFormFile в массив байтов (ЛР 5, п. 5.2.2).
    /// Разрешены только растровые форматы: SVG может содержать скрипты.
    /// </summary>
    public class ImageFileReader : IImageFileReader
    {
        public const long MaxFileSizeBytes = 2 * 1024 * 1024;

        private static readonly HashSet<string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/png", "image/jpeg", "image/gif", "image/webp", "image/bmp"
        };

        private readonly ILogger<ImageFileReader> _logger;

        public ImageFileReader(ILogger<ImageFileReader> logger)
        {
            _logger = logger;
        }

        public async Task<ImageReadResult> ReadAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var validationError = Validate(file, out var mimeType);
            if (validationError != null)
            {
                _logger.LogWarning("Изображение отклонено: {Reason}", validationError);
                return ImageReadResult.Failure(validationError);
            }

            // CopyToAsync гарантированно читает поток целиком (один ReadAsync — нет)
            using var memory = new MemoryStream((int)file.Length);
            await file.CopyToAsync(memory, cancellationToken);
            return ImageReadResult.Success(memory.ToArray(), mimeType);
        }

        private static string? Validate(IFormFile file, out string mimeType)
        {
            mimeType = ImageContentTypes.Resolve(file.FileName);

            if (file.Length == 0)
            {
                return "Файл пустой";
            }

            if (file.Length > MaxFileSizeBytes)
            {
                return $"Размер файла не должен превышать {MaxFileSizeBytes / 1024 / 1024} МБ";
            }

            return AllowedMimeTypes.Contains(mimeType)
                ? null
                : "Допустимы только изображения PNG, JPEG, GIF, WEBP или BMP";
        }
    }
}
