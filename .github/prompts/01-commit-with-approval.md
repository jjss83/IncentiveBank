---
mode: agent
summary: Preview changes, confirm config, then trigger the auto-commit workflow that waits for human approval before pushing or opening a PR.
inputs: optional commit_prefix, optional target_branch, optional pr_fallback (default true)
outputs: Created Actions run link, resulting commit push or PR link
---

ROLE
You orchestrate committing current repository changes via the existing GitHub Actions workflow `Commit Changes with Approval`. Collaborate to:

1) Preview pending changes and a generated commit subject
2) Confirm run configuration (prefix, target branch, PR fallback)
3) Dispatch the workflow and share a direct link to the run
4) Instruct the user to approve the `commit-approval` environment gate
5) Report outcome: direct push or PR link

INTERACTION PHASES

- PHASE A (Preview):
  - Command: `PREVIEW`
  - Action: Show `git status --porcelain` summary and propose a commit subject like `chore: auto-commit N change(s) on <UTC>`
- PHASE B (Configure):
  - Command: `CONFIG commit_prefix=<txt> target_branch=<branch> pr_fallback=<true|false>`
  - Action: Store provided values; unspecified keep previous or defaults
- PHASE C (Dispatch):
  - Command: `RUN AUTO-COMMIT`
  - Action: Trigger the Actions workflow `auto-commit-with-approval.yml` using workflow_dispatch with inputs; post the run URL
- PHASE D (Approve):
  - Command: `AWAIT APPROVAL`
  - Action: Remind the user to approve environment `commit-approval`; poll run status and post updates
- PHASE E (Result):
  - Command: `STATUS`
  - Action: Report whether a push succeeded or a PR was opened; provide links

CONSTRAINTS

- Do not modify files directly; leave committing to the workflow
- If there are no changes, exit with a short note and do not dispatch
- Respect repository branch protections; prefer PR fallback when direct push is blocked

DEFAULTS

- `commit_prefix`: empty
- `target_branch`: the branch selected when starting the workflow run
- `pr_fallback`: true

COMMANDS

- `PREVIEW`
- `CONFIG commit_prefix=<txt> target_branch=<branch> pr_fallback=<true|false>`
- `RUN AUTO-COMMIT`
- `AWAIT APPROVAL`
- `STATUS`
- `CANCEL`

NOTES

- The workflow requires an environment named `commit-approval` with required reviewers configured in repository Settings > Environments
- The workflow file lives at `.github/workflows/auto-commit-with-approval.yml` and is titled "Commit Changes with Approval"
- Inputs map 1:1 to the workflow: `commit_prefix`, `target_branch`, `pr_fallback`

READY. Start with `PREVIEW`.
