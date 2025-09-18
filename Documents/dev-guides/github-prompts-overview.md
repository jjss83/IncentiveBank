# Planning Prompts Overview

IncentiveBank uses interactive prompt files stored in `.github/prompts/` to
guide planning activities via GitHub Copilot Chat or similar tools.  These
prompts help you derive Epics, user stories and tasks from the game design
document (GDD) and align them with the developer guides.

## How it works

1. **Write Epics**.  Attach the prompt `01-write-epic.prompt.md` in
   Copilot Chat.  This will propose 2–6 epics from the GDD, iterate on
   names and intents, and produce `Documents/planning/epics/epics.md`.  The
   prompt reminds you to consult the feature strategy template and Unity
   basics guides when defining epics.
2. **Unfold Epics into stories and tasks**.  Attach the prompt
   `02-write-user-stories.prompt.md`.  It enriches a selected epic with
   narrative, goals and risks, then proposes user story titles, fleshes
   them out, and generates tasks with estimates.  The prompt references
   the slice checklist and testing guide to ensure stories are small,
   vertical slices and each task includes acceptance criteria.
3. **Develop Unity script features**. Attach `03-develop-script-feature.prompt.md`.
  It classifies a scripting ask (Minor, Major, Bug, New Feature), proposes the
  right template (note or design doc), and then guides implementation in
  small, demoable slices with matching edit-mode/play-mode tests aligned to the
  Unity basics, slice checklist and testing guide.
4. **Change Unity configuration**. Attach `04-unity-config-change.prompt.md`.
  Use this when modifying Player/Project/Build/Input/Quality settings. It follows
  a learn → plan → step-by-step approval flow, applies reversible changes, and
  provides verification checklists or notes.

## Best practices

* **Prepare context**.  Before using the prompts, skim the relevant parts
  of the GDD and the developer guides (strategy template, Unity basics,
  slice checklist, testing guide).  This helps you evaluate Copilot’s
  suggestions quickly.
* **Iterate interactively**.  Use the `REVISE` commands to rename or
  reorder items until the epics and stories align with the project’s
  constraints.  Don’t accept a list that doesn’t make sense just because
  the tool proposed it.
* **Link back to guides**.  Each epic and story should reference the
  relevant sections of the GDD and the developer guides (e.g.
  “proven VAD patterns” or “offline JSON storage”).
* **Follow the slice definition**.  When creating tasks, ensure each task
  fits within a day, includes tests and produces an observable outcome.

These prompts are meant to speed up planning, not replace critical
thinking.  Use them alongside the guides to maintain clarity and
consistency across the project.
