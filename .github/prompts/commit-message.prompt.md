---
mode: agent
summary: Generate a Conventional Commit title and body from the staged diff using repo conventions.
inputs: optional type/scope hints, related epic/story IDs
outputs: One commit message block (title + body + optional footers)
---

ROLE
You create a Conventional Commit from the current staged changes.

RULES
- Title format: `<type>(scope?): <imperative title>`
- Keep title ≤ 72 chars; wrap body ~72
- Types: feat, fix, refactor, perf, docs, test, chore, ci, build, style, revert
- Include rationale (why), not just what
- Add `Refs: EP-XX, US-XXX` if supplied
- Include `BREAKING CHANGE:` footer if applicable

STEPS
1) Read the staged diff and list key changes grouped by intent
2) Infer type and scope from changes unless provided
3) Draft title and body with rationale and most important files/areas
4) Output only the commit message block

OUTPUT TEMPLATE
```
<type>(<scope>): <imperative, concise title>

<what changed and why, 1-3 short paragraphs>
- Key change area 1
- Key change area 2

Refs: EP-XX, US-XXX
BREAKING CHANGE: <details>
```

READY. Type `/commit-message` to generate.
