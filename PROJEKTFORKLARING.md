# CleanCarApi – Fullständig Projektförklaring

## Vad är det här för projekt?

CleanCarApi är ett **REST API** byggt med **ASP.NET Core 8** som hanterar bilar och bilmärken. Det demonstrerar flera moderna arkitekturmönster: **Clean Architecture**, **CQRS** och **MediatR**. API:et har inloggning med JWT-tokens och rollbaserad behörighet (Admin/User).

---

## Mappstruktur – de 4 lagren

```
CleanCarApi/
├── CleanCarApi.API/            ← Presentationslager (HTTP-endpoints)
├── CleanCarApi.Application/    ← Applikationslager (affärslogik, CQRS)
├── CleanCarApi.Domain/         ← Domänlager (entiteter, gränssnitt)
└── CleanCarApi.Infrastructure/ ← Infrastrukturlager (databas, repositories)
```

Varje lager har ett tydligt ansvar och beroenden går bara inåt:
- **API** beror på **Application**
- **Application** beror på **Domain**
- **Infrastructure** beror på **Domain**
- **Domain** beror på ingenting

---

## Lager 1 – Domain (Kärnan)

**Mapp:** `CleanCarApi.Domain/`

Detta är projektets kärna. Ingen extern kod används här.

### Entities/
Domänobjekten – det dessa klasser representerar i verkligheten.

**Car.cs**
```
Id, Make, Model, Year, Price, FuelType, BrandId, Brand
```
- En bil har ett bilmärke (Brand) kopplat via `BrandId`

**Brand.cs**
```
Id, Name, Cars (lista med bilar)
```
- Ett märke kan ha många bilar (1-till-många)

### Enums/
**FuelType.cs** – `Petrol`, `Diesel`, `Electric`, `Hybrid`

### Interfaces/
**IRepository.cs** – Generiskt gränssnitt som definierar vad ett repository måste kunna:
```
GetAllAsync()
GetByIdAsync(id)
AddAsync(entity)
UpdateAsync(entity)
DeleteAsync(id)
```

> **Varför?** Domain-lagret vet att det behöver ett repository, men bryr sig inte om hur det är implementerat. Det är Infrastructure-lagrets jobb.

---

## Lager 2 – Infrastructure (Databas)

**Mapp:** `CleanCarApi.Infrastructure/`

Här implementeras allt som rör databas och datalagring.

### Data/AppDbContext.cs
Ärver från `IdentityDbContext<IdentityUser>` vilket innebär att Identity-tabellerna (användare, roller) automatiskt skapas i databasen.

```csharp
DbSet<Car> Cars
DbSet<Brand> Brands
// + Identitytabeller från IdentityDbContext
```

Konfigurerar relationen: Brand → Cars (en-till-många), och att `Price` sparas som `decimal(18,2)`.

### Repositories/
**Repository.cs** – Generisk implementation av `IRepository<T>`. Fungerar för alla entiteter.

**CarRepository.cs** – Specialiserad version för `Car`. Laddar alltid in `Brand`-objektet via `.Include(c => c.Brand)` (eager loading) så att bilens märke alltid följer med i svaret.

### Migrations/
Autogenererade filer av Entity Framework Core som skapar databasens tabeller.

---

## Lager 3 – Application (CQRS + MediatR)

**Mapp:** `CleanCarApi.Application/`

Här bor affärslogiken. Använder **CQRS** (Command Query Responsibility Segregation) med **MediatR**.

### Vad är CQRS?
Principen är att dela upp operationer i två typer:
- **Commands** – Ändrar data (skapa, uppdatera, radera, logga in, registrera)
- **Queries** – Läser data (hämta alla bilar, hämta en bil)

### Vad är MediatR?
MediatR är ett bibliotek som fungerar som en mellanhand. Istället för att controllern pratar direkt med repositories, skickar den ett meddelande (Command/Query) till MediatR, som sedan hittar rätt Handler och kör den.

```
Controller → MediatR → Handler → Repository → Databas
```

### Cars/Commands/
| Fil | Vad den gör |
|-----|-------------|
| `CreateCarCommand.cs` | Innehåller data för att skapa en bil |
| `UpdateCarCommand.cs` | Innehåller data för att uppdatera en bil |
| `DeleteCarCommand.cs` | Innehåller id för bilen som ska raderas |

### Cars/Queries/
| Fil | Vad den gör |
|-----|-------------|
| `GetAllCarsQuery.cs` | Tom förfrågan – hämta alla bilar |
| `GetCarByIdQuery.cs` | Innehåller id – hämta en specifik bil |

### Cars/Handlers/
Varje command/query har en tillhörande Handler som utför jobbet.

**Exempel – CreateCarHandler:**
1. Tar emot `CreateCarCommand`
2. Mappar DTO till Car-entitet via AutoMapper
3. Sparar via `IRepository<Car>.AddAsync()`
4. Mappar tillbaka Car till `CarDto`
5. Returnerar `CarDto`

### Auth/
Samma mönster för autentisering:
- `LoginCommand` + `LoginHandler` – validerar inloggning, skapar JWT-token
- `RegisterCommand` + `RegisterHandler` – skapar ny användare, tilldelar roll

### Behaviors/ValidationBehavior.cs
En **MediatR Pipeline Behavior** – kod som körs automatiskt INNAN varje handler. Används för att validera inkommande data med FluentValidation. Om validering misslyckas kastas ett undantag innan handlern ens körs.

### DTOs/ (Data Transfer Objects)
DTOs är de objekt som skickas till och från API:et. De exponerar aldrig domänentiteterna direkt.

| DTO | Används för |
|-----|-------------|
| `CarDto` | Returnera bildata till klienten (inkl. märkesnamn som sträng) |
| `CreateCarDto` | Ta emot data för att skapa en bil |
| `UpdateCarDto` | Ta emot data för att uppdatera en bil |
| `LoginDto` | Användarnamn + lösenord för inloggning |
| `RegisterDto` | Användarnamn + lösenord + roll för registrering |

### Mappings/MappingProfile.cs
AutoMapper-konfiguration som definierar hur objekt konverteras:
```
Car       → CarDto      (BrandId konverteras till BrandName-sträng)
CreateCarDto → Car
UpdateCarDto → Car
```

---

## Lager 4 – API (Controllers)

**Mapp:** `CleanCarApi.API/`

### Controllers/
Tar emot HTTP-förfrågningar och skickar dem vidare till MediatR. Controllern gör minimal logik – den delegerar allt till Application-lagret.

**AuthController.cs**
- `POST /api/auth/register` – Registrera ny användare
- `POST /api/auth/login` – Logga in, få JWT-token

**CarsController.cs**
- `GET /api/cars` – Hämta alla bilar (kräver inloggning)
- `GET /api/cars/{id}` – Hämta en bil (kräver inloggning)
- `POST /api/cars` – Skapa bil (kräver Admin-roll)
- `PUT /api/cars/{id}` – Uppdatera bil (kräver Admin-roll)
- `DELETE /api/cars/{id}` – Radera bil (kräver Admin-roll)

**BrandsController.cs**
- `GET /api/brands` – Hämta alla märken (kräver inloggning)
- `GET /api/brands/{id}` – Hämta ett märke (kräver inloggning)
- `POST /api/brands` – Skapa märke (kräver Admin-roll)
- `PUT /api/brands/{id}` – Uppdatera märke (kräver Admin-roll)
- `DELETE /api/brands/{id}` – Radera märke (kräver Admin-roll)

### Program.cs – Hjärtat i konfigurationen
Här registreras alla tjänster i DI-containern (Dependency Injection) och middleware-kedjan konfigureras.

**Vad som registreras:**
```
DbContext         → SQL Server-databas
Identity          → Användarhantering + rollhantering
JWT Auth          → Token-validering
MediatR           → Hittar alla handlers automatiskt
FluentValidation  → Validerar commands/queries
AutoMapper        → Konfigurerar mappningar
Repositories      → CarRepository + generisk Repository<Brand>
Swagger           → API-dokumentation med JWT-stöd
```

### appsettings.json – Konfiguration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=CleanCarDb;..."
  },
  "Jwt": {
    "Key": "SuperSecretKey...",
    "Issuer": "CleanCarApi",
    "Audience": "CleanCarApiUsers"
  }
}
```

---

## Autentisering med JWT

### Hur det fungerar steg för steg:

1. **Registrering** – Klienten skickar `POST /api/auth/register` med `{ username, password, role }`
2. **Inloggning** – Klienten skickar `POST /api/auth/login` med `{ username, password }`
3. **Token skapas** – Om inloggning lyckas returneras en JWT-token (giltig i 2 timmar)
4. **Token används** – Klienten skickar token i headern: `Authorization: Bearer <token>`
5. **Validering** – API:et validerar token automatiskt vid varje skyddat anrop

### Vad JWT-token innehåller (claims):
- Användarnamn
- Användar-ID
- Roller (Admin/User)

### Rollbaserad behörighet:
```
[Authorize(Roles = "Admin")]       → Endast Admin
[Authorize(Roles = "Admin,User")]  → Både Admin och User
```

---

## Komplett dataflöde – Skapa en bil

```
1. Klient skickar: POST /api/cars
   Body: { "make": "Volvo", "model": "XC90", "year": 2024, "price": 599000, "fuelType": 0, "brandId": 1 }
   Header: Authorization: Bearer <jwt-token>

2. CarsController.Create(dto)
   → Skapar CreateCarCommand med dto
   → Skickar via _mediator.Send(command)

3. ValidationBehavior (körs automatiskt)
   → Validerar command-objektet med FluentValidation
   → Om ogiltig data → kasta undantag → 400 Bad Request

4. CreateCarHandler.Handle(command)
   → AutoMapper: CreateCarDto → Car-entitet
   → _repository.AddAsync(car)

5. CarRepository.AddAsync(car)
   → AppDbContext.Cars.AddAsync(car)
   → AppDbContext.SaveChangesAsync()
   → Car sparad i databasen

6. CreateCarHandler (fortsätter)
   → AutoMapper: Car → CarDto
   → Returnerar CarDto

7. CarsController
   → Returnerar 201 Created med CarDto i body
```

---

## NuGet-paket som används

| Paket | Syfte |
|-------|-------|
| `MediatR` | CQRS-mediator, kopplar commands/queries till handlers |
| `AutoMapper` | Konverterar mellan entiteter och DTOs |
| `FluentValidation` | Validerar inkommande data med regler |
| `JwtBearer` | Validerar JWT-tokens i HTTP-headers |
| `Identity.EntityFrameworkCore` | Användarhantering, lösenordshashning, roller |
| `EntityFrameworkCore.SqlServer` | ORM – kommunicerar med SQL Server |
| `Swashbuckle.AspNetCore` | Genererar Swagger UI för att testa API:et |

---

## Varför Clean Architecture?

| Problem utan Clean Architecture | Lösning med Clean Architecture |
|----------------------------------|--------------------------------|
| Allt är kopplat till allt | Tydliga lager med ett riktning på beroenden |
| Svårt att byta databas | Infrastructure kan bytas ut utan att ändra Domain |
| Svårt att testa | Application kan testas utan databas |
| Svårt att förstå koden | Varje klass har ett tydligt ansvar |

---

## Sammanfattning

```
Klient (Swagger/Postman/App)
        ↓  HTTP-anrop
CleanCarApi.API (Controllers)
        ↓  _mediator.Send(command/query)
CleanCarApi.Application (Handlers, CQRS)
        ↓  _repository.GetAllAsync() etc.
CleanCarApi.Domain (IRepository<T>)
        ↓  (implementerad av)
CleanCarApi.Infrastructure (Repository, DbContext)
        ↓  EF Core
SQL Server (CleanCarDb)
```

Flödet är linjärt och ett-riktat. Varje lager vet bara om lagret innanför det, aldrig utanför. Det gör koden lätt att underhålla, testa och bygga vidare på.
