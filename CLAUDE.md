# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

Demo source code for the Pluralsight course **"Architecting an ASP.NET Core MVC Application for Unit Testability"** by Benjamin Day. It is not a single application — it is a set of independent snapshots that go alongside the course modules. Each module folder contains a `before/` and `after/` pair so a viewer can compare the state of the code at the start vs. end of that module's demos.

Course URL: https://www.pluralsight.com/courses/architecting-aspnet-core-mvc-unit-testability

Target framework throughout: **.NET 7** (per the merged "Upgraded to .Net 7" PR). Test framework: **MSTest**.

## Repository layout

```
demos-m2-what-should-i-test/                  Pluralsight.UnitTestDemo  (intro/scaffolding)
demos-m3-abstraction-to-test-ui/              Benday.WebCalculator      (abstracting UI for testability)
demos-m4-overthrowing-tyranny-of-the-database/ Benday.Presidents        (decoupling from EF/SQL)
demos-m5-leveraging-the-strategy-pattern/      Benday.Presidents
demos-m6-invoking-the-right-logic/             Benday.Presidents
demos-m7-testing-security-authorization/       Benday.Presidents
demos-m8-testing-security-middleware/          Benday.Presidents
demos-m9-validating-webapi/                    Benday.Presidents        (most complete state)
```

Each module folder has `before/` and `after/` subfolders. They are **independent solutions** — no shared code, no cross-module references. The `Benday.Presidents` solution evolves from m4 through m9; treat the m9 `after/` snapshot as the most feature-complete reference.

Module 2's `before/` is just a `create-aspnetcore-with-efcore7.bat` scaffolding script (it generates the m2 `after/` solution layout). Module 3 onward has full solutions on both sides.

## Build / run / test

Always work inside the specific snapshot you are looking at — there is no root solution. Either `cd` into the snapshot folder or pass the `.sln` path explicitly:

```powershell
# Build a specific snapshot
dotnet build demos-m9-validating-webapi\after\Benday.Presidents\Benday.Presidents.sln

# Run all tests in a snapshot
dotnet test demos-m9-validating-webapi\after\Benday.Presidents\Benday.Presidents.sln

# Run a single test by fully-qualified name
dotnet test demos-m9-validating-webapi\after\Benday.Presidents\Benday.Presidents.sln `
  --filter "FullyQualifiedName~PresidentServiceFixture.SaveNewPresident"

# Run the web app (where applicable, e.g. m9)
dotnet run --project demos-m9-validating-webapi\after\Benday.Presidents\src\Benday.Presidents.WebUi
```

Module 6's solution is unusual — `Benday.Presidents.sln` sits at the module root (`demos-m6-invoking-the-right-logic/after/Benday.Presidents.sln`), not inside a `Benday.Presidents/` subfolder like the other modules.

### Database expectations (m4+)

The `Benday.Presidents` app uses **SQL Server + EF Core** with a `default` connection string in `appsettings.json` pointing at `Server=(local); Database=...; Trusted_Connection=True; Encrypt=False;`. On startup, `Program.cs` calls `context.Database.Migrate()` for both `PresidentsDbContext` and the Identity `ApplicationDbContext` — so a reachable SQL Server instance is required to run the WebUi project. Unit tests don't need a database; integration tests do.

## Architecture (Benday.Presidents — the through-line for m4–m9)

The whole point of the course is wiring this app for testability, so the project structure reflects that intent. By m9 the solution looks like:

**src/**
- `Benday.DataAccess` — generic, app-agnostic repository abstractions (`IRepository<T>`, `IInt32Identity`) and an EF Core base implementation under `SqlServer/`. Reused across the demos to keep persistence pluggable.
- `Benday.Presidents.Api` — **the business/domain layer**. Holds domain entities (`Person`, `PersonFact`, `Relationship`, `Subscription`, …), `PresidentsDbContext`, domain services (`PresidentService`, `SubscriptionService`, `FeatureManager`, `Logger`), and the interfaces that make them mockable (`IPresidentService`, `IPresidentsDbContext`, `IUsernameProvider`, `IFeatureManager`, `ILogger`). The `Models/` folder also contains strategy interfaces (e.g. `IValidatorStrategy<T>`, `IDaysInOfficeStrategy`) introduced in m5.
- `Benday.Presidents.WebUi` — ASP.NET Core MVC front end. Controllers, Razor views, ASP.NET Identity, and the `Security/` folder containing the authorization handlers/requirements, the `IUserAuthorizationStrategy` abstraction (m7), and `PopulateSubscriptionClaimsMiddleware` (m8). `Program.cs` is where DI registration lives — `RegisterTypes()` is the canonical place to add new bindings.

**test/**
- `Benday.Presidents.UnitTests` — pure unit tests, no DB. References both `Api` and `WebUi`.
- `Benday.Presidents.MvcIntegrationTests` (m9) — uses `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory`) to spin up the real pipeline; `Benday.Presidents.WebUi` exposes `InternalsVisibleTo` for this project.
- `Benday.Presidents.IntegrationTests` (earlier modules) — DB-backed integration tests.
- `Benday.Presidents.Tests.Common` — shared test helpers/fixtures.

### Patterns the demos are deliberately teaching

When editing inside a Presidents snapshot, expect (and preserve) these patterns:

- **Everything injectable**: controllers, middleware, services, even username/claims access (`IUsernameProvider`, `IUserClaimsPrincipalProvider`) go through interfaces so unit tests don't need `HttpContext`.
- **Repository + DbContext interface**: persistence is reached via `IRepository<T>` and `IPresidentsDbContext`, never `DbContext` directly from business code.
- **Strategy pattern (m5+)**: domain rules (validation, days-in-office calculations, user authorization) are pulled into `IValidatorStrategy<T>` / `IDaysInOfficeStrategy` / `IUserAuthorizationStrategy` so each rule is independently testable and swappable in DI.
- **Authorization as handlers/requirements (m7)**: custom policies (e.g. `PolicyName_EditPresident`) are implemented with `AuthorizationHandler` + `IAuthorizationRequirement` and registered in `Program.cs`.
- **Middleware via `IMiddleware`**: e.g. `PopulateSubscriptionClaimsMiddleware` is registered as a transient service and applied via an extension method (`MiddlewareExtensionMethods`), which is what makes it unit-testable.

When adding new functionality, mirror these patterns rather than introducing direct `DbContext`/`HttpContext`/`new`-up coupling — that's the whole pedagogical point of the codebase.

### WebCalculator (m3) — different shape

`Benday.WebCalculator` is a smaller, separate demo about extracting calculator logic from a Razor Page so the page becomes a thin shell over a testable service. Don't confuse it with the Presidents architecture.

## Conventions

- C# style is **4-space indent, tabs disabled, `Nullable` enabled, `ImplicitUsings` enabled** in every csproj.
- Test projects use **MSTest** (`[TestClass]` / `[TestMethod]`), not xUnit or NUnit.
- Test class names follow `<TypeUnderTest>Fixture` (e.g. `PresidentServiceFixture`).
- DI registrations live in `Program.cs` → `RegisterTypes()`; keep new bindings there rather than scattering them.
