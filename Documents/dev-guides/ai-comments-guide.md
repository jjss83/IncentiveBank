AI-Specific Comments Guide
==========================

Purpose
-------
Provide patterns for writing comments that help AI agents (and humans) quickly grasp context, intent, and constraints without leaving the code. These comments are additive to normal documentation and should remain concise.

Principles
----------
- Be local: place comments beside the code they explain.
- Be structured: use consistent fields so agents can parse quickly.
- Be minimal but complete: include intent, inputs/outputs, constraints, edge cases, and rationale.
- Keep current: when code changes, update the comment in the same PR.

Comment Shapes (Choose One)
---------------------------

1) Brief header (good default)

```csharp
/// AI: Purpose: Compute daily reward cap.
/// AI: Inputs: currentPoints (int), dailyLimit (int), now (DateTime, UTC).
/// AI: Output: cappedPoints (int). Pure.
/// AI: Constraints: No allocations in hot path; log only at debug level.
/// AI: Edges: dailyLimit <= 0 => return 0; overflow-safe math.
/// AI: Rationale: Called per-frame in UI; keep O(1).
public static int CapDaily(int currentPoints, int dailyLimit, DateTime now)
{
    // ...
}
```

2) Task block (use for multi-step logic)

```csharp
/* AI
Context: Player can earn points from multiple sources in a frame.
Intent: Merge sources deterministically and cap at per-day limit.
Inputs: sources (IEnumerable<int>), limitProvider (IDailyLimit).
Output: merged (int), capped (int).
Constraints: Deterministic; stable ordering; no LINQ allocations.
Edges: Negative source => treat as 0; missing provider => default limit = 0.
Rationale: Prevent exploits from burst awarding.
*/
```

3) TODO preface (paired with the todo workflow)

```csharp
// AI-TODO: Implement cache invalidation for daily limit at UTC midnight.
// Assumptions: Server time authoritative; Unity client uses UTC.
// Done-When: New day triggers recompute without restart; covered by unit test.
```

When To Use
-----------
- Non-obvious business rules or domain invariants
- Performance-sensitive code (allocations, GC, threading)
- Asynchronous flows, event lifecycles, and Unity callbacks
- Serialization contracts and ScriptableObject data models

What To Avoid
-------------
- Restating obvious code (“increments i”)
- Drifting from reality (stale comments)
- Duplicating external docs: link them instead

Unity Examples
--------------

MonoBehaviour lifecycle intent

```csharp
/// AI: Purpose: Debounced save after settings change.
/// AI: Lifecycle: Subscribes in OnEnable, unsubscribes in OnDisable.
/// AI: Constraints: No writes in Update; writes only on idle > 500ms.
public class SettingsSaver : MonoBehaviour
{
    // ...
}
```

ScriptableObject data contract

```csharp
/// AI: Contract: Designer-defined reward tiers. Immutable at runtime.
/// AI: Constraints: GUID-stable; do not rename fields without migration.
[CreateAssetMenu(menuName = "Rewards/TierConfig")]
public class TierConfig : ScriptableObject
{
    public string tierId; // Stable ID used in save files
    public int pointsRequired;
}
```

Checklist
---------
- Comment is adjacent to the code
- Intent and constraints are stated
- Inputs/outputs and edge cases are listed
- Rationale is included for non-obvious choices
- Links to external references (if any) are present
