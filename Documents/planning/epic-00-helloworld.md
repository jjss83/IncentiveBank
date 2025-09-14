# EP-00 HelloWorld Platform Validation

**Narrative**
Validate basic Unity app launch and splash screen display on Android, iPad, and Windows.

**Business Value**
Ensures build pipeline and device compatibility before deeper feature work.

**Status**
Active

---

## User Story

### US-000 (Epic: EP-00)
As a developer, I want to see a splash screen on all target platforms so that I know the build and launch process works.

**Acceptance Criteria**
- App launches and displays splash screen on Android, iPad, Windows
- No errors or crashes on startup
- Splash screen visible for at least 1 second
- Build instructions documented for each platform

**Status**
Ready

---

## Implementation Tasks

### TK-0000 US-000 design doc (Story: US-000, Type: Design, Est: XS)
**Outcome**
Design doc describing splash screen implementation and platform build steps.
**GDD Trace**
GDDv1.1.md#platforms--tech
**Dependencies**
None
**Acceptance Criteria**
- File `Documents/design/US-000-design.md` created
- Sections: Purpose, Platforms, Build Steps, Risks, Decisions
- At least 1 risk + 1 open question captured

### TK-0001 Splash screen prefab created (Story: US-000, Type: CreateAsset, Est: XS)
**Outcome**
Unity prefab for splash screen visual.
**GDD Trace**
GDDv1.1.md#uxui-lean
**Dependencies**
TK-0000
**Acceptance Criteria**
- File `Assets/Prefabs/SplashScreen.prefab` exists
- Visual matches kid-friendly theme
- No console errors on instantiation

### TK-0002 Platform build validation (Story: US-000, Type: Test, Est: XS)
**Outcome**
App builds and launches with splash screen on Android, iPad, Windows.
**GDD Trace**
GDDv1.1.md#platforms--tech
**Dependencies**
TK-0000, TK-0001
**Acceptance Criteria**
- Build instructions documented for each platform
- Splash screen visible for at least 1 second
- No startup errors or crashes
- Screenshots captured for each platform
