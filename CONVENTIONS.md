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

## Unit tests

#### General Guidelines

- Write **automated tests** for all non-trivial logic.
- Keep tests **small and focused** – one test per behavior.
- Use clear naming with this pattern:  
  **`MethodName_StateUnderTest_ExpectedBehavior`**  
  Example: `Add_ValidNumbers_ReturnsSum`

#### Test Structure: AAA Pattern

Follow the **Arrange – Act – Assert** pattern:

```csharp
// Arrange
var calculator = new Calculator();

// Act
var result = calculator.Add(2, 3);

// Assert
Assert.Equal(5, result);
```

#### Test Naming

Use descriptive names with underscores:

```
MethodName_Scenario_ExpectedOutcome
```

Example:

```csharp
Withdraw_WhenBalanceIsInsufficient_ThrowsInvalidOperationException
```

#### Test Organization

- Group tests by the **class under test**.
- Use one test class per target class:  
  Example: `CalculatorTests`

#### Testing Principles

- Tests must be **deterministic** (same result every time).
- Tests should be **fast** (suitable for every build).
- Avoid dependencies on external systems (file system, databases, network, etc.).

#### Mocking and Isolation

- Use mocking frameworks like **Moq** to isolate dependencies.
- Prefer **constructor injection** for testable code.

#### What Not to Do

- Don’t test multiple behaviors in a single test.
- Don’t rely on shared state between tests.
- Don’t include logic or branching in test methods.

#### Tools and Frameworks

- Use **xUnit** as the primary test framework.
- Use **Moq** or similar for mocking.