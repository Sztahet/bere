"""Export the proposed seven-trait profiles without losing uncertainty markers."""
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "RIFTORYN_SPLOT_CECH_BOHATEROW_PROPOZYCJA.md"
OUTPUT = ROOT / "riftoryn-unity/Assets/Resources/hero_traits.json"
PATTERN = re.compile(r"^- \*\*(.+?) — ([1-5? ·]+)\.\*\* (.+)$", re.MULTILINE)


def profiles():
    roster = json.loads((ROOT / "riftoryn-unity/Assets/Resources/codex.json").read_text(encoding="utf-8"))["heroes"]
    ids = {hero["name"]: hero["id"] for hero in roster}
    result = []
    for name, ratings, explanation in PATTERN.findall(SOURCE.read_text(encoding="utf-8")):
        values = ratings.split(" · ")
        if name not in ids or len(values) != 7 or any(not re.fullmatch(r"[1-5]\??", value) for value in values):
            raise ValueError(f"Invalid profile: {name}: {ratings}")
        scores = [int(value[0]) for value in values]
        if sum(scores) != 21:
            raise ValueError(f"Profile does not total 21: {name}")
        result.append({"id": ids[name], "scores": scores,
                       "tentative": [value.endswith("?") for value in values],
                       "explanation": explanation})
    if len(result) != 36 or len({item["id"] for item in result}) != len(result):
        raise ValueError("Expected 36 distinct proposed profiles")
    return {"profiles": result}


if __name__ == "__main__":
    OUTPUT.write_text(json.dumps(profiles(), ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
