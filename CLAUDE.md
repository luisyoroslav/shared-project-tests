# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Purpose

PoC for sharing .NET domain code across client-specific assemblies via a **Shared Project** (`.shproj`) plus per-client `partial class` extensions and conditional compilation. There is no runtime DI/composition layer — variation is resolved entirely at MSBuild/compile time.

## Common commands

```bash
# Build everything
dotnet build SharedProjectTest.sln

# Build a single client (domain or console)
dotnet build ConsoleSony/ConsoleSony.csproj

# Run a specific client console app
dotnet run --project ConsoleSony

# Inspect what the conditional MSBuild logic resolved to (DefineConstants, HasSubfiles)
dotnet build ConsoleSony/ConsoleSony.csproj -v normal
# The `BeforeResolveReferencesCustom` target in projitems prints these at "high" importance.
```

Target framework is `net9.0`. There is no test project, lint config, or CI in this repo.

## Architecture

The solution simulates a white-label product where one core "Blackfish" domain is compiled differently for each client (Canalplus, Hawk, Sony, StudioCloud, Warner). The variation mechanism is the interesting part — read these four files together to understand it:

1. **`Blackfish.Shared.Domain/Blackfish.Shared.Domain.projitems`** — the actual shared sources. It is **imported directly** by each client `.csproj` (`<Import Project="..\Blackfish.Shared.Domain\Blackfish.Shared.Domain.projitems" Label="Shared" />`), not referenced as an assembly. Files listed under `<ItemGroup><Compile Include=... />` are compiled into every consumer. The `BeforeResolveReferencesCustom` target conditionally adds `Subfile.cs` only when `$(HasSubfiles) == 'True'`.

2. **`Directory.Build.props`** — auto-applied to every project. Inspects `$(MSBuildProjectName)` (case-insensitive substring match) and sets `GlobalConstants` to flags like `Sony;HasSubfiles`, `Canalplus`, `Warner;HasSubfiles`, etc. These are appended to `DefineConstants`, then `HasSubfiles` is derived by regex on `DefineConstants`. Current mapping:
   - `Sony`, `StudioCloud`, `Warner` → `HasSubfiles` enabled
   - `Canalplus`, `Hawk` → no subfiles

3. **`Directory.Build.targets`** — forwards `GlobalConstants` to every `ProjectReference` via `AdditionalProperties`. This is what makes a `Console*` project pass its own constants down into its referenced `Blackfish.<Client>.Domain` project at build time.

4. **Client `.csproj`** (e.g., `Blackfish.Sony.Domain.csproj`) — imports the shared `.projitems` and may also contain a small per-client folder (e.g., `Blackfish/Campaigns/Campaign.cs`) that uses `partial class` to add client-specific properties to a shared type (Sony adds `CountryId`, Canalplus adds `ParentId`, etc.). Console apps (`Console<Client>`) reference exactly one client domain via `<ProjectReference>` and act as smoke tests.

### Consequences when editing

- **Changing a shared type**: edit under `Blackfish.Shared.Domain/Blackfish/...`. Every client recompiles it. If the shared file should be conditionally included, update both the `<ItemGroup>` (compiled unconditionally) and the `BeforeResolveReferencesCustom` target (conditionally added) in `Blackfish.Shared.Domain.projitems` — note `Subfile.cs` is intentionally listed only in the target, not in the unconditional `<ItemGroup>`.
- **Adding a client-specific property** to a shared type: declare the shared type as `partial` and add a matching `partial class` file under the client's `Blackfish.<Client>.Domain/Blackfish/...` folder. Don't add per-client `#if` blocks to shared files unless that's the only option.
- **Adding a new client**: create `Blackfish.<Name>.Domain.csproj` (importing the shared `.projitems`) plus a `Console<Name>` exe project, add both to `SharedProjectTest.sln`, register the `SharedMSBuildProjectFiles` GUID entry, and add a `GlobalConstants` line in `Directory.Build.props` (include `HasSubfiles` if the client should compile `Subfile.cs`).
- **`Blackfish.Canalplus.Domain.csproj`** has `<None Remove="Blackfish\Campaigns\Campaign.cs" />` — this prevents the projitems' `<None Include="...\**\*.cs" />` glob (used for IDE visibility) from double-listing the local Campaign.cs, not a real exclusion.
- **`Blackfish.Warner.Domain.csproj`** has `<Compile Remove="Class1.cs" />` to drop a leftover scaffolded file; don't reintroduce it.
