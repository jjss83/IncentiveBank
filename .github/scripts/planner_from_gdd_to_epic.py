import argparse
import os
import re
from pathlib import Path


SECTION_ORDER = [
    ("## Workflow Overview", "Workflow Overview"),
    ("## Epic Narrative", "Epic Narrative"),
    ("## Business Value", "Business Value"),
    ("## Goals & Non-Goals", "Goals & Non-Goals"),
    ("## Acceptance Criteria", "Acceptance Criteria"),
    ("## Technical Design", "Technical Design"),
    ("### Architecture Sketch", "Architecture Sketch"),
    ("### CI/CD Notes", "CI/CD Notes"),
    ("## Risks & Mitigations", "Risks & Mitigations"),
    ("## Traceability", "Traceability"),
]


def extract_front_matter_id_and_name(text: str):
    # Simple YAML front matter extraction
    # Expect lines like: id: EP-00, name: ...
    id_match = re.search(r"^id:\s*(.+)$", text, flags=re.MULTILINE)
    name_match = re.search(r"^name:\s*(.+)$", text, flags=re.MULTILINE)
    epic_id = id_match.group(1).strip() if id_match else None
    epic_name = name_match.group(1).strip() if name_match else None
    return epic_id, epic_name


def extract_section(text: str, section_heading: str) -> str:
    pattern = re.compile(rf"^{re.escape(section_heading)}\s*$", re.MULTILINE)
    m = pattern.search(text)
    if not m:
        return ""
    start = m.end()
    # Next top-level section start: lines that start with '## ' (or for ### subsections, stop at next '## ' or '### ')
    if section_heading.startswith("### "):
        next_pattern = re.compile(r"^(## |### )", re.MULTILINE)
    else:
        next_pattern = re.compile(r"^(## )", re.MULTILINE)
    next_m = next_pattern.search(text, start)
    end = next_m.start() if next_m else len(text)
    content = text[start:end].strip("\n")
    return content.strip()


def build_epic_summary(epic_path: Path) -> str:
    text = epic_path.read_text(encoding="utf-8")
    epic_id, epic_name = extract_front_matter_id_and_name(text)
    title = epic_name or epic_path.stem
    header = f"### {epic_id or epic_path.stem} — {title}\n"
    body_parts = []
    for heading, label in SECTION_ORDER:
        section = extract_section(text, heading)
        if section:
            body_parts.append(f"{label}\n{section}\n")
    return header + "\n".join(body_parts) + "\n"


def main():
    parser = argparse.ArgumentParser(description="Generate epic list summary from existing epic markdown files")
    parser.add_argument("--epics_dir", default="Documents/planning/epics", help="Directory containing epic *.md files")
    parser.add_argument("--output", default="Documents/planning/epics/_epic-list.md", help="Output markdown file path")
    args = parser.parse_args()

    epics_dir = Path(args.epics_dir)
    out_path = Path(args.output)
    out_path.parent.mkdir(parents=True, exist_ok=True)

    epic_files = sorted([p for p in epics_dir.glob("*.md") if p.is_file() and not p.name.startswith("_")])
    if not epic_files:
        print(f"No epic files found in {epics_dir}")
        out_path.write_text("# Epic List\n\n_No epics found._\n", encoding="utf-8")
        return

    parts = ["---\ngenerated_at: PLACEHOLDER\nsource: epics_dir\n---\n\n# Epic List\n"]
    for epic_file in epic_files:
        parts.append(build_epic_summary(epic_file))

    out_path.write_text("\n\n".join(parts), encoding="utf-8")
    print(f"Wrote epic list to {out_path}")


if __name__ == "__main__":
    main()
