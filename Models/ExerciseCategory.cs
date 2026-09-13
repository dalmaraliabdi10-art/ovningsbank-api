namespace OvningsbankApi.Models;

/// <summary>
/// övningens typ. Enum istället för fri text så att både API och klienter
/// har samma uppsättning kategorier - det gör filtrering i frontend trivial
/// och omöjliggör stavfel i databasen.
/// Namnen är ASCII för att hålla JSON och URLer fria från teckenkodning;
/// klienterna översätter till svenska etiketter i sitt gränssnitt.
/// </summary>
public enum ExerciseCategory
{
    Uppvarmning,
    Passningsspel,
    Avslut,
    Taktik
}
