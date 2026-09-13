using OvningsbankApi.Dtos;

namespace OvningsbankApi.Services;

/// <summary>
/// Applikationslogiken. Controllern känner bara till detta interface och
/// arbetar uteslutande med DTO:er - entiteten Exercise läcker aldrig ut ur
/// servicelagret.
/// </summary>
public interface IExerciseService
{
    Task<IReadOnlyList<ExerciseReadDto>> GetAllAsync();
    Task<ExerciseReadDto?> GetByIdAsync(int id);
    Task<ExerciseReadDto> CreateAsync(ExerciseCreateDto dto);
    Task<ExerciseReadDto?> UpdateAsync(int id, ExerciseUpdateDto dto);
    Task<ExerciseReadDto?> SetImageAsync(int id, string imagePath);
}
