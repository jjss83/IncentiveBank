# AI-Enhanced Development Principles

Purpose: Establish clear, actionable principles for using AI agents and assistants effectively in this Unity project (and general software dev). These principles focus on clarity, maintainability, and testability. Each principle links to a detailed guide with patterns and examples.

## The Six Principles

1) AI-specific comments in code
- Intent: Make agent-oriented context and rationale explicit for future automation and teammates.
- What to do: Embed concise, structured comments near the code they describe (context, intent, constraints, edge cases, dependencies, and rationale). Prefer code-local docs over external docs.
- See: `ai-comments-guide.md`.

2) Naming you can navigate
- Intent: Names should self-document roles and make code discoverable in Unity and C# ecosystems.
- What to do: Use consistent conventions for classes, assets, MonoBehaviours, ScriptableObjects, events, async, tests, and files.
- See: `ClassName.ai.md`.

3) Todo-first workflow
- Intent: Force clarity before code and ensure completeness after changes.
- What to do: Agents (and humans) generate a todo list before implementing; update and close each item post-implementation.
- See: sections in `copilot-playbook.md` and checklist updates in `code-review-checklist.md`.

4) Consistency checks
- Intent: Prefer solutions that align with the surrounding codebase. Deviations must be justified and scoped.
- What to do: Compare patterns (naming, DI, serialization, async, testing). If your approach conflicts, propose the closest consistent alternative and document rationale inline.
- See: `copilot-playbook.md` → Consistency pass; `code-review-checklist.md` additions.

5) Testability first
- Intent: Make code easy to verify quickly, locally, and repeatedly.
- What to do: Design seams, pure functions, inject dependencies, and author tests in parallel. Favor small, deterministic tests.
- See: `unit-testing-best-practices.md` and `testing-in-unity.md`.

6) Inline documentation as the source of truth
- Intent: Final documentation must live with the code that it explains.
- What to do: Keep design intent, constraints, tricky edges, and links embedded in code comments and docstrings (XML docs). External design docs are inputs; the code carries the final narrative.
- See: `ai-comments-guide.md`.

## How to use these principles

- Before you code: write a short todo list, note assumptions, and pick names that reflect intent.
- While you code: embed AI-specific comments near key logic. Keep functions small; expose test seams.
- After you code: run a consistency pass and ensure tests exist. Close all todos. Update inline docs if anything changed.
- In reviews: use the checklist to verify adherence.

## Quick links

- AI comments guide: `ai-comments-guide.md`
- Naming convention: `ClassName.ai.md`
- Unit testing (general): `unit-testing-best-practices.md`
- Unity testing (Edit/Play): `testing-in-unity.md`
- Copilot playbook (AI workflow): `copilot-playbook.md`
- Code review checklist: `code-review-checklist.md`

## References

- Microsoft C# naming guidelines: https://learn.microsoft.com/dotnet/standard/design-guidelines/naming-guidelines
- NUnit docs (Unity uses NUnit): https://docs.nunit.org/
- Unity Test Framework: https://docs.unity3d.com/Packages/com.unity.test-framework@latest
- Unity Scripting API: ScriptableObject: https://docs.unity3d.com/ScriptReference/ScriptableObject.html