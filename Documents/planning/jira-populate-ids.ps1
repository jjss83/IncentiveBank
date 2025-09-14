Param(
  [string]$ConfigPath = "x:\workspace\IncentiveBank\Documents\planning\jira.config.json"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Info($msg) { Write-Host "[info] $msg" -ForegroundColor Cyan }
function Write-Warn($msg) { Write-Host "[warn] $msg" -ForegroundColor Yellow }
function Write-Err($msg)  { Write-Host "[error] $msg" -ForegroundColor Red }

if (-not (Test-Path -LiteralPath $ConfigPath)) {
  Write-Err "Config not found at $ConfigPath"
  exit 1
}

$configRaw = Get-Content -LiteralPath $ConfigPath -Raw -Encoding UTF8
$config = $configRaw | ConvertFrom-Json

if (-not $config.site) { Write-Err "Missing 'site' in config"; exit 1 }
if (-not $config.project -or -not $config.project.key) { Write-Err "Missing 'project.key' in config"; exit 1 }

$email = $env:JIRA_EMAIL
$token = $env:JIRA_API_TOKEN
if (-not $email -or -not $token) {
  Write-Err "Set JIRA_EMAIL and JIRA_API_TOKEN environment variables before running."
  exit 1
}

$pair   = "$email:$token"
$bytes  = [System.Text.Encoding]::UTF8.GetBytes($pair)
$basic  = [System.Convert]::ToBase64String($bytes)
$headers = @{ Authorization = "Basic $basic"; Accept = "application/json" }

$site = $config.site.TrimEnd('/')
$projectKey = $config.project.key

function Try-GetCloudId {
  try {
    $url = "$site/_edge/tenant_info"
    $resp = Invoke-RestMethod -Method GET -Uri $url -Headers $headers -ErrorAction Stop
    if ($resp -and $resp.cloudId) { return $resp.cloudId }
  } catch { Write-Warn "Could not fetch cloudId from _edge/tenant_info: $($_.Exception.Message)" }
  return $null
}

function Get-ProjectInfo($key) {
  $url = "$site/rest/api/3/project/$key"
  return Invoke-RestMethod -Method GET -Uri $url -Headers $headers -ErrorAction Stop
}

function Get-IssueTypeIdsForProject($key) {
  # Use createmeta to get issuetypes available for the project
  $url = "$site/rest/api/3/issue/createmeta?projectKeys=$key"
  $meta = Invoke-RestMethod -Method GET -Uri $url -Headers $headers -ErrorAction Stop
  if (-not $meta.projects -or $meta.projects.Count -eq 0) { throw "No project data in createmeta" }
  $it = @{}
  $types = $meta.projects[0].issuetypes
  foreach ($t in $types) {
    $norm = ($t.name -replace '[^a-z]', '').ToLower()
    $it[$norm] = $t.id
  }
  return $it
}

function Map-IssueTypes($typeMap) {
  # Normalize target names
  $want = @{
    epic     = 'Epic'
    story    = 'Story'
    task     = 'Task'
    bug      = 'Bug'
    subtask  = 'Subtask'
  }
  $resolve = @{}
  foreach ($k in $want.Keys) {
    $needle = $k
    $id = $null
    if ($typeMap.ContainsKey($needle)) { $id = $typeMap[$needle] }
    elseif ($needle -eq 'subtask' -and $typeMap.ContainsKey('subtask')) { $id = $typeMap['subtask'] }
    elseif ($needle -eq 'subtask' -and $typeMap.ContainsKey('subtask')) { $id = $typeMap['subtask'] }
    elseif ($needle -eq 'subtask' -and $typeMap.ContainsKey('subtask')) { $id = $typeMap['subtask'] }
    else {
      # Try common variants
      $variants = @('sub-task','standard task') | ForEach-Object { ($_ -replace '[^a-z]', '').ToLower() }
      foreach ($v in $variants) { if ($typeMap.ContainsKey($v)) { $id = $typeMap[$v]; break } }
    }
    if ($id) { $resolve[$want[$k]] = "$id" }
  }
  return $resolve
}

try {
  Write-Info "Fetching project info for '$projectKey'..."
  $proj = Get-ProjectInfo -key $projectKey
  $config.project.id = "$($proj.id)"
  $config.project.name = $proj.name
  Write-Info "Project id: $($config.project.id) | name: $($config.project.name)"

  Write-Info "Fetching issue type metadata..."
  $tmap = Get-IssueTypeIdsForProject -key $projectKey
  $resolved = Map-IssueTypes -typeMap $tmap

  if (-not $config.issueTypes) { $config | Add-Member -NotePropertyName issueTypes -NotePropertyValue (@{}) }
  foreach ($k in @('Epic','Story','Task','Bug','Subtask')) {
    if ($resolved.ContainsKey($k)) { $config.issueTypes[$k] = $resolved[$k] }
    elseif (-not $config.issueTypes[$k]) { $config.issueTypes[$k] = "" }
  }
  Write-Info ("Issue type IDs: " + ($config.issueTypes | ConvertTo-Json -Compress))

  if (-not $config.cloudId) {
    Write-Info "Attempting to fetch cloudId..."
    $cid = Try-GetCloudId
    if ($cid) { $config.cloudId = "$cid"; Write-Info "cloudId: $cid" }
    else { Write-Warn "cloudId not resolved; leaving as null" }
  }

  Write-Info "Writing updated config to $ConfigPath"
  $config | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $ConfigPath -Encoding UTF8
  Write-Host "Done." -ForegroundColor Green
}
catch {
  Write-Err $_
  exit 1
}
