# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

A Visual Studio "React and ASP.NET Core" SPA template project, extended with a layered backend
(controller → service → repository) backed by SQL Server via Dapper. It currently exposes a single
`/weatherforecast` endpoint consumed by a React app.

- `ReactApp.Server/` — ASP.NET Core Web API (.NET 10), serves the built SPA and hosts the API.
- `reactapp.client/` — React 19 + Vite SPA (JavaScript, not TypeScript).
- `ReactApp.slnx` — solution file referencing both projects.

In development, the server project uses `Microsoft.AspNetCore.SpaProxy` to launch and proxy to the
Vite dev server automatically (see `SpaProxyLaunchCommand`/`SpaProxyServerUrl` in
[ReactApp.Server.csproj](ReactApp.Server/ReactApp.Server.csproj)). There is no `.env`-based config;
the dev HTTPS port is passed via `ASPNETCORE_HTTPS_PORT`/`ASPNETCORE_URLS` and read by
[vite.config.js](reactapp.client/vite.config.js) to configure the proxy target.

## Commands

Run these from the repository root unless noted.

### Backend (ReactApp.Server)
- Build: `dotnet build ReactApp.slnx`
- Run (serves API + proxies to Vite dev server): `dotnet run --project ReactApp.Server`
  - HTTPS profile listens on `https://localhost:7009` (see
    [launchSettings.json](ReactApp.Server/Properties/launchSettings.json))
- No backend test project currently exists.

### Frontend (reactapp.client)
Run from `reactapp.client/`:
- Install deps: `npm install`
- Dev server: `npm run dev` (normally launched automatically by the SpaProxy when running the server project)
- Build: `npm run build`
- Lint: `npm run lint`
- Preview production build: `npm run preview`
- No frontend test suite currently exists.

## Architecture notes

- **Layering**: `Controllers` → `Interfaces` (service/repository abstractions) → `Services`
  (business logic) → `Databases` (data access). Follow this pattern for new features — add an
  interface in `Interfaces/`, register the implementation in `Program.cs` via
  `builder.Services.AddTransient<...>()`.
- **Data access**: `WeatherForecastRepository` uses Dapper (`Microsoft.Data.SqlClient` +
  `SqlConnection.QueryAsync<T>`) with raw SQL rather than EF Core. There is currently no
  `DbContext`; follow the existing raw-SQL-with-Dapper convention for new repositories.
- **Connection string**: currently hardcoded in
  [WeatherForecastRepository.cs](ReactApp.Server/Databases/WeatherForecastRepository.cs) pointing
  at a local `(localdb)\datapoint` instance rather than read from `appsettings.json`.
- **SPA integration**: `Program.cs` calls `UseDefaultFiles()`, `MapStaticAssets()`, and
  `MapFallbackToFile("/index.html")` so the built client is served by the ASP.NET Core app in
  production; in development, requests fall through to the Vite dev server via the SPA proxy.
- **API docs**: Swagger/OpenAPI is enabled only in Development (`MapOpenApi`, `UseSwagger`,
  `UseSwaggerUI`).
- **Client-side API calls**: the client calls relative paths (e.g. `fetch('weatherforecast')`);
  the Vite dev proxy config in `vite.config.js` maps `^/weatherforecast` to the backend, so add new
  API route prefixes there when adding endpoints.
