using System.ComponentModel.DataAnnotations;
using OvningsbankApi.Models;

namespace OvningsbankApi.Dtos;

/// <summary>
/// Det klienten får skicka in vid skapande.
/// Id, CreatedAt och ImagePath saknas medvetet, de sätts av servern
/// respektive av upload-endpointen och ska inte kunna styras utifrån.
/// </summary>
public class ExerciseCreateDto
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
}
