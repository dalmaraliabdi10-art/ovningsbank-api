using System.ComponentModel.DataAnnotations;
using OvningsbankApi.Models;

namespace OvningsbankApi.Dtos;

/// <summary>
/// Vid uppdatering får även Status ändras, det är så en tränare markerar
/// att ett pass är genomfört.
/// </summary>
public class ExerciseUpdateDto
{
    [Required(ErrorMessage = "Titel måste anges.")]
    [MaxLength(120, ErrorMessage = "Titeln får vara högst 120 tecken.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Beskrivning måste anges.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public ExerciseCategory Category { get; set; }

    [Required]
    public DifficultyLevel Difficulty { get; set; }

    [Required]
    public ExerciseStatus Status { get; set; }
}
