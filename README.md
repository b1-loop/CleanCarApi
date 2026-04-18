# CleanCarApi



\# CleanCarApi







\\# CleanCarApi







Ett ASP.NET Core Web API byggt med Clean Architecture.







\\## Tekniker



\\- ASP.NET Core 8



\\- Clean Architecture (API, Application, Domain, Infrastructure)



\\- CQRS med MediatR



\\- Repository Pattern



\\- Entity Framework Core med SQL Server



\\- JWT-autentisering med rollbaserad access (Admin/User)



\\- AutoMapper



\\- FluentValidation (Pipeline Behaviour)



\\- Swagger UI







\\## Kom igång







\\### Krav



\\- .NET 8 SDK



\\- SQL Server Express (LocalDB)







\\### Starta projektet



1\\. Klona repot



2\\. Uppdatera connection string i `appsettings.json` vid behov



3\\. Kör migrationer:











4\\. Starta projektet — Swagger öppnas på `/swagger`







\\## Struktur







src/



├── CleanCarApi.API          # Controllers, Program.cs



├── CleanCarApi.Application  # Commands, Queries, Handlers, DTOs, Validators



├── CleanCarApi.Domain       # Entities, Interfaces, Enums



└── CleanCarApi.Infrastructure # DbContext, Repositories, Migrations







\\## Endpoints







\\### Auth



| Method | Endpoint | Åtkomst |



|--------|----------|---------|



| POST | /api/auth/register | Öppen |



| POST | /api/auth/login | Öppen |







\\### Cars



| Method | Endpoint | Åtkomst |



|--------|----------|---------|



| GET | /api/cars | Admin, User |



| GET | /api/cars/{id} | Admin, User |



| POST | /api/cars | Admin |



| PUT | /api/cars/{id} | Admin |



| DELETE | /api/cars/{id} | Admin |







\\### Brands



| Method | Endpoint | Åtkomst |



|--------|----------|---------|



| GET | /api/brands | Admin, User |



| GET | /api/brands/{id} | Admin, User |



| POST | /api/brands | Admin |



| PUT | /api/brands/{id} | Admin |



| DELETE | /api/brands/{id} | Admin |







\\## Roller



\\- \\\*\\\*Admin\\\*\\\* — full åtkomst till alla endpoints



\\- \\\*\\\*User\\\*\\\* — kan endast läsa data (GET)







