# Code Review Checklist

Use this checklist to keep reviews focused and consistent.  Aim for
constructive, collaborative feedback that improves the code and the
implementation plan.

## Mindset

* **Keep it simple**.  Prefer straightforward solutions over clever
  abstractions.  Ask: can this be clearer?
* **Respect the plan**.  Check that the change aligns with the feature
  strategy and slice definition.  If not, discuss before merging.
* **Assume good intent**.  Focus on learning and quality, not blame.

## Checklist items

1. **Does it solve the problem?**  Confirm that the code addresses the
   stated goal and satisfies the acceptance criteria from the strategy.
2. **Lifecycle correctness**【318299573780667†L84-L100】.  Are Unity event
   functions used appropriately (e.g. physics in `FixedUpdate`, UI in
   `Update`)?  Is initialisation in `Awake`/`Start` and cleanup in
   `OnDestroy`?
3. **Memory allocations**【770200127517993†L135-L137】.  Look for `new`
   allocations in per‑frame methods; suggest pooling or caching.
4. **Composition vs inheritance**.  Are responsibilities broken into
   reusable components?  Would a ScriptableObject be better for shared
   data【851037607720067†L84-L122】?
5. **Coupling and cohesion**.  Are modules loosely coupled and cohesive?
   Avoid long chains of dependencies; favour DI or event broadcasting for
   decoupling.
6. **Naming and intent**.  Are class and method names descriptive?  Do
   comments explain *why* something is done when it’s not obvious?
7. **Test clarity**.  Are there corresponding tests?  Do they clearly
   express the expected behaviour?  Recommend adding tests if missing.
8. **Cross‑platform considerations**.  Does the code handle Android,
   iOS and Windows appropriately (e.g. microphone selection, platform
   checks)?
9. **AI comments and inline docs**. Non‑obvious logic includes AI‑specific
   comments (intent, constraints, edges, rationale). Public APIs have
   XML docs. Inline docs are up‑to‑date with implementation.
10. **Todo workflow**. PR description or inline section shows a small todo
    list; all items are completed or explicitly deferred.
11. **Consistency pass**. Naming, patterns, and folder/asset organization
    align with surrounding code and `ClassName.ai.md`. Deviations are justified.
12. **Docs and references**. Any external docs or strategy links are referenced
    in code comments where relevant.

## Final checks

* Run the project to ensure there are no console errors or new warnings.
* Verify that assets and scenes open cleanly in the editor.
* Confirm that the branch and commit names follow the conventions (see
  `commit‑and‑pr‑guide.md`).

A good review not only finds issues but also recognises good practices.
Celebrate clarity and simplicity when you see it.
