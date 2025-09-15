# Prompt Library

This directory contains reusable GitHub Copilot Chat prompt files (`*.prompt.md`) that standardize planning and controlled issue creation for the project.

## Files

1. `01-planner-from-gdd.prompt.md`
   - Kanban backlog generator (Epics → Stories → Tasks) from `Documents/GDDv1.1.md`
   - Output: `Documents/planning/kanban-backlog.md`

2. `01-planner-from-gdd-to-epic.prompt.md`
   - Interactive: propose epic titles + intents, iterate, then output a single consolidated list
   - Output: `Documents/planning/epics/epics.md` (confirmed epics only)

3. `02-planner-epic-unfold.md`
   - Interactive (multi-phase):
     1) Enrich `epic-XX.md` overview sections and confirm
     2) Propose story titles, iterate, confirm
     3) Write full stories, confirm
     4) Propose tasks with types/estimates, iterate, confirm with AC
   - Output: updates `Documents/planning/epics/epic-XX.md`

4. `02-issue-creator.prompt.md`
   - (Optional Phase 2) Parses the backlog and, after explicit tokenized approval, creates GitHub Issues into a ProjectV2 board
   - Enforces dry-run preview, AC grammar checks, idempotency, label conventions

Retired / Consolidated:

- `CreateTasks.prompt.md` (replaced)
- `PlanerTasksFromGDD.prompt.md` (typo; superseded)
- `ProjectTaskCreator.prompt.md` (renamed to `02-issue-creator.prompt.md`)

## Enabling Prompt Files in VS Code

1. Open Copilot Chat (`Ctrl+I`).
2. Click the paperclip (Attach) → choose prompt file.
3. Enter your command (e.g., `Iteration = 1` or `PREVIEW 1`).

## Planner Usage (Backlog and Interactive)

Command examples:

```text
Iteration = 1
REVISE reduce tasks to 12 and add a logging story
Iteration = 2
```

Output includes: Summary, Epics, User Stories, Tasks, Parking Lot, Risks, Assumptions, Conventions.

Interactive Epic Flow (01 → single list):

```text
Attach: 01-planner-from-gdd-to-epic.prompt.md
Command: PROPOSE EPICS
REVISE Rename EP-01 to "Pipeline Validation"
CONFIRM EPICS
```

Interactive Epic Unfold Flow (02 → enrich then stories/tasks):

```text
Attach: 02-planner-epic-unfold.md
Command: ENRICH EPIC EP-00
CONFIRM EPIC EP-00
PROPOSE STORIES EP-00
REVISE STORIES Split Android+iOS into separate stories
CONFIRM STORIES
WRITE STORY US-003
CONFIRM STORY US-003
PROPOSE TASKS US-003
CONFIRM TASKS US-003
```

Acceptance Criteria rules (tasks): 3–7 bullets, objective, no trailing punctuation, no `and/or`.

## Phase 2 Usage (Issue Creator)

Two-step gated workflow:

1. Preview:

```text
PREVIEW 1
PREVIEW
PREVIEW CHANGES
```

1. Approve (token required):

```text
APPROVE ALL 2025-09-12T12:34Z
APPROVE IT1-001 IT1-004 IT1-007 APPROVALTOKEN123
APPROVE STORIES US1-01 2025-09-12Z
APPROVE EPICS EP1-01 2025-09-12Z
CANCEL
```

Revision examples:

```text
REVISE Change IT1-003 Est to S; Add AC IT1-003: Zero allocs after warmup
REVISE Retitle IT1-005: Reading session HUD
```

Approval Token: Any 8+ char alphanumeric / timestamp-like string (`[A-Za-z0-9:+T-]{8,}`) required to prevent accidental creation.

## Label & Field Conventions

Labels automatically (or implicitly) used:

- `iter:<N>`
- `type:<Type>` (Design | Code | UX | Config | CreateAsset | UseAsset | Test | Chore)
- `size:<XS|S|M>`
- `story:US<N>-NN` (on tasks)
- `epic:EP<N>-NN` (on tasks)
Optional heuristic: `area:<token>` from GDD anchor segment.

ProjectV2 attempted fields (ignored if absent): Status=Backlog, Iteration, Estimate, Type.

## Anchor Hygiene

All tasks include `GDD Trace: GDDv1.1.md#<anchor>` where `<anchor>` follows GitHub slug rules (lowercase, dashes, punctuation trimmed). If a needed concept lacks a heading, the planner adds a NOTE recommending a stub heading.

## Outcome vs Activity

Prefer: `Voice activity detector exposes stable Active flag`  
Avoid: `Implement voice detection` (activity-focused)

## Good vs Bad Task Examples

Good:

```markdown
### IT1-004 Session time accumulator (Story: US1-01, Type: Code, Est: S)
Outcome: Aggregates active voice seconds and exposes progress toward goal.
Acceptance Criteria:
- Only increments while VoiceActive true
- Drift <0.1s over simulated 300s
- Inspector shows goal + current seconds
- Update loop allocs = 0 after warmup
```

Bad:

```markdown
### IT1-004 Time system
Outcome: Make time work.
Acceptance Criteria:
- Works well and is fast
- Time tracked and displayed and saved
```

## Parameterization for Reuse

Before using in another repo, search & replace placeholder tokens:

- `<owner>/<repo>`
- `<user>` (Project board URL)
- Board URL: `https://github.com/users/<user>/projects/1`

## Idempotency Notes

Running PREVIEW repeatedly is safe. Creation step skips items whose titles already exist as Issues (marked `duplicate-existing`). To force variant creation, adjust the title or revise plan before approval.

## Roadmap Ideas (Not Implemented Yet)

- Optional JSON export of parsed plan for analytics.
- Automatic cross-linking between Story Issues and Task Issues.
- Configurable estimate scales (e.g., XS/S/M/L) via front matter flag.

## Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| No tasks parsed | Heading pattern mismatched | Ensure `IT<N>-NNN` present in heading |
| All gddTrace flagged missing | Anchor typos | Verify anchor slugs in `GDDv1.1.md` |
| Approval rejected | Missing token | Append valid token (timestamp recommended) |

---
Maintained: 2025-09-12. Update this README whenever prompt grammar or conventions change.
