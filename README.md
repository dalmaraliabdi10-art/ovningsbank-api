# Övningsbank för fotbollstränaren — API

REST-API byggt med ASP.NET Core WebAPI, lagrar fotbollsövningar, tillhörande
bilder, betjänar både webbapplikationen och mobilapplikationen i projektet.

## Kom igång

Krävs: .NET SDK 8.0 eller senare.

```bash
cd ovningsbank-api
dotnet restore
dotnet run
```

API:et startar på **http://localhost:5080**. Swagger finns på
http://localhost:5080/swagger och öppnas automatiskt.

Databasen (`ovningsbank.db`) skapas i projektmappen vid första uppstarten och
fylls med fyra exempelövningar. Ingen databasinstallation behövs.

## Endpoints

| Metod | Väg | Beskrivning |
|---|---|---|
| GET | `/api/exercises` | Alla övningar, nyast först |
| GET | `/api/exercises/{id}` | En enskild övning |
| POST | `/api/exercises` | Skapar en övning |
| PUT | `/api/exercises/{id}` | Uppdaterar en övning, inklusive status |
| POST | `/api/exercises/{id}/upload` | Laddar upp en bild (multipart, fältnamn `file`) |

Uppladdade bilder serveras statiskt på `/uploads/<filnamn>`.

### Datamodell

```json
{
  "id": 1,
  "title": "Rondo 5 mot 2",
  "description": "Fem spelare i en ring ...",
  "category": "Passningsspel",
  "difficulty": "Medel",
  "status": "Planerad",
  "imagePath": "/uploads/3f2a9c....png",
  "createdAt": "2026-09-09T09:00:00Z"
}
```

- `category`: `Uppvarmning` | `Passningsspel` | `Avslut` | `Taktik`
- `difficulty`: `Latt` | `Medel` | `Svar`
- `status`: `Planerad` | `Genomford`

## Tekniska val

**SQLite via EF Core.** Uppgiften bedöms genom att repot klonas och körs lokalt.
SQLite kräver ingen serverinstallation och databasfilen skapas automatiskt vid uppstart. 

**`EnsureCreated()` istället för migrationer.** `EnsureCreated()` gör att
`dotnet run` räcker, inget `dotnet-ef`-verktyg och ingen migrationskörning
innan API:et startar. 

**Controller → service → repository.** Controllern hanterar bara HTTP:
modellbindning, statuskoder och validering. Logiken ligger i `ExerciseService`
och datalagringen bakom `IExerciseRepository`. Servicelagret vet inte att det är
EF Core bakom interfacet, så lagringen kan bytas ut utan att logiken rörs.

**Separata DTOer för create, update och read.** Klienten ska inte kunna sätta
`Id`, `CreatedAt` eller `ImagePath` direkt. `ExerciseCreateDto` saknar dem
medvetet; `ExerciseUpdateDto` tillåter dessutom `Status`, eftersom det är så en
tränare markerar att ett pass är genomfört. Entiteten `Exercise` lämnar aldrig
servicelagret.

**Svårighetsgrad och status som två fält.** Uppgiftsbeskrivningen tillåter
antingen svårighetsgrad eller status. Här finns båda, eftersom de ändras vid
olika tillfällen: svårighetsgraden är en egenskap hos övningen och ändras
sällan, medan statusen ändras varje gång ett pass körts. Det ger också
PUT-endpointen ett realistiskt användningsfall.

**Enum lagrad som text.** `HasConversion<string>()` gör databasfilen läsbar vid
felsökning och en ny kategori kan läggas till utan att betydelsen av redan
sparade rader förskjuts. I JSON serialiseras enum som text av samma skäl,
klienterna slipper hålla en kopia av enum-ordningen aktuell.

**Slumpade filnamn vid uppladdning.** Filen sparas som en GUID plus
originaländelsen. Två uppladdningar av `taktik.png` skriver därmed inte över
varandra och ett filnamn från klienten kan inte användas för att skriva utanför
uppladdningsmappen. Filtyp och storlek valideras mot `appsettings.json`.

**Sökväg i databasen, inte filens bytes.** Bilden lagras på disk och databasen
håller bara den relativa webbsökvägen. Statiska filer serveras snabbare av
webbservern än via en databasläsning, och databasfilen hålls liten.

**CORS-origins från konfiguration.** Tillåtna adresser läses från
`appsettings.json` istället för att hårdkodas. När mobilappen tillkommer läggs
datorns IP i det lokala nätet till där, utan att någon kodrad ändras.

## Projektstruktur

```
ovningsbank-api/
├─ Controllers/      HTTP-lager, endast bindning och statuskoder
├─ Services/         Applikationslogik och fillagring
├─ Data/             DbContext, repository och seed-data
├─ Models/           Entiteter och enums
├─ Dtos/             API-kontraktet mot klienterna
├─ wwwroot/uploads/  Uppladdade bilder (versionshanteras inte)
└─ Program.cs        Tjänsteregistrering, CORS och pipeline
```
