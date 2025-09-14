import argparse
import re
from pathlib import Path


def read_epic(epics_dir: Path, epic_selector: str) -> Path:
    # epic_selector can be an ID like EP-00 or a filename like epic-00.md
    candidate_by_id = None
    if epic_selector.upper().startswith("EP-"):
        # find file containing `id: EP-XX` in front matter
        for p in epics_dir.glob("*.md"):
            text = p.read_text(encoding="utf-8", errors="ignore")
            if re.search(rf"^id:\s*{re.escape(epic_selector)}\s*$", text, re.MULTILINE):
                candidate_by_id = p
                break
    if candidate_by_id:
        return candidate_by_id

    # fallback: try direct match by filename
    p = epics_dir / epic_selector
    if p.exists():
        return p
    # try normalized name like epic-00.md from EP-00
    norm = epic_selector.lower().replace("ep-", "epic-") + (".md" if not epic_selector.lower().endswith(".md") else "")
    p = epics_dir / norm
    if p.exists():
        return p
    raise FileNotFoundError(f"Epic not found for selector: {epic_selector}")


def main():
    parser = argparse.ArgumentParser(description="Unfold a single epic file into a standalone artifact")
    parser.add_argument("--epics_dir", default="Documents/planning/epics", help="Directory containing epic *.md files")
    parser.add_argument("--epic", required=True, help="Epic selector: EP-XX or filename (e.g., epic-00.md)")
    parser.add_argument("--output", default="Documents/planning/epics/_selected-epic.md", help="Output markdown file path")
    args = parser.parse_args()

    epics_dir = Path(args.epics_dir)
    out_path = Path(args.output)
    out_path.parent.mkdir(parents=True, exist_ok=True)

    epic_file = read_epic(epics_dir, args.epic)
    content = epic_file.read_text(encoding="utf-8")
    out_path.write_text(content, encoding="utf-8")
    print(f"Wrote selected epic to {out_path}")


if __name__ == "__main__":
    main()
