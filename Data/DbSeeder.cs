using OvningsbankApi.Models;

namespace OvningsbankApi.Data;

/// <summary>
/// Lägger in några övningar första gången databasen skapas.
/// Syftet är att den som klonar repot ser en fylld lista direkt istället för
/// ett tomt gränssnitt - både vid utveckling och vid rattning.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Exercises.Any()) return;

        db.Exercises.AddRange(
            new Exercise
            {
                Title = "Rondo 5 mot 2",
                Description = "Fem spelare i en ring med cirka åtta meters diameter, två i mitten. "
                            + "Ytterspelarna får högst två touch. Byte när en mittenspelare vinner bollen "
                            + "eller när en passning bryts. Kör 4 x 3 minuter med 45 sekunders vila.",
                Category = ExerciseCategory.Passningsspel,
                Difficulty = DifficultyLevel.Medel,
                Status = ExerciseStatus.Planerad
            },
            new Exercise
            {
                Title = "Ledrörlighet och löpskola",
                Description = "Tio minuter i lugnt tempo: höjda knän, hälsparkar, sidogalopp och "
                            + "hallhopp över en linje. Avslutas med tre stegringslopp på 30 meter.",
                Category = ExerciseCategory.Uppvarmning,
                Difficulty = DifficultyLevel.Latt,
                Status = ExerciseStatus.Genomford
            },
            new Exercise
            {
                Title = "Avslut efter inspel från kant",
                Description = "Två köer vid mittlinjen, en kantspelare på var sida. Kantspelaren slår "
                            + "inspel i första eller andra stolpen. Angriparen ska ta sig före sin markering "
                            + "innan bollen slås. 12 avslut per sida, byt sedan kant.",
                Category = ExerciseCategory.Avslut,
                Difficulty = DifficultyLevel.Medel,
                Status = ExerciseStatus.Planerad
            },
            new Exercise
            {
                Title = "Presspel i hög zon",
                Description = "8 mot 8 på halv plan med tre småmål. Det försvarande laget får bara "
                            + "poäng genom att vinna bollen inom sex sekunder efter förlorad boll. "
                            + "Tränaren fryser spelet vid felaktig pressvinkel och ställningsspel.",
                Category = ExerciseCategory.Taktik,
                Difficulty = DifficultyLevel.Svar,
                Status = ExerciseStatus.Planerad
            }
        );

        db.SaveChanges();
    }
}
