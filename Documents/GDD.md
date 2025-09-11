**Role & Output Style**

You are a **Senior Game Designer & Unity Technical Lead**. Produce a **complete Game Design Document (GDD)** for a small, offline, privacy-first reading-incentive game built with Unity. Write concise, implementation-aware design, not marketing fluff. Use clear headings, simple language, and tables where helpful. Assume **local-only MVP**, **no user management**, bilingual **Spanish/English** support.

**Vision (fixed)**
A kid reads aloud (or practices early reading sounds) for short sessions. The app detects human voice (basic on-device listening) and, optionally, tracks text-following. When a session reaches its configured goal (e.g., 5 minutes), the kid earns in-game currency/tokens that parents can map to real incentives (e.g., screen time, toys, allowance). Everything runs offline on a single device.

**Non-Negotiable Constraints (fixed)**

* Engine: **Unity (LTS)**, UI Toolkit. Platforms: Android phones & tablets (min Android 8.0, target 10+), iOS iPhone & iPad (iOS 15+), Windows PC (Win10+ with built‑in or external mic). (All confirmed for MVP)
* Offline/by-design: **no servers, no analytics**, local storage only.
* **No user accounts** for MVP.
* **Primary input**: reading time captured via **on-device audio** (human-voice presence) and touch.
* **Optional input**: text-following (finger or caret) if reading happens on-screen.
* **Languages**: Spanish and English UI + content support.
* **Configurable incentives** (what/how/when), but **grant logic is deterministic and visible** to the parent.
* **Tiny/local models only** if any ML is used (e.g., VAD—voice activity detection).
* **Age-appropriate UX**, COPPA-friendly in spirit (no data leaves device).

**Deliverables (what to produce now)**

1. **High-level pitch + player promise**
2. **Target audience & use cases** (ages, early readers vs fluent)
3. **Core loop** (1-minute, 5-minute, session loop)
4. **Game pillars** (3–5) and how each maps to mechanics/UI
5. **MVP feature set** vs **Post-MVP** (explicit cutlines)
6. **Session flow** (state diagram + bullets)
7. **Mechanics**

   * Reading session detection (VAD thresholds, calibration, noise handling)
   * Optional text-following (touch/caret tracking if on-screen reading)
   * Progress capture (pages/paragraphs/minutes)
   * Incentive configuration (parent view) and redemption
   * Anti-abuse basics appropriate for kids (e.g., voice presence + touch cadence, simple heuristics)
8. **Content strategy**

   * On-screen texts vs physical books; how progress is recorded for each
   * Spanish/English content handling and UI locale
   * Dyslexia-friendly options (fonts/spacing/contrast)
9. **Economy design (lightweight)**

   * Earn rates, caps, cooldowns, streaks
   * Mapping “tokens → real-world reward” via parent checklist
10. **UX/UI** (wireframe-level descriptions)

    * Kid home, “Start Reading”, session timer, soft stop, reward screen
    * Parent quick-config (PIN-gated), incentive mapping, logs
11. **Audio & ML subsystem** (implementation-aware)

    * On-device microphone pipeline (permissions, calibration, privacy messaging)
    * **VAD** baseline (energy + zero-crossing + optional tiny model); false-positive handling (TV/music)
    * Latency and performance budgets
12. **Platform targets & perf targets** (fps, memory, low-end device constraints)
13. **Saving & data model** (local JSON or ScriptableObjects; sample schemas)
14. **Localization plan** (text keys, RTL not required; Spanish/English done)
15. **Accessibility** (dyslexia mode, font size, color contrast, audio feedback)
16. **Parental Controls** (PIN, device time windows, manual grant/revoke)
17. **Testing & validation**

    * Manual test checklist + acceptance criteria for MVP
    * Audio edge cases (quiet kid, room noise, TV in background)
18. **Roadmap**

    * Milestones M0–M2 with acceptance criteria
    * Risks & mitigations (tech/UX/legal-adjacent)
19. **Open questions** (clearly listed)

**Known Inputs (fill from the questionnaire below; where missing, make sensible defaults and mark with “Assumed”)**

* Platforms & devices: \Android phones & tablets (min Android 8.0, target 10+), iOS iPhone & iPad (iOS 15+), Windows PC (Win10+ with built‑in or external mic). (All confirmed for MVP
* Unity version: latest beta versions
* Reading context: ON SCREEN ONLY
* Session goal default:configurable
* Incentive types allowed: screen time, allowance, toy points
* Bilingual scope: \[Full UI + content metadata in EN]
* Offline ASR scope: \[VAD only for MVP; optional phoneme hints if on-device feasible]
* Content source: Fixed text sources to test formats can be fixed for the MVP
* Data retention: local only, do persist the points or tokens in between session, no back up necesary though
* Safety/abuse policy: [light heuristics OK; no biometric storage]

**Output Format**

* Markdown with `#` headings, tables where helpful, and a one-page **Executive Summary** first.
* Include a small **system diagram (ASCII)** for runtime flow (mic → VAD → session logic → reward).
* End with **Backlog (prioritized)** and **Open Questions**.

---

## Questionnaire (answer inline; I’ll turn it into the GDD)

1. **Platforms**: Which devices for MVP? Android tablet/phone, iPad, Windows laptop with mic
2. **Unity version**: Which LTS do you prefer? Latest beta
3. **Reading context**: Are kids reading **on-screen** (text displayed in-app), **from physical books** (app listens), or **both**? just from the screen
4. **Text-following**: If on-screen, should the kid drag a finger/caret to track progress? yes, this could be an option, but it has to verify the child is trying to read (hear human sounds), if its recognizable, then the tracking is automatic
5. **Session goal**: Default read time per reward (5 minutes?), and daily cap? (e.g., max 30 minutes/day rewarded) this all should be configurable, for now, we can use fixed json files with the configuration settings for the MVP
6. **Incentives**: Which real-world incentives do you want to model first? for the MVP tokens that can be exchange for: screen time, allowance, stickers. All configure through fixed json files for the MVP
7. **Economy rules**: How many in-game tokens/min per reading minute? Any streak bonus? Any cooldowns between sessions? not for the MVP, lets keep it simple
8. **Parent controls**: PIN lock needed? Manual override to grant/deny rewards? Export a simple local report (CSV/JSON)? not for the MVP
9. **Languages**: UI in ES/EN for MVP? Any initial content in both languages? lets make it a requirement to be able to read from a content folder, for the MVP this content folder will be fixed. The format of the content I don't know and should be an open question to investigate more. Support ES/EN
10. **Content source**: For MVP, should we ship **a tiny built-in library** (short ES/EN passages), **allow parent to paste/import text**, or **track time only** (no text)? lets make it a requirement to be able to read from a content folder, for the MVP this content folder will be fixed. The format of the content I don't know and should be an open question to investigate more.
11. **Validation strictness**: For early readers, is **“human voice present”** sufficient? yes
    For older kids, do you want optional stricter validation? Yes, then each word will auto marked as read and only advance as the child reads
12. **Noise environments**: Any typical contexts we should design for? (car rides, living room with TV, classroom) not for the MVP, but if there are ways to get these noise dealing features, let's leverage them
13. **Privacy stance**: Confirm **no cloud**, **no third-party analytics**, **no recordings saved** (only lightweight session stats). Is that correct? yes
14. **Accessibility defaults**: Enable dyslexia-friendly font/spacing toggle by default? Larger base font? no
15. **Devices constraints**: Low-end Android target RAM/CPU expectations? Any must-support devices you own? no
16. **Art/Theme**: What vibe do you want? (cozy reading nook, playful library critters, minimalist “focus” mode) kid oriented, simple clean but child friendly
17. **Reward reveal**: How should the reward be presented? (confetti + stamp book, ticket machine animation, simple modal) for now just the token count
18. **Parent setup time**: Should first-run setup be <2 minutes with presets? What presets? (Age band, 5-min goal, token mapping) pressets can be a json file fixed for now
19. **Data model**: Do you want a simple local JSON file with human-readable logs? yes
20. **Future (post-MVP)**: List 3 features you might want soon (e.g., phoneme-level feedback, gentle mispronunciation hints, book-level progress, achievements). achivements

---

## Optional “Quick-Start” version (short prompt)

> Create a complete GDD for a local-only Unity reading-incentive game where kids earn tokens after reading aloud for \[N] minutes. Use VAD-style on-device listening to confirm “human voice present.” Spanish/English UI and content. No user accounts. Simple economy: \[RULES]. Platforms: \[PLATFORMS]. Reading context: \[ON-SCREEN/PHYSICAL/BOTH]. Include: core loop, MVP scope, session flow, audio pipeline, economy, parent controls, accessibility, data model (local JSON), acceptance criteria, risks/mitigations, milestone roadmap, prioritized backlog, and open questions. Format as clear Markdown with tables and a one-page executive summary.

---

### Notes & tips (so the GDD is actionable)

* **Keep ML tiny**: Start with energy-based VAD + basic heuristics; add a micro-model later only if needed.
* **Anti-abuse for kids**: Gentle, not punitive—e.g., require touch/caret movement for 30–60 seconds in on-screen mode or random “point to the sun icon” prompts.
* **Design for noisy homes**: Quick mic calibration step + “quiet please” coach marks.
* **Economy transparency**: Parents see exactly how minutes → tokens → reward.
* **Accessibility first**: Dyslexia toggle, larger font defaults, high contrast, minimal animations during reading.

---

Reply with your questionnaire answers (even rough is fine). I’ll immediately turn them into a tight first-draft GDD you can start building against.
