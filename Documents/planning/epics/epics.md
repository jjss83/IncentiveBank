# Epic List (from GDD)

EP-00 — Bootstrap & Splash (Hello World)
Intent: Show a simple branded splash and “Hello” screen to validate startup pipeline and rendering across target devices; GDD: `GDDv1.1.md#uxui-lean`

EP-01 — Voice Activity Detection (VAD) Core
Intent: Provide reliable on-device voice activity detection with hysteresis/debounce to accurately count reading time; GDD: `GDDv1.1.md#vad-voice-activity-detection`

EP-02 — Strict Mode & Caret Cadence
Intent: Count time only when voice is present and a caret advances at a minimum cadence to simulate word-by-word compliance; GDD: `GDDv1.1.md#caretstrictness`

EP-03 — Rewards & Token Grant
Intent: Award tokens on goal completion and play a brief token pop animation for satisfying feedback; GDD: `GDDv1.1.md#rewards`

EP-04 — Content & Localization System
Intent: Load ES/EN passages from local JSON manifest with difficulty tags and present localized UI; GDD: `GDDv1.1.md#content--localization-final`

EP-05 — Settings & Logging
Intent: Persist JSON settings and structured session logs for goals, tokens, and strict mode state; GDD: `GDDv1.1.md#settings--logs-json-locked`

EP-06 — UX/UI MVP Shell
Intent: Ship the minimal screens (Home, Passage Select, Mic Calibrate, Reading View, Reward) with simple kid-friendly UI; GDD: `GDDv1.1.md#uxui-lean`

---
Note: Per-epic files (`epic-XX.md`) are created later via the 02 prompt.
