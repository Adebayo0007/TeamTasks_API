# Team Task Management API (C# / .NET 8 / SQL Server)

A secure, team-scoped task management REST API that demonstrates clean layering, JWT auth, EF Core with SQL Server, and good DevEx (Swagger, logging, tests).

## Features (meets brief)

- Users register/login → JWT
- Users can create teams
- Team Admins can add users to their team
- Only **team members** can list/create tasks in that team
- Any **team member** can update/delete/patch status of tasks in their team
- SQL Server + EF Core (code-first) with normalized relationships
- Logging (Serilog), Swagger/OpenAPI, global exception handling
- Clean architecture: **Controllers → Services → Repositories → EF Core**

## Tech stack

- .NET 8 Web API
- EF Core 8 + SQL Server
- JWT (custom) + BCrypt password hashing
- Serilog, Swagger, AutoMapper
- xUnit (+ Moq, FluentAssertions) for tests

## Getting started

1. **Clone & restore**
   ```bash
   dotnet restore
   ```

2. **Configure database**
   - Edit `TeamTasksAPI/TeamTask_API/appsettings.json` → `ConnectionStrings:DefaultConnection` for your SQL Server.
   - Replace `Jwt:Secret` with a long random value (>= 32 chars).

3. **Migrations**
   Generate and apply migrations (recommended):
   ```bash
   cd /TeamTask_API
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add Initial
   dotnet ef database update
   ```

   Alternatively, run the included SQL script (barebones schema) if you prefer:
   - `TeamTasksAPI/TeamTask_API/Scripts/initial.sql`

4. **Run**
   ```bash
   dotnet run --project TeamTasksAPI/TeamTask_API/TeamTask_API.csproj
   ```

5. **Swagger**
   Visit `/swagger` to explore and test the API.

## API Endpoints

### Auth
- `POST /auth/register` → `{ email, fullName, password }` → `{ token, expiresAt }`
- `POST /auth/login` → `{ email, password }` → `{ token, expiresAt }`

### Users
- `GET /users/me` (Bearer) → current user profile

### Teams
- `POST /teams` (Bearer) → body: `"Team Name"` → `{ id, name }`
- `POST /teams/{teamId}/users` (Bearer) → body: `"{userIdToAdd}"` → `204`  
  Requires requester to be **Team Admin**.

### Tasks
- `GET /teams/{teamId}/tasks` (Bearer, TeamMember policy) → list tasks
- `POST /teams/{teamId}/tasks` (Bearer, TeamMember policy) → create task
- `PUT /tasks/{taskId}` (Bearer) → update task (team membership enforced)
- `DELETE /tasks/{taskId}` (Bearer) → delete task (team membership enforced)
- `PATCH /tasks/{taskId}/status` (Bearer) → set status: `Pending|InProgress|Completed`

## Assumptions & Notes

- Simple custom auth: we don't use ASP.NET Identity for brevity.
- Anyone in a team can manage tasks for that team (per brief).
- Team roles: `Admin` and `Member` (Admins can add members).
- Passwords are hashed with BCrypt; tokens expire in `Jwt:ExpiryMinutes`.
- Minimal DTOs to keep the surface tight.
- Global exception middleware maps common exceptions to JSON errors:
  - 400 (InvalidOperationException), 403 (UnauthorizedAccessException), 404 (KeyNotFoundException), 500 fallback.

## Testing

```bash
dotnet test
```

The test project shows examples for `AuthService` happy-path and `TaskService` membership enforcement.

## Project Structure

```
TeamTask_API.sln
TeamTasksAPI/TeamTask_API
  Controllers/          # API surface
  Data/                 # DbContext
  Domain/               # Entities, Enums, Repo/Service contracts
  Infrastructure/       # Repos, Services, Security, Policies
  DTOs/                 # Request/Response models
  Middleware/           # Global exception handler
  Mappings/             # AutoMapper profiles
  Scripts/initial.sql   # Optional DB bootstrap
tests/TeamTaskApi.Tests # xUnit tests
```

## Security Checklist

- [x] Validate JWT and issuer/audience
- [x] No tokens in logs, minimal error messages on auth
- [x] Per-team authorization via policy + service checks
- [x] Hash passwords; never return password fields

## Next Steps (Stretch)
- Pagination & filtering for tasks
- Soft-delete, auditing
- Email invites/acceptance flow
- Refresh tokens, password reset
```

