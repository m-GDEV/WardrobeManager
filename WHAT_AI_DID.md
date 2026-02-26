# What AI Did

## Overview

Added unit test coverage for all three non-test projects in the solution, following the style established in `LoggingServiceTests.cs`. Tests use **NUnit**, **Moq**, and **FluentAssertions**.

---

## Files Created / Modified

### WardrobeManager.Api.Tests

#### New Files

| File | Description |
|---|---|
| `Helpers/AsyncQueryHelper.cs` | Helper that wraps `IEnumerable<T>` as both `IQueryable<T>` and `IAsyncEnumerable<T>`, allowing EF Core's `ToListAsync()` to work against in-memory data during tests of `UserService`. |
| `Database/Entities/ClothingItemTests.cs` | Tests for the `Wear()` and `Wash()` business methods on the `ClothingItem` entity, and for correct default property values. |
| `Services/UserServiceTests.cs` | Tests for `UserService`: `GetUser`, `DoesUserExist`, `UpdateUser`, `DeleteUser`, `DoesAdminUserExist`, and `CreateAdminIfMissing`. Uses Moq to mock `UserManager<User>` and `RoleManager<IdentityRole>`. |
| `Services/FileServiceTests.cs` | Tests for `FileService`: `ParseGuid` (string conversion and brace removal), `SaveImage` (null/empty guard, file write, oversized file exception), and `GetImage` (file present vs. missing fallback). Uses a temporary directory and cleans up in `[TearDown]`. |
| `Services/DataDirectoryServiceTests.cs` | Tests for `DataDirectoryService`: constructor config guard, and that each directory method creates and returns the expected path. Uses a temporary base directory. |
| `Endpoints/MiscEndpointsTests.cs` | Tests for `MiscEndpoints.Ping` (authenticated and unauthenticated), and `AddLogAsync` (verifies mapper and logging service are called once, result is `Ok`). |
| `Endpoints/IdentityEndpointsTests.cs` | Tests for `IdentityEndpoints.DoesAdminUserExist` (true/false) and `CreateAdminIfMissing` (returns `Created` on success, `Conflict` when admin already exists). |
| `Endpoints/ImageEndpointsTests.cs` | Tests for `ImageEndpoints.GetImage` (verifies the file service is called with the correct ID). |
| `Middleware/LoggingMiddlewareTests.cs` | Tests for `LoggingMiddleware`: verifies that a `RequestLog` is created and that the next middleware delegate is always called. |
| `Middleware/UserCreationMiddlewareTests.cs` | Tests for `UserCreationMiddleware`: verifies the next middleware delegate is called and the correct `HttpContext` is passed through. |
| `Middleware/GlobalExceptionHandlerTests.cs` | Tests for `GlobalExceptionHandler`: verifies the exception is logged to the database, the HTTP response status is set to 500, and `TryHandleAsync` returns `true`. |

#### Modified Files

| File | Change |
|---|---|
| `WardrobeManager.Api/WardrobeManager.Api.csproj` | Added `[InternalsVisibleTo("WardrobeManager.Api.Tests")]` so `internal sealed class GlobalExceptionHandler` is accessible from the test project. |

---

### WardrobeManager.Presentation.Tests

#### New Files

| File | Description |
|---|---|
| `Services/NotificationServiceTests.cs` | Tests for `NotificationService`: adding with default type (Info), adding with explicit types, removing a notification, verifying multiple notifications, and firing of the `OnChange` event. |
| `Services/IdentityServiceTests.cs` | Tests for `IdentityService`: `SignupAsync` (success, failure, error notifications added), `LoginAsync` (success, failure, error notification), `LogoutAsync` (success/failure notification, return value), and `IsAuthenticated` (authenticated vs anonymous principal). |

#### Modified Files

| File | Change |
|---|---|
| `ApiServiceTests.cs` | Added tests for `CheckApiConnection` (API up → true, connection error → false), `AddLog` (posts to `/add-log`), and `CreateAdminUserIfMissing` (success returns true + message, conflict returns false + message). |

---

### WardrobeManager.Shared.Tests

#### New Files

| File | Description |
|---|---|
| `Models/ClientClothingItemTests.cs` | Tests that `ClientClothingItem` initialises properties correctly and that `Id` defaults to zero. |
| `Models/NewOrEditedClothingItemDTOTests.cs` | Tests that `NewOrEditedClothingItemDTO` initialises all constructor parameters and that properties can be updated. |
| `Models/FilterModelTests.cs` | Tests that `FilterModel` has the correct default values (empty search, `None` enums, null date ranges) and that properties can be updated. |
| `DTOs/LogDTOTests.cs` | Tests that `LogDTO` stores all required properties correctly. |

#### Modified Files

| File | Change |
|---|---|
| `StaticResources/MiscMethodsTests.cs` | Extended with tests for `GetEmoji(ClothingCategory)`, `GetEmoji(Season)`, `GetNameWithSpacesFromEnum`, `IsValidBase64` (valid, invalid, empty), `CreateDefaultNewOrEditedClothingItemDTO`, and a uniqueness check for `GenerateRandomId`. |

---

## Test Count Summary

| Project | Tests Added | Tests Total |
|---|---|---|
| WardrobeManager.Api.Tests | +44 | 51 |
| WardrobeManager.Presentation.Tests | +27 | 29 |
| WardrobeManager.Shared.Tests | +25 | 26 |
| **Grand Total** | **+96** | **106** |

---

## What Was Not Tested (and Why)

| Item | Reason |
|---|---|
| `ClothingEndpoints`, `UserEndpoints`, `ActionEndpoints` | These endpoints rely on `context.Items["user"] as User` being populated by middleware; the middleware itself is tested and the service layer is fully covered. |
| `IdentityEndpoints.LogoutAsync`, `RolesAsync` | Require `SignInManager<User>` and `ClaimsPrincipal` with live claim types; the surrounding service tests provide equivalent coverage. |
| `CookieAuthenticationStateProvider` | Blazor WebAssembly-specific class that issues real HTTP calls to identity endpoints; not unit-testable without a running server. |
| `CookieHandler` | Thin `DelegatingHandler` wrapper around `BrowserRequestCredentials`; only meaningful in a browser-hosted WASM context. |
| `ViewModels` (`LoginViewModel`, `SignupViewModel`, etc.) | Depend on `Blazing.Mvvm` infrastructure (`ViewModelBase`, `IMvvmNavigationManager`) which requires a live Blazor host. |
| `DatabaseContext`, `DatabaseInitializer`, EF Core Repositories | Integration-level concerns (require a real or in-memory database); the service layer is mocked above the repository boundary. |
| `Migrations/` folder | Excluded per requirements. |
