# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout

This is a monorepo with the actual source nested under `template/`:

- `template/PawPal.Backend/` — .NET 8 Clean Architecture API (solution: `PawPalApp.Backend.sln`)
- `template/PawPal.Frontend/PawPal_fe/` — Angular 21 SPA
- `docker-compose.yml` (repo root) — Mailpit SMTP catcher, only needed for testing email flows

All commands below assume you've `cd`ed into the relevant project directory.

## Backend (`template/PawPal.Backend`)

### Commands

```bash
dotnet restore                                  # restore once after clone
cd Market.API && dotnet run --launch-profile https   # run the API (https://localhost:7260)
dotnet build                                    # build whole solution (from template/PawPal.Backend)
dotnet test                                     # run all tests (from template/PawPal.Backend)
dotnet test --filter "FullyQualifiedName~ProductCategoryUnitTests"   # run a single test class
dotnet test --filter "DisplayName~SomeTestName"                       # run a single test by name
```

No migration commands are needed — the database is created/seeded automatically on startup in Development (see `DatabaseInitializer.cs`, `Database/Seeders/`).

### Layers and dependency direction

Standard Clean Architecture, referenced strictly inward:

- **Market.Domain** — entities only (`Entities/Adoptions`, `Animal_Info`, `Catalog`, `Identity`, `Messaging`, `Moderation`, `News`, `Places`, `Posts`, `Security`), no dependencies on other layers.
- **Market.Shared** — cross-cutting DTOs, constants, and `Options` classes bound from config; referenced by every other layer.
- **Market.Application** — business logic as MediatR commands/queries, organized as vertical slices under `Modules/<Feature>/{Commands,Queries}` (e.g. `Modules/Posts/Commands/Create/CreatePostCommand.cs` + `CreatePostCommandHandler.cs`). `Abstractions/` holds the interfaces (`IAppDbContext`, `IAppCurrentUser`, `IJwtTokenService`, `IEmailService`, hub service interfaces, etc.) that Infrastructure/API implement. FluentValidation validators live beside their command/query and run through `Common/Behaviors/ValidationBehavior.cs`, a MediatR pipeline behavior registered in `Application/DependencyInjection.cs`.
- **Market.Infrastructure** — EF Core (`Database/DatabaseContext.cs`, `Database/Configurations/*` fluent config, `Database/Migrations`), seeders, SignalR hubs (`Signal/`), and concrete implementations of Application abstractions.
- **Market.API** — ASP.NET controllers under `Controllers/<Feature>/`, one thin controller per feature that just dispatches to MediatR (`IMediator.Send(...)`), plus `Middleware/` (e.g. `InputSanitizationMiddleware`) and `DependencyInjection.cs` for API-layer service registration. `Program.cs` wires everything together (Serilog, JWT auth w/ SignalR query-string token support, CORS, rate limiting, Firebase push init, static file serving for uploaded images under `/StaticImages`).
- **Market.Tests** — mix of unit tests (`UnitTests/<Feature>`) and integration tests (`ProductCategoryTests/IntegrationTests`) using `CustomWebApplicationFactory`.

When adding a new feature, follow the existing vertical-slice pattern: add the command/query + handler (+ validator) under `Market.Application/Modules/<Feature>/`, expose it via a controller action under `Market.API/Controllers/<Feature>/`, and add any new entity/config under `Market.Domain/Entities/<Feature>` and `Market.Infrastructure/Database/Configurations/<Feature>`.

### Local dependencies for a working backend

- SQL Server reachable as `localhost` (Windows auth) or LocalDB — connection string in `Market.API/appsettings.json`.
- Outbound internet access — login verifies reCAPTCHA against Google server-side.
- SMTP catcher on `localhost:1025` (`docker compose up -d` at repo root) — only required for new-account registration; seeded test accounts don't need it.
- `firebase-service-account.json` in `Market.API/` — optional; its absence just disables push notifications (logged as a warning, not fatal).

Seeded test accounts: `admin@market.local` / `Admin123!` (Admin) and `johnnydoe1@gmail.com` / `johnnydoe1` (verified user).

## Frontend (`template/PawPal.Frontend/PawPal_fe`)

### Commands

```bash
npm install
npm start                 # ng serve, http://localhost:4200
npm run build              # ng build
npm run watch               # ng build --watch --configuration development
npm test                    # ng test (Karma/Jasmine)
npx ng test --include='**/some.component.spec.ts'   # run a single spec file
```

Expects the backend at `https://localhost:7260` (see `src/environments/environment.ts`) and requires trusting the backend's self-signed dev cert (`dotnet dev-certs https --trust` on the backend machine, or accept the browser warning once).

### Structure

- `app/modules/` — one NgModule per app area, each lazy-loaded from `app-routing-module.ts`: `public` (unauthenticated browsing — catalog, news, post, profile), `auth` (login/register/email confirmation), `client` (authenticated end-user area — posts, adoption, messaging, favorites, settings), `admin` (role-gated: `data: { requireRoleId: 3 }` in routing, moderation/user-management views). `modules/shared/` holds cross-module components/utils.
- `app/core/` — app-wide singletons: `guards/` (`myAuthGuard`, `myExpireGuard` — auth/role/token-expiry route guards), `interceptors/` (auth token attach, error logging, loading bar, rate-limit handling), `services/` (auth, SignalR, notifications, theming, toaster), `preload/` (custom route preloading strategy).
- `app/api-services/` — one folder per backend feature/resource (mirrors `Market.API/Controllers/<Feature>`), e.g. `animal-posts`, `adoption`-related folders, `messaging`, `moderation`, `users`. Add new API clients here following the existing per-resource folder convention.
- Real-time features (comments, messaging) connect to the backend SignalR hubs (`/commentHub`, `/messageHub`) via `core/services/signalr.service.ts`; the JWT is passed as an `access_token` query param since SignalR can't set an Authorization header on the initial handshake.

## Cross-cutting notes

- CORS is currently locked to `http://localhost:4200` / `https://localhost:4200` plus a configurable `allowedOrigins` value — keep this in mind when changing ports or adding new frontend origins.
- There is no CI configured (`.github/workflows/` is empty) — verification is manual (`dotnet build` / `dotnet test` / `ng test` / `ng build`).
