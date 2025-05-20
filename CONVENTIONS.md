# Project Conventions

## Naming

#### We will use following prefixes to denote the working stage 

| Type        | Purpose                             | Example                        |
| ----------- | ----------------------------------- | ------------------------------ |
| `feature/`  | New features                        | `feature/user-authentication`  |
| `bugfix/`   | Non-critical bug fixes              | `bugfix/login-button-disabled` |
| `hotfix/`   | Critical production fixes           | `hotfix/crash-on-startup`      |
| `release/`  | Pre-release and versioning branches | `release/v1.1.0`               |
| `chore/`    | Maintenance, infra, deps, cleanup   | `chore/update-nuget-packages`  |
| `test/`     | Experimental or test branches       | `test/performance-benchmark`   |
| `docs/`     | Docs-related changes only           | `docs/add-api-usage-examples`  |
| `refactor/` | Code structure changes              | `refactor/bll-layer-split`     |

#### Branch naming
- `feature/<short-description>` — new features
- `bugfix/<short-description>` — bug fixes
- `hotfix/<short-description>` — critical fixes
- Example: `feature/user-login-form`

#### Commit Messages (Conventional Commits)
- Format: `<type>(scope): message`
- Example: `feat(auth): add JWT token support`


#### Folder Structure
- `UI/` — ASP.NET Core Web API
- `BLL/` — Business logic
- `DAL/` — Data layer (EF Core)
- `*.Tests/` — Unit test projects
