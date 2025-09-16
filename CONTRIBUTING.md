# Contributing to IncentiveBank

Thanks for helping build IncentiveBank! This guide explains how we branch, plan, implement, and open pull requests. Keep it simple, keep it shippable.

## Prerequisites

- Unity Editor: 6000.0.48f1 (see `ProjectSettings/ProjectVersion.txt`)
- Target modules as needed (Windows, Android, iOS, WebGL)

## Branching

Prefer using the GitHub Action to create a branch with a clean, consistent name.

1. In GitHub → Actions → "Create Feature Branch" → Run workflow

- Feature title: short human title (e.g., Add splash bootstrap)
- Base: `main` (or another branch)
- Prefix: `feature` (or `fix` / `chore`)

This creates a branch like `feature/<actor>/add-splash-bootstrap-YYYYMMDD` and pushes it to origin.

Fallback (local):

```powershell
# Windows PowerShell
git fetch origin ; git switch -c feature/yourname/short-slug-20250914 origin/main
```

## Commits

- Small, focused commits; present tense, imperative mood
- Reference IDs when relevant: `EP-00`, `US-003`, `TK-0006`
- Example: `feat(bootstrap): log Startup version/platform (US-001 TK-0001)`

## Pull Requests

PR checklist:

- Title states the outcome (what’s true after merge)
- Link the Epic/Story/Tasks (e.g., EP-00 / US-001 / TK-0001)
- Include screenshots/gifs for visual changes
- Unity: project opens cleanly, Play from Bootstrap works (no errors; warnings ≤ 2)
- Docs updated if behavior or plan changed (epic/story/task md files)

Scope: Prefer one story per PR (or a coherent slice). Keep PRs under ~400 lines net-diff where possible.

## Planning (Interactive Prompts)

We plan epics, stories, and tasks using prompt files in `.github/prompts/` via Copilot Chat (Attach → select prompt).

1. Epic titles and intents → full epic files

- Attach: `01-planner-from-gdd-to-epic.prompt.md`
- Commands:
  - `PROPOSE EPICS`
  - `REVISE <instructions>`
  - `CONFIRM EPICS`
- Output: `Documents/planning/epics/epic-XX.md` with standard sections

1. Unfold an epic → stories → story details → tasks

- Attach: `02-planner-epic-unfold.prompt.md`
- Commands:
  - `PROPOSE STORIES EP-XX`
  - `REVISE STORIES <instructions>` → `CONFIRM STORIES`
  - `WRITE STORY US-XXX` → `CONFIRM STORY US-XXX`
  - `PROPOSE TASKS US-XXX` → `CONFIRM TASKS US-XXX`
- Constraints:
  - Every story has exactly one Design task first
  - Task AC: 3–7 bullets, objective, no trailing punctuation, no `and/or`
  - Use bracketed type prefixes where helpful, e.g., `[Unity Config]`

## Task Types & Labels

Types (use in task titles and labels):

- Unity Config, Script Development, Asset Creation, Platform Setup/Verification, CI/CD, Signing & Release, Documentation, QA/Validation

Suggested labels:

- `type:<type>` (e.g., `type:unity-config`)
- `size:<XS|S|M>`
- `platform:<windows|android|ios|webgl>`

## Unity Conventions

- Scenes:
  - `Assets/Scenes/Bootstrap.unity` (entry) → loads `Assets/Scenes/Splash.unity`
  - Add both in Build Settings (Bootstrap first)
- Scripts:
  - `Assets/Scripts/Bootstrap/Bootstrap.cs` → logs startup and loads Splash
  - `Assets/Scripts/Bootstrap/VersionLabel.cs` → shows `Application.version` and platform
- Assets:
  - `Assets/Art/Textures/splash.png` (placeholder splash)
- Logging:
  - On startup: `Startup: version=<v> platform=<p>` via `Debug.Log`
- Performance hygiene:
  - Avoid per-frame allocations in `Update`
  - Prefer `Awake/Start` initialization and pooling where applicable
- Settings (baseline):
  - Mobile: IL2CPP, .NET Standard 2.1, Android ARM64 only
  - "Allow downloads over HTTP" = Not allowed (default secure)

## File & Doc Hygiene

- Keep epic/story/task markdown scannable; no trailing punctuation on AC bullets
- Use stable IDs: `EP-XX`, `US-XXX`, `TK-XXXX`
- Include GDD trace anchors when relevant: `GDDv1.1.md#<anchor>`

## Secrets & Signing

- Do not commit secrets or keystores
- Mobile signing: see `Documents/build/signing-notes.md` (placeholders only)

## Getting Help

Open a discussion or drop a note in `Documents/planning/` with your questions or proposals. When in doubt, propose a tiny slice and iterate.
