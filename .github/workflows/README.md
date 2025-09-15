# Workflows

This folder contains GitHub Actions workflows for this repository.

## Commit Changes with Approval

Workflow file: `auto-commit-with-approval.yml`

Purpose: Stage all local changes in the repository, generate an auto-commit message, and then wait for human approval before pushing. If the push is blocked (branch protection), it can optionally open a pull request from a temporary branch.

### Setup

1. Create a protected Environment named `commit-approval` in your repository settings and add at least one reviewer. The `commit` job targets this environment and will pause until approval

1. Ensure the repository (or organization) allows GitHub Actions to create and approve pull requests if you intend to use the PR fallback. The workflow sets permissions:

- `contents: write`
- `pull-requests: write`

1. Optional: Define a repository variable `DATE_UTC` if you want a custom date string in summaries; otherwise the workflow uses `date -u` directly

### Usage

- Manually from the Actions tab: Run the workflow `Commit Changes with Approval`.
- Inputs:
  - `commit_prefix` (string, optional): Prefix the commit subject, e.g., `feat`, `chore`, `docs`
  - `target_branch` (string, optional): Target branch to push to; defaults to the branch you run it from
  - `pr_fallback` (boolean, default `true`): If direct push fails, open a PR from a temporary branch

When there are changes, the workflow will:

- Generate a commit message preview and attach an artifact
- Wait on the `commit-approval` environment for human approval
- Commit and push to the target branch, or open a PR if push fails and fallback is enabled

### Notes

- If there are no changes, the workflow exits early.
- The fallback PR uses a branch named `auto/commit-YYYYMMDD-HHMMSS`.
- Git author is set to `${GITHUB_ACTOR} <${GITHUB_ACTOR}@users.noreply.github.com>`.
- The workflow can also be called by other workflows using `workflow_call` with the same inputs.
