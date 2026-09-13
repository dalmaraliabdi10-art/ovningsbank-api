using OvningsbankApi.Models;

namespace OvningsbankApi.Data;

/// <summary>
/// Datalagringen bakom ett interface. Servicelagret känner bara till detta
/// kontrakt, inte att det råkar vara EF Core och SQLite bakom, byts lagringen
/// ut behöver ingen annan fil ändras.
/// </summary>
public interface IExerciseRepository
{
    Task<IReadOnlyList<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(int id);
    Task<Exercise> AddAsync(Exercise exercise);
    Task<bool> UpdateAsync(Exercise exercise);
    Task<bool> ExistsAsync(int id);
}
