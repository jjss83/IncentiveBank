# Commit and Pull Request Guide

Good commit messages and small pull requests make it easier for your team
to understand changes, review them quickly and maintain a clean history.

## Commit hygiene

* **Atomic changes**: Each commit should address a single idea (e.g.
  implement VAD calibration, add a test) rather than bundling multiple
  features.
* **Present tense, imperative mood**: Write commit messages like
  “Add VAD calibration thresholds” instead of “Added VAD calibration”.
* **Reference IDs**: When relevant, include the epic, user story or task
  IDs (e.g. `EP-01`, `US-005`, `TK-0042`).  Example:

  ```
  feat(timer): add strict mode counting logic (US-003 TK-0007)
  ```

* **Conventional prefixes**: Use `feat` for new functionality, `fix` for
  bug fixes, `chore` for maintenance, `test` for adding tests and
  `docs` for documentation updates.  Avoid new prefixes unless the team
  agrees on them.

## Pull request guidance

* **One story per PR**: Prefer a single user story or slice per pull
  request.  Keep net diffs under ~400 lines where possible.
* **Describe the outcome**: The PR title should state what’s true after
  merging (e.g. “Timer pauses during silence”).  Avoid references to
  implementation details.
* **Link to planning docs**: In the PR body, link to the feature strategy,
  user story or task documents.  Copy the relevant sections or quote
  acceptance criteria so reviewers have context.
* **Checklist**: Verify that your PR meets these criteria before
  requesting review:

  - [ ] All acceptance criteria satisfied
  - [ ] Tests pass locally and in CI
  - [ ] Unity project opens with no errors or new warnings
  - [ ] Docs updated (if behaviour changed)
  - [ ] Screenshots or GIFs included for UI changes

* **Reviewers**: Tag the appropriate reviewers (e.g. owner of the epic)
  and request feedback early.  Encourage small, frequent feedback cycles.

For the structure of PR descriptions, see the **pull request template** in
`.github/pull_request_template.md`.
