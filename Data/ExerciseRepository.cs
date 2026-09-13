using Microsoft.EntityFrameworkCore;
using OvningsbankApi.Models;

namespace OvningsbankApi.Data;

public class ExerciseRepository : IExerciseRepository
{
    private readonly AppDbContext _db;

    public ExerciseRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Exercise>> GetAllAsync() =>
        await _db.Exercises
            .AsNoTracking()          // läsoperation, ingen ändringssparning behövs
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

    public async Task<Exercise?> GetByIdAsync(int id) =>
        await _db.Exercises.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Exercise> AddAsync(Exercise exercise)
    {
        _db.Exercises.Add(exercise);
        await _db.SaveChangesAsync();
        return exercise;
    }

    public async Task<bool> UpdateAsync(Exercise exercise)
    {
        _db.Exercises.Update(exercise);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _db.Exercises.AnyAsync(e => e.Id == id);
}
