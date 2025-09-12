# US1-01 Design Document: Core Reading Session MVP

## Purpose

This design document outlines the implementation approach for US1-01, which establishes the core reading session functionality for the Reading Rewards application. The primary purpose is to create a minimal viable product (MVP) that allows children to read passages aloud, tracks their voice activity, and rewards them with tokens upon reaching session goals.

**User Story (Inferred)**: As a child user, I want to start a reading session where my voice is detected while reading passages so that I can earn tokens when I complete my reading goal.

## Scope

### In Scope
- Voice Activity Detection (VAD) system for tracking when child is speaking
- Timer system that counts seconds of voice activity toward session goals
- Basic reading view with passage display
- Token reward system triggered by meeting session goals
- Simple session flow: start → read → complete → reward
- JSON-based configuration and logging
- Support for the default session goal (5 minutes) and reward (1 token)

### Out of Scope  
- Caret/finger tracking functionality (deferred to later iterations)
- Strict mode implementation (voice + caret requirement)
- Multi-language content switching (basic EN support only)
- Advanced audio processing (spectral flatness, complex noise filtering)
- Parent controls and settings UI
- Token spending/redemption system

## Data Flow

```
1. Session Start
   ├── Load settings.json (sessionGoalMinutes, rewardTokensPerGoal)
   ├── Initialize microphone and VAD system
   ├── Load selected passage from Content/manifest.json
   └── Begin audio monitoring

2. Active Reading Loop
   ├── Capture audio frames (10-30ms intervals)
   ├── Calculate RMS energy per frame
   ├── Apply hysteresis thresholds for voice detection
   ├── Accumulate voice-active seconds toward goal
   └── Update UI with progress indication

3. Goal Achievement
   ├── Check if voice seconds >= sessionGoalMinutes * 60
   ├── Award rewardTokensPerGoal tokens
   ├── Update logs.json with session data
   ├── Display reward animation
   └── Return to home screen

4. Data Persistence
   ├── Append session record to logs.json
   ├── Update tokenTotal in logs.json
   └── Maintain session history for tracking
```

## Key Classes

### AudioManager
**Responsibility**: Manages microphone input and VAD processing
- `InitializeMicrophone()`: Setup audio capture
- `ProcessAudioFrame(float[] samples)`: Calculate RMS energy
- `IsVoiceDetected()`: Apply thresholds and hysteresis
- `GetAmbientFloor()`: Calibrate noise baseline

### SessionController  
**Responsibility**: Orchestrates reading session flow and state
- `StartSession(PassageData passage)`: Initialize session
- `UpdateSession()`: Process audio and track progress  
- `CompleteSession()`: Handle goal achievement and rewards
- `EndSession()`: Cleanup and save data

### PassageLoader
**Responsibility**: Handles content loading and management
- `LoadManifest()`: Parse Content/manifest.json
- `GetPassagesByDifficulty(string difficulty)`: Filter content
- `LoadPassage(string passageId)`: Load specific passage JSON

### ProgressTracker
**Responsibility**: Tracks session progress and goal achievement
- `AddVoiceSeconds(float seconds)`: Accumulate progress
- `IsGoalMet()`: Check completion status
- `GetProgressPercentage()`: UI progress indication

### RewardSystem
**Responsibility**: Manages token awards and persistence  
- `AwardTokens(int amount)`: Grant tokens for goal completion
- `GetTotalTokens()`: Retrieve current token count
- `SaveSessionLog(SessionData data)`: Persist session record

## Risks

### 1. False Positive Voice Detection
**Risk**: Background noise, music, or TV audio being detected as voice activity, leading to unearned progress.
**Impact**: High - undermines the core value proposition of reading practice tracking.
**Mitigation**: 
- Implement ambient noise floor calibration at session start
- Add 600ms debounce to avoid brief noise spikes
- Use simple RMS energy thresholds with hysteresis (different enter/exit levels)
- Consider basic spectral flatness check in future iteration

### 2. Unity Microphone API Reliability
**Risk**: Microphone permissions, initialization failures, or platform-specific audio issues.
**Impact**: High - core functionality completely blocked.
**Mitigation**:
- Robust error handling with user-friendly error messages
- Fallback graceful degradation (manual timer mode for testing)
- Platform-specific testing on Android, iOS, and Windows
- Clear permission request flows

### 3. Performance Impact on Target Devices
**Risk**: Continuous audio processing consuming excessive CPU/battery on lower-end tablets.
**Impact**: Medium - poor user experience, device heating, battery drain.
**Mitigation**:
- Target <3% CPU usage for VAD processing
- Zero allocations per audio frame
- Efficient circular buffer for audio samples
- Frame rate monitoring and adaptive quality

## Decisions

### Audio Processing Approach
**Decision**: Use simple RMS energy-based VAD with hysteresis thresholds
**Rationale**: Balances accuracy with performance requirements. More sophisticated approaches (spectral analysis, ML-based VAD) add complexity and CPU overhead inappropriate for MVP.
**Alternatives Considered**: Unity's built-in audio analysis, third-party VAD libraries
**Trade-offs**: Lower accuracy vs. simplicity and performance

### Data Storage Format  
**Decision**: JSON files for all configuration and logging
**Rationale**: Human-readable, easily debuggable, no database dependencies, aligns with offline-first approach
**Alternatives Considered**: SQLite, binary formats, Unity PlayerPrefs
**Trade-offs**: Less efficient storage vs. simplicity and transparency

### Session Goal Tracking
**Decision**: Count only voice-active seconds toward session goals
**Rationale**: Ensures children are actually attempting to read rather than just sitting silently
**Alternatives Considered**: Total elapsed time, word count estimation
**Trade-offs**: Requires reliable VAD vs. simpler time tracking

### Unity Version Strategy
**Decision**: Target Unity latest LTS with latest beta fallback
**Rationale**: Balance stability (LTS) with access to newest features (beta) for microphone APIs
**Alternatives Considered**: Stick to LTS only, use latest beta primarily
**Trade-offs**: Potential beta instability vs. feature access

## Open Questions

1. **VAD Calibration UX**: How long should the ambient noise calibration period be, and what visual feedback should guide users during this process?

2. **Goal Achievement Feedback**: Beyond the simple token pop animation, what additional feedback (audio, haptic, visual) would enhance the reward experience without being distracting?

3. **Session Recovery**: How should the app handle interrupted sessions (app backgrounded, phone call, etc.)? Should partial progress be saved or discarded?

4. **Content Progression**: Should the app suggest next passages based on completion history, or leave selection entirely to the user/parent?

5. **Accessibility**: What accommodations are needed for children with hearing difficulties or speech differences that might affect VAD accuracy?

---

**Document Status**: Draft v1.0  
**GDD Trace**: GDDv1.1.md (Core Loops, VAD Mechanics, Rewards sections)  
**Dependencies**: None  
**Last Updated**: 2025-01-27