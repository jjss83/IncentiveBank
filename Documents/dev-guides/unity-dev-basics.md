# Unity Development Basics

This guide distills proven practices for building Unity features in
IncentiveBank.  It emphasises simplicity, composition and memory hygiene so
that you can focus on delivering value to players.

## Lifecycle essentials

Unity executes scripts in a defined order.  Understanding this order helps
you keep behaviour predictable:

* **Awake** and **OnEnable** run once when a script instance is loaded.
  `Awake` is called for all active scripts before any `Start` methods are
  invoked【318299573780667†L46-L68】.  Use `Awake` to initialise internal
  references.  Use `Start` to set up interactions with other components that
  may require those references.
* **Update** is called **once per frame** and is the main workhorse for
  gameplay logic【318299573780667†L92-L100】.  Use `Update` for
  input handling, timers, UI updates and non‑physics logic.
* **FixedUpdate** is called on a **fixed timestep** and may be invoked
  multiple times per frame or not at all【318299573780667†L84-L100】.  It is
  aligned with the physics simulation.  Put `Rigidbody` movements and other
  physics‑related operations here and **do not multiply by
  `Time.deltaTime`** – the fixed timestep already accounts for elapsed time.
* **LateUpdate** runs after `Update` and `FixedUpdate`【318299573780667†L92-L100】.
  Use it for camera adjustments or logic that depends on everything else being
  updated.

### Coroutines and async

Coroutines execute after `Update` and before rendering.  They are useful for
timed sequences (e.g. a reward animation) but should be avoided for
performance‑critical loops.  For asynchronous file I/O or web requests, use
`async`/`await` with `Task` in Unity’s newer versions; however, avoid mixing
coroutines and async operations in the same flow.

## Memory and garbage collection

C#’s automatic garbage collector makes coding easy but can introduce
performance spikes.  To avoid stutters, **minimise per‑frame allocations**;
ideally allocate **zero bytes per frame**【770200127517993†L135-L137】.  Some
tips:

* Avoid `new` within `Update` and `FixedUpdate`.  Reuse lists and arrays;
  clear them instead of creating new ones.
* Prefer `foreach` loops in Edit‑mode only; in play code, index your lists
  explicitly to avoid hidden allocations.
* Pool objects that are created and destroyed frequently (e.g. VAD audio
  buffers) instead of instantiating/destroying them each time.

Profiling with the **Unity Profiler** can help identify allocations and
performance hotspots.  It gathers CPU, memory, renderer and audio data and
allows you to connect to devices to test on target platforms【980320426449486†L88-L110】.

## Composition over inheritance

Unity favours composition: attach multiple small `MonoBehaviour` components
to a GameObject rather than building deep inheritance hierarchies.  Reserve
inheritance for shared behaviour where base classes truly add value.  If you
have data that multiple objects share, use a **ScriptableObject**:

* ScriptableObjects derive from `UnityEngine.Object` and are assets, not
  components【851037607720067†L84-L122】.  They store data separately from
  GameObjects and can be referenced by many objects to avoid duplicating
  values.  For example, settings for the VAD thresholds or reward values
  could live in a ScriptableObject so they can be tuned in one place.

## Scene and prefab discipline

Maintain clear boundaries between scenes and prefabs:

* **Bootstrap** scene loads other scenes.  Keep it minimal (e.g. a loader
  script that reads settings and logs the platform).
* Use prefabs for reusable UI elements (e.g. reading view, reward pop).  Make
  prefabs self‑contained with their own scripts.
* When referencing other prefabs, use the `Addressables` system so you can
  load assets asynchronously and avoid bundling everything at launch.  Assign
  addressable labels and group assets logically (e.g. `UI`, `Audio`,
  `Localization`).

## Platform checks and cross‑platform audio

IncentiveBank targets Android, iOS (iPad) and Windows.  Use
`Application.platform` to branch behaviour only when necessary (e.g. choose
the correct microphone device or handle platform‑specific permission
requests).  When using the **Microphone API**, enumerate devices with
`Microphone.devices` and query their capabilities with `GetDeviceCaps`.

## Voice activity detection (VAD) guidelines

The app’s VAD must be robust and efficient.  Instead of inventing new
algorithms, follow **proven patterns**:

1. **Calibrate the ambient floor** at startup by sampling a few seconds of
   microphone input and computing its RMS energy.
2. Compute the **RMS energy** of incoming audio frames (10–30 ms) and use
   hysteresis thresholds with a debounce (e.g., 600 ms) to avoid blips.
3. Optionally compute **spectral flatness** to suppress music/TV noise.
4. Count only the **seconds of voice** toward the session goal; pause during
   silence.  In strict mode, combine voice detection with caret movement.

Avoid per‑frame allocations in your VAD code.  Pre‑allocate audio buffers
and reuse them; do not call `Microphone.Start` repeatedly during a session.

## Profiling first steps

To profile your feature:

1. Open **Window → Analysis → Profiler**.  Attach it to a device if you’re
   testing on mobile.【980320426449486†L88-L110】.
2. Record a session while performing the reading loop.  Look for CPU time
   consumed by your scripts and memory allocations.
3. Investigate spikes; ensure audio processing stays within budget and there
   are no unexpected allocations.  Use the Audio Profiler module to inspect
   microphone input overhead.

Following these basics will help you build features that are robust across
platforms and simple to maintain.  For more project‑specific constraints,
review the **feature strategy template** and **slice checklist**.
