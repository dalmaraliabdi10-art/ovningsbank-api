using OvningsbankApi.Data;
using OvningsbankApi.Dtos;
using OvningsbankApi.Models;

namespace OvningsbankApi.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _repository;

    public ExerciseService(IExerciseRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<ExerciseReadDto>> GetAllAsync()
    {
        var exercises = await _repository.GetAllAsync();
        return exercises.Select(ToDto).ToList();
    }

    public async Task<ExerciseReadDto?> GetByIdAsync(int id)
    {
        var exercise = await _repository.GetByIdAsync(id);
        return exercise is null ? null : ToDto(exercise);
    }

    public async Task<ExerciseReadDto> CreateAsync(ExerciseCreateDto dto)
    {
        var exercise = new Exercise
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            Category = dto.Category,
            Difficulty = dto.Difficulty,
            Status = ExerciseStatus.Planerad,   // en ny övning är alltid oplanerad från början
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(exercise);
        return ToDto(created);
    }

    public async Task<ExerciseReadDto?> UpdateAsync(int id, ExerciseUpdateDto dto)
    {
        var exercise = await _repository.GetByIdAsync(id);
        if (exercise is null) return null;

        exercise.Title = dto.Title.Trim();
        exercise.Description = dto.Description.Trim();
        exercise.Category = dto.Category;
        exercise.Difficulty = dto.Difficulty;
        exercise.Status = dto.Status;
        // ImagePath rör vi inte här - bilden byts via upload-endpointen,
        // så en vanlig redigering kan aldrig rada bort en uppladdad bild av misstag.

        await _repository.UpdateAsync(exercise);
        return ToDto(exercise);
    }

    public async Task<ExerciseReadDto?> SetImageAsync(int id, string imagePath)
    {
        var exercise = await _repository.GetByIdAsync(id);
        if (exercise is null) return null;

        exercise.ImagePath = imagePath;
        await _repository.UpdateAsync(exercise);
        return ToDto(exercise);
    }

    private static ExerciseReadDto ToDto(Exercise e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        Description = e.Description,
        Category = e.Category,
        Difficulty = e.Difficulty,
        Status = e.Status,
        ImagePath = e.ImagePath,
        CreatedAt = e.CreatedAt
    };
}
