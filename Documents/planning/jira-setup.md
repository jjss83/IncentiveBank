# Jira Setup for IncentiveBank

This project can create Jira issues directly from the Kanban backlog using your Atlassian Cloud.

## Target Cloud & Project

- Cloud: <https://juanjosess.atlassian.net>
- Project: MBA (board 1)
- Note: If cloudId or issue type IDs are unknown, fetch them once (see below) and update `jira.config.json` accordingly.

## Local Config File

Saved at `Documents/planning/jira.config.json`:

- Maps EP-/US-/TK- IDs to Jira: Epic / Story / Task
- Includes site URL, cloudId (optional until fetched), project key/id

### Fetching IDs (one-time)

If you need to populate `cloudId`, project `id`, or issue type IDs, use your preferred Jira API client or CLI. Typical sequence:

1. List accessible clouds to get `cloudId` for `juanjosess.atlassian.net`.
2. List projects, find key `MBA`, record its numeric `id`.
3. Get issue type metadata for project `MBA` and copy the IDs for Epic/Story/Task/Bug/Subtask into `jira.config.json`.

Or run the helper script provided in this repo (Windows PowerShell):

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
powershell -File "x:\workspace\IncentiveBank\Documents\planning\jira-populate-ids.ps1"
```

## Authentication (Personal Access Token)

Create a token at: <https://id.atlassian.com/manage-profile/security/api-tokens>

Store your credentials locally as environment variables (PowerShell):

```powershell
# Set once per session (Windows PowerShell)
$env:JIRA_EMAIL="your.email@example.com"
$env:JIRA_API_TOKEN="<paste-api-token>"
```

Optional: make persistent for your user profile:

```powershell
[Environment]::SetEnvironmentVariable("JIRA_EMAIL","your.email@example.com","User")
[Environment]::SetEnvironmentVariable("JIRA_API_TOKEN","<paste-api-token>","User")
```

## Mapping Policy

- `EP-XX` → Jira Epic
- `US-XXX` → Jira Story
- `TK-XXXX` → Jira Task (default)
- Labels added: `repo:IncentiveBank`, `area:<gdd-trace-area>` when derivable

## Usage Flow

1. Ensure `Documents/planning/kanban-backlog.md` is up to date
2. Run PREVIEW using the Issue Creator prompt
3. Run CREATE commands to push Items to Jira project MBA

## Troubleshooting

- 401 Unauthorized: check `$env:JIRA_EMAIL` and `$env:JIRA_API_TOKEN`
- 403 Forbidden: verify you have create permissions on project MBA
- Missing types: confirm issue type IDs in `jira.config.json` match project
- Network: retry; partial failures will be reported and skipped
