using Microsoft.EntityFrameworkCore;
using OvningsbankApi.Models;

namespace OvningsbankApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Exercise> Exercises => Set<Exercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var exercise = modelBuilder.Entity<Exercise>();

        exercise.Property(e => e.Title).IsRequired().HasMaxLength(120);
        exercise.Property(e => e.Description).IsRequired();

        // Enum lagras som text i databasen istället för som siffra.
        // Det gör databasfilen läsbar vid felsökning och gör att en tillagd
        // kategori inte förskjuter betydelsen av redan sparade rader.
        exercise.Property(e => e.Category).HasConversion<string>().HasMaxLength(40);
        exercise.Property(e => e.Difficulty).HasConversion<string>().HasMaxLength(20);
        exercise.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
    }
}
