namespace OvningsbankApi.Services;

/// <summary>Resultatet av ett sparningsförsök - antingen en sökväg eller ett fel att visa för användaren.</summary>
public record FileSaveResult(bool Success, string? RelativePath, string? Error);

public interface IFileStorageService
{
    Task<FileSaveResult> SaveImageAsync(IFormFile file);
}
