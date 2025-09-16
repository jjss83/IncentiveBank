# Developer Guide Pack for IncentiveBank

Welcome to the **IncentiveBank developer guide**.  These lightweight documents
help you build features for the Reading Rewards app efficiently and safely.

This pack favours a **strategy‑before‑code** approach:

* Begin with a clear **feature strategy** using the template provided (see
  `feature‑strategy‑template.md`).  Identify the problem, goals, constraints,
  acceptance criteria and a minimal vertical slice before you write any code.
* Break the strategy into **small slices** (`feature‑slice‑checklist.md`) that
  ship measurable value or testable results in isolation.  Small slices keep
  momentum and reduce risk.
* Use the **Unity basics guide** (`unity‑dev‑basics.md`) to stay aligned with
  proven Unity practices: understand the lifecycle of `Awake`, `Start`,
  `Update`, `FixedUpdate` and `LateUpdate`【318299573780667†L46-L100】, avoid
  unnecessary allocations【770200127517993†L135-L137】, and leverage
  ScriptableObjects for data【851037607720067†L84-L122】.
* When using GitHub Copilot or other LLMs, follow the **Copilot playbook**
  (`copilot‑playbook.md`) to craft thoughtful prompts, ask for options and
  trade‑offs, and validate the AI’s output【785259186302324†L340-L411】.
* Ensure every change includes tests.  The **testing guide**
  (`testing‑in‑unity.md`) explains Unity’s Test Framework and shows how to
  decide between Edit‑mode and Play‑mode tests【504304711547036†L21-L71】.

For more on planning epics, stories and tasks, see the interactive prompt
files under `.github/prompts/` and the brief overview in
`github‑prompts‑overview.md`.

If you’re unsure where to start: read the **Unity basics** guide first,
then draft your **feature strategy** and review it with your team before
writing code.  Each document cross‑links to the others, so feel free to
browse as needed.
