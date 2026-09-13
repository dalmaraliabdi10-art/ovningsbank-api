namespace OvningsbankApi.Services;

/// <summary>
/// Sparar uppladdade bilder i wwwroot/uploads och returnerar en relativ webbsökväg.
/// Filnamnet slumpas med en GUID: två tränare som laddar upp "taktik.png" skriver
/// då inte över varandras bilder och ett filnamn från klienten kan inte användas
/// för att skriva utanför uppladdningsmappen.
/// </summary>
public class FileStorageService : IFileStorageService
{
    private const string UploadFolder = "uploads";

    private readonly IWebHostEnvironment _env;
    private readonly long _maxBytes;
    private readonly string[] _allowedExtensions;

    public FileStorageService(IWebHostEnvironment env, IConfiguration config)
    {
        _env = env;
        _maxBytes = config.GetValue<long?>("Upload:MaxBytes") ?? 5 * 1024 * 1024;
        _allowedExtensions = config.GetSection("Upload:AllowedExtensions").Get<string[]>()
                             ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    }

    public async Task<FileSaveResult> SaveImageAsync(IFormFile file)
    {
        if (file.Length == 0)
            return new FileSaveResult(false, null, "Filen är tom.");

        if (file.Length > _maxBytes)
            return new FileSaveResult(false, null,
                $"Filen är för stor. Högst {_maxBytes / (1024 * 1024)} MB tillåts.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            return new FileSaveResult(false, null,
                $"Filtypen {extension} stöds inte. Tillåtna typer: {string.Join(", ", _allowedExtensions)}.");

        // WebRootPath är null om wwwroot saknas vid uppstart - skapa den då.
        var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var targetFolder = Path.Combine(webRoot, UploadFolder);
        Directory.CreateDirectory(targetFolder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(targetFolder, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return new FileSaveResult(true, $"/{UploadFolder}/{fileName}", null);
    }
}
