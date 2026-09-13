using OvningsbankApi.Models;

namespace OvningsbankApi.Dtos;

/// <summary>Det klienterna får tillbaka från API:et.</summary>
public class ExerciseReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ExerciseCategory Category { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public ExerciseStatus Status { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}
