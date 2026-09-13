using System.ComponentModel.DataAnnotations;

namespace OvningsbankApi.Models;

/// <summary>
/// En övning i tränarens övningsbank.
/// Entiteten används bara internt - klienterna ser DTOerna i Dtos/.
/// </summary>
public class Exercise
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public ExerciseCategory Category { get; set; }

    public DifficultyLevel Difficulty { get; set; }

    public ExerciseStatus Status { get; set; } = ExerciseStatus.Planerad;

    /// <summary>
    /// Relativ webbsökväg till bilden, t.ex. "/uploads/3f2a....png".
    /// Null tills en bild laddats upp. Vi sparar sökvägen och inte filens
    /// bytes i databasen: statiska filer serveras snabbare av webbservern
    /// än genom en databasläsning, och databasfilen hålls liten.
    /// </summary>
    public string? ImagePath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
