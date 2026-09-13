using Microsoft.AspNetCore.Mvc;
using OvningsbankApi.Dtos;
using OvningsbankApi.Services;

namespace OvningsbankApi.Controllers;

/// <summary>
/// Controllern hanterar bara HTTP: bindning, statuskoder och validering.
/// All logik ligger i IExerciseService, all fillagring i IFileStorageService.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _service;
    private readonly IFileStorageService _fileStorage;

    public ExercisesController(IExerciseService service, IFileStorageService fileStorage)
    {
        _service = service;
        _fileStorage = fileStorage;
    }

    /// <summary>Hämtar alla övningar, nyast först.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExerciseReadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExerciseReadDto>>> GetAll()
    {
        var exercises = await _service.GetAllAsync();
        return Ok(exercises);
    }

    /// <summary>Hämtar en enskild övning.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ExerciseReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseReadDto>> GetById(int id)
    {
        var exercise = await _service.GetByIdAsync(id);
        if (exercise is null)
            return NotFound(new { message = $"Ingen övning med id {id} hittades." });

        return Ok(exercise);
    }

    /// <summary>Skapar en ny övning.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ExerciseReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExerciseReadDto>> Create([FromBody] ExerciseCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);

        // 201 med Location-header pekar klienten på den nya resursen.
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Uppdaterar en befintlig övning, inklusive dess status.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ExerciseReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseReadDto>> Update(int id, [FromBody] ExerciseUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound(new { message = $"Ingen övning med id {id} hittades." });

        return Ok(updated);
    }

    /// <summary>
    /// Laddar upp en bild till en övning. Multipart/form-data med fältnamnet "file".
    /// Returnerar hela den uppdaterade övningen så att klienten kan uppdatera sin
    /// vy utan ett extra GET-anrop.
    /// </summary>
    [HttpPost("{id:int}/upload")]
    [ProducesResponseType(typeof(ExerciseReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseReadDto>> Upload(int id, IFormFile file)
    {
        if (file is null)
            return BadRequest(new { message = "Ingen fil bifogades." });

        var exercise = await _service.GetByIdAsync(id);
        if (exercise is null)
            return NotFound(new { message = $"Ingen övning med id {id} hittades." });

        var result = await _fileStorage.SaveImageAsync(file);
        if (!result.Success)
            return BadRequest(new { message = result.Error });

        var updated = await _service.SetImageAsync(id, result.RelativePath!);
        return Ok(updated);
    }
}
