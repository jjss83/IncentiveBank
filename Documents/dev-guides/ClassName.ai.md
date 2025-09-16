ClassName.ai.md — Naming Conventions for Unity and C#
=====================================================

Scope
-----
Practical naming rules to improve discoverability, refactoring safety, and agent comprehension across Unity and general C#.

General C#
----------
- Classes, structs, enums: PascalCase (e.g., `RewardCalculator`)
- Methods, properties, events: PascalCase (e.g., `Calculate`, `Points`, `OnLimitReached`)
- Fields: `camelCase` private, `PascalCase` public; prefer private with properties
- Constants: `PascalCase` or `SCREAMING_SNAKE_CASE` for interop contexts
- Interfaces: Prefix with `I` (e.g., `IRewardSource`)
- Generics: `TThing` (e.g., `TContext`)
- Async methods: suffix `Async` and return `Task`/`UniTask` if used
- Tests: Mirror type under test + scenario (e.g., `RewardCalculator_CapDaily_ShouldClampAtLimit`)

Unity-Specific
--------------
- MonoBehaviour classes: VerbNoun or NounRole (e.g., `RewardUIController`, `PlayerInputRouter`)
- ScriptableObjects: NounConfig/Asset (e.g., `RewardTierConfig`)
- Editor scripts: Suffix `Editor` (e.g., `RewardTierConfigEditor`)
- Events: `OnEventName` pattern (e.g., `OnRewardGranted`)
- Asset files: Match class name (Unity guidance); avoid spaces (e.g., `RewardTierConfig.asset`)
- Serialized fields: prefer `[SerializeField] private` with `PascalCase` property wrapper
- Menu paths: Use clear namespaces (e.g., `Rewards/TierConfig`)

Files & Folders
---------------
- One top-level type per file; file name equals type name
- Tests: mirror source folder structure; suffix files with `Tests`
- Editor code in `Editor/` folders; runtime code in `Runtime/` or `Scripts/`
- Assembly definitions: Reflect module name (e.g., `IncentiveBank.Core`)

Domain Patterns
---------------
- Calculators and policies: `XCalculator`, `XPolicy`
- Providers and services: `XProvider`, `XService`
- Repositories and stores: `XRepository`, `XStore`
- Messages/events: `XRequested`, `XCompleted`, `XFailed`
- DTOs: `XDto`; avoid leaking domain logic into DTOs

Rationale & Tips
----------------
- Prefer intention-revealing names over comments
- Avoid abbreviations unless standard (e.g., `UI`, `ID`, `DTO`)
- Keep names stable; rename broadly with IDE tooling
- Align with surrounding code; do not introduce novel prefixes/suffixes casually

References
----------
- Microsoft C# naming guidelines: https://learn.microsoft.com/dotnet/standard/design-guidelines/naming-guidelines
- Unity manual (naming and folders): https://docs.unity3d.com/Manual/SpecialFolders.html
- NUnit test naming ideas: https://docs.nunit.org/
