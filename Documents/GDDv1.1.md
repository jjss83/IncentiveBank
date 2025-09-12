# Reading Rewards — GDD v1.1 (Finalized for MVP)

## Decisions locked (from you)

* **Content format:** JSON (passages + manifest).
* **Defaults:** `sessionGoalMinutes = 5`, `rewardTokensPerGoal = 1`.
* **Celebration:** Tiny token pop animation on reward.
* **Windows audio:** Keep default WASAPI modes.
* **Difficulty tags:** `early`, `older`, `strict`.
* **ASR/phonemes:** Post-MVP (ES/EN).

## Executive Summary (unchanged essence)

Kids read **on-screen** passages aloud. On-device VAD confirms “human voice present”; optional **caret mode** lets kids slide a finger across words. Hitting the **goal time** grants tokens. Everything is **offline**, **local JSON-configured**, bilingual **ES/EN**, and **no user accounts**.

### Platforms & Tech

Android tablet/phone, iPad, Windows laptop with mic • Unity (latest beta; fallback to latest LTS if needed) • TextMeshPro • Unity Microphone API • local JSON storage.

---

## Core Loops (compact)

* **Read loop:** Voice present → timer counts → (optional) caret highlights words.
* **Session loop:** Reach 5 min → award 1 token → tiny pop animation → log → home.
* **Parent loop:** Edit `settings.json` + content folder; no in-app controls in MVP.

---

## Mechanics (finalized)

### VAD (voice activity detection)

* Calibrate ambient floor on start; compute RMS energy per 10–30ms frame.
* Hysteresis thresholds (enter/exit) + 600ms debounce to avoid blips.
* Optional spectral flatness check to suppress TV/music.
* Count **seconds of voice** toward `sessionGoalMinutes`.

### Caret/Strictness

* **Default (early/older):** Voice alone counts time; caret highlighting is optional for engagement.
* **Strict mode (category = `strict` or `strictMode: true` in settings):** Timer counts **only when** voice is present **and** the caret advances periodic words (e.g., at least 1 word/3s). This simulates “word-by-word” without ASR.

### Rewards

* Grant `rewardTokensPerGoal` (default 1) when goal met; show **token pop** animation (200–400ms scale+ease).
* No streaks/cooldowns in MVP.

---

## Content & Localization (final)

* **Folder:** `/Content` with a `manifest.json`, ES/EN subfolders, and passage JSON files.
* **Difficulty tags:** `early`, `older`, `strict`.
* **UI localization:** `/Localization/ui_en.json`, `/Localization/ui_es.json`.

**Passage JSON (locked)**

```json
{
  "id": "en_beg_001",
  "locale": "en",
  "title": "The Cat on the Mat",
  "difficulty": "early",
  "body": "The cat sits on the mat.\nIt purrs softly.\nI read to my cat.",
  "wordBoundaries": null
}
```

* `wordBoundaries` optional; if `null`, split by whitespace.
* For Spanish, same structure with `"locale": "es"`.

**Manifest JSON (locked)**

```json
{
  "locales": ["en", "es"],
  "groups": [
    {
      "id": "en.early",
      "title": "Early (EN)",
      "files": ["en/early/en_beg_001.json", "en/early/en_beg_002.json"]
    },
    {
      "id": "es.early",
      "title": "Principiante (ES)",
      "files": ["es/early/es_pri_001.json"]
    }
  ]
}
```

---

## Settings & Logs (JSON, locked)

**settings.json**

```json
{
  "locale": "en",
  "sessionGoalMinutes": 5,
  "rewardTokensPerGoal": 1,
  "strictMode": false,
  "contentRoot": "Content",
  "incentives": ["screen_time", "allowance", "stickers"],
  "difficultyDefault": "early"
}
```

**logs.json**

```json
{
  "tokenTotal": 0,
  "sessions": [
    {
      "ts": "2025-09-11T22:00:12Z",
      "passageId": "en_beg_001",
      "difficulty": "early",
      "secondsCounted": 312,
      "goalMet": true,
      "tokensAwarded": 1,
      "strictMode": false
    }
  ]
}
```

---

## UX/UI (lean)

* **Home:** Title, token total, Start Reading, Passages.
* **Passage Select:** Lists by locale+difficulty.
* **Mic Calibrate:** Single overlay with level meter; “Quiet please…” hint.
* **Reading View:** Large TMP text, timer chip, “voice detected” indicator, caret toggle, End Session.
* **Reward Screen:** Token total bumps with **tiny pop** animation; “Back to Home.”

Theme: **kid-oriented, simple, clean**.

---

## Audio/Perf

* Frame budget: VAD <3% CPU on mid devices; zero allocations per audio frame.
* Windows: keep default WASAPI settings.

---

## Acceptance Criteria (updated)

1. Counts time **only** when voice is detected; pause during silence.
2. **Strict mode** (when enabled): timer counts only with **voice + caret advancement** cadence.
3. Reaching `sessionGoalMinutes` grants `rewardTokensPerGoal` and plays **token pop** animation.
4. Content loads from fixed `/Content`; locale toggle shows ES/EN lists.
5. No recordings saved; only aggregate session stats to `logs.json`.
6. Works on Android, iPad, and Windows with default mic settings.

---

## Risks & Mitigations (kept tight)

* **Music/TV false positives:** spectral flatness + debounce + “noise warning” coach mark.
* **Caret compliance fatigue in strict:** soften cadence requirement (e.g., 1 word/5s) and visual nudge.
* **Unity beta stability:** if blocked, move to latest LTS; no feature impact.

---

## Roadmap (unchanged structure)

* **M0 Prototype:** Mic + VAD + timer + reading view + token grant + JSON I/O.
* **M1 MVP Hardening:** Strict mode, locale switching, basic ES/EN content, error handling.
* **M2 Nice-ities:** Token pop polish, accessibility toggles, better noise hints.

---

# Drop-in Starter Files (copy, rename, and go)

**/AppData/settings.json** (ready-to-use)

```json
{
  "locale": "en",
  "sessionGoalMinutes": 5,
  "rewardTokensPerGoal": 1,
  "strictMode": false,
  "contentRoot": "Content",
  "incentives": ["screen_time", "allowance", "stickers"],
  "difficultyDefault": "early"
}
```

**/AppData/logs.json**

```json
{ "tokenTotal": 0, "sessions": [] }
```

**/AppData/Content/manifest.json**

```json
{
  "locales": ["en", "es"],
  "groups": [
    { "id": "en.early", "title": "Early (EN)", "files": ["en/early/en_beg_001.json"] },
    { "id": "es.early", "title": "Principiante (ES)", "files": ["es/early/es_pri_001.json"] }
  ]
}
```

**/AppData/Content/en/early/en\_beg\_001.json**

```json
{
  "id": "en_beg_001",
  "locale": "en",
  "title": "Sunny Spots",
  "difficulty": "early",
  "body": "Sun on the rug.\nI sit and read.\nWords glow like dots.",
  "wordBoundaries": null
}
```

**/AppData/Content/es/early/es\_pri\_001.json**

```json
{
  "id": "es_pri_001",
  "locale": "es",
  "title": "Luz en el piso",
  "difficulty": "early",
  "body": "Luz en el piso.\nMe siento a leer.\nLas palabras brillan.",
  "wordBoundaries": null
}
```

**/AppData/Localization/ui\_en.json**

```json
{
  "home_title": "Reading Rewards",
  "start_reading": "Start Reading",
  "passages": "Passages",
  "tokens": "Tokens",
  "mic_calibrate": "Calibrate Microphone",
  "voice_detected": "Voice detected",
  "end_session": "End Session",
  "goal_reached": "Goal reached!"
}
```

**/AppData/Localization/ui\_es.json**

```json
{
  "home_title": "Recompensas de Lectura",
  "start_reading": "Empezar a leer",
  "passages": "Lecturas",
  "tokens": "Fichas",
  "mic_calibrate": "Calibrar micrófono",
  "voice_detected": "Voz detectada",
  "end_session": "Terminar sesión",
  "goal_reached": "¡Objetivo logrado!"
}
```
