# PawPal

A pet adoption / marketplace app with an Angular frontend and a .NET (Clean Architecture) backend.

## Prerequisites

| Tool | Version | Notes |
|---|---|---|
| .NET SDK | 8.0+ | backend targets `net8.0` |
| Node.js | 20.19+ or 22.12+ | required by Angular 21 |
| Angular CLI | 21.x | `npm install -g @angular/cli` (optional — `npx ng` also works) |
| SQL Server | any local instance, **or** LocalDB | see [Database](#database) below |
| Docker | any recent version | optional — only needed to run the Mailpit SMTP catcher for testing registration |

## 1. Clone the repo

```bash
git clone https://github.com/wiggyless/PawPal.git
cd PawPal
```

Note: the actual source lives under the `template/` folder in this repo (`template/PawPal.Backend`, `template/PawPal.Frontend`) — all paths below are relative to the repo root you just cloned into.

## 2. Backend setup (`template/PawPal.Backend`)

```bash
cd template/PawPal.Backend
dotnet restore
```

### Database

The connection string lives in `Market.API/appsettings.json`:

```
Server=localhost;Database=PawPal_db;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```
This assumes a local SQL Server instance reachable as `localhost` using Windows Authentication. 

**You do not need to run any migration commands manually.** 

### Firebase (push notifications) — optional

`firebase-service-account.json` is a private key and is **not** in the repo (it's gitignored). If you have it, drop it into `template/PawPal.Backend/Market.API/` (same folder as `Program.cs`). If it's missing, the API still starts fine — it logs a warning and push notifications are simply skipped.

### Email (SMTP) — needed for registration

New-account registration sends a confirmation email via SMTP, configured in `Market.API/appsettings.Development.json` to point at `localhost:1025`. This is meant to be caught by a local dev SMTP tool rather than a real mail server. If nothing is listening on that port, **registering a new account will fail** (login with the pre-seeded accounts below still works fine, since those don't need to send email).

To test registration, run a local SMTP catcher before starting the API. A ready-to-use [`docker-compose.yml`](docker-compose.yml) is included at the repo root (Mailpit):

```bash
docker compose up -d
```

Caught emails can be viewed at `http://localhost:8025`. (Any similar tool — Mailpit, Papercut, smtp4dev — works the same way.)

### reCAPTCHA — needs internet access

Login calls Google's reCAPTCHA verification endpoint server-side. The dev site/secret keys already in `appsettings.json` are configured for `localhost`, but **the machine running the backend needs outbound internet access** for login to succeed.

### Run the backend

```bash
cd Market.API
dotnet run --launch-profile https
```

## 3. Frontend setup (`template/PawPal.Frontend/PawPal_fe`)

```bash
cd template/PawPal.Frontend/PawPal_fe
npm install
ng serve
```

Then open `http://localhost:4200`. The frontend expects the backend at `https://localhost:7260` (see `src/environments/environment.ts`) — the first time you hit it, your browser will likely warn about the backend's self-signed dev HTTPS certificate; you'll need to accept/trust it (or run `dotnet dev-certs https --trust` once on the backend machine).

## Test accounts

Seeded automatically on first run in Development:

| Email | Password | Role |
|---|---|---|
| `admin@market.local` | `Admin123!` | Admin |
| `johnnydoe1@gmail.com` | `johnnydoe1` | Verified user |

## Ports / services summary

| Service | URL | Required? |
|---|---|---|
| Frontend (Angular) | `http://localhost:4200` | yes |
| Backend API + Swagger | `https://localhost:7260` | yes |
| SQL Server | `localhost` (default instance) or LocalDB | yes |
| SMTP catcher (Mailpit etc.) | `localhost:1025` | only for testing registration/email flows |
| Firebase | — | optional, push notifications only |
| Google reCAPTCHA | internet access from backend | yes, for login |

## Known limitations for local/test use

- Registering a new account requires a local SMTP catcher (see above); logging in with the seeded accounts does not.
- Login requires the backend to reach Google's reCAPTCHA API over the internet.
- Firebase push notifications are optional and silently disabled if the credentials file isn't present.
