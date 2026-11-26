"""Generate the offline HTML timeline data from canon events and story sections."""

import json
import re
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
WEB_DATA = ROOT / "riftoryn-unity" / "Web" / "timeline-data.js"
WORLD_HISTORY = ROOT / "world_history.md"

manifest = json.loads((ROOT / "opowiadania" / "katalog.json").read_text(encoding="utf-8"))
story_by_folder = {
    story["folder"]: (hero, story)
    for hero in manifest
    for story in hero["stories"]
}


def era_sort(date_text):
    date_text = date_text.lower()
    cross_year = re.search(r"(\d+)\s*/\s*(\d+)\s+(pt|et)\b", date_text)
    era_match = re.search(r"(pt|et)", date_text)
    era = era_match.group(1).upper() if era_match else "ET"
    range_match = re.search(r"(\d+)\s*[–-]\s*(\d+)\s+(pt|et)", date_text)
    year_match = re.search(r"(\d+)\s+(pt|et)\b", date_text)
    if cross_year:
        year = int(cross_year.group(1))
        if any(label in date_text for label in ("koniec zimy", "schyłek zimy")):
            year = int(cross_year.group(2))
        era = cross_year.group(3).upper()
    elif range_match:
        year = int(range_match.group(1))
        era = range_match.group(3).upper()
    elif year_match:
        year = int(year_match.group(1))
        era = year_match.group(2).upper()
    else:
        year = 0

    month = 1
    day = 1
    month_match = re.search(
        r"(\d+)(?:\s*[–-]\s*\d+)?\s+(stycznia|lutego|marca|kwietnia|maja|czerwca|lipca|sierpnia|"
        r"września|października|listopada|grudnia)",
        date_text,
    )
    if month_match:
        day = int(month_match.group(1))
        month_names = {
            "stycznia": 1, "lutego": 2, "marca": 3, "kwietnia": 4, "maja": 5,
            "czerwca": 6, "lipca": 7, "sierpnia": 8, "września": 9,
            "października": 10, "listopada": 11, "grudnia": 12,
        }
        month = month_names[month_match.group(2)]
    else:
        season_months = [
            ("koniec grudnia", 12), ("koniec listopada", 11), ("koniec października", 10),
            ("koniec września", 9), ("koniec sierpnia", 8), ("koniec lipca", 7),
            ("koniec czerwca", 6), ("koniec maja", 5), ("koniec kwietnia", 4),
            ("koniec marca", 3), ("koniec zimy", 2 if cross_year else 12),
            ("schyłek zimy", 2 if cross_year else 12), ("pierwsza odwilż", 3),
            ("środek zimy", 12), ("późna jesień", 11), ("wiosna", 4),
            ("lato", 7), ("jesień", 10), ("zima", 12), ("marzec", 3),
            ("kwiecień", 4), ("maj", 5), ("czerwiec", 6),
            ("styczeń", 1), ("luty", 2), ("lipiec", 7), ("sierpień", 8),
            ("wrzesień", 9), ("październik", 10), ("listopad", 11), ("grudzień", 12),
            ("maja", 5), ("lipca", 7),
        ]
        for label, season_month in season_months:
            if label in date_text.lower():
                month = season_month
                # Winter belongs to the year before the first thaw. Sort broad
                # winter labels after scenes explicitly set in midwinter.
                day = {"środek zimy": 15, "zima": 30, "koniec zimy": 28 if cross_year else 31, "schyłek zimy": 28 if cross_year else 31}.get(label, 1)
                break

    if era == "PT":
        timeline_year = -year
    else:
        timeline_year = year
    return timeline_year, month, day


def display_parts(date_text):
    if " · " in date_text:
        date_part, location = date_text.split(" · ", 1)
    elif "·" in date_text:
        date_part, location = date_text.split("·", 1)
    else:
        date_part, location = date_text, ""
    return date_part.strip(), location.strip()


def parse_world_history():
    text = WORLD_HISTORY.read_text(encoding="utf-8")
    try:
        timeline = text.split("## Pełna oś czasu\n", 1)[1].split(
            "\n## Konflikty na przyszłość", 1
        )[0]
    except IndexError as error:
        raise ValueError("Could not find the full timeline in world_history.md") from error

    events = []
    current = None
    for line in timeline.splitlines():
        if line.startswith("### "):
            if current:
                events.append(current)
                current = None
        elif line.startswith("#### "):
            if current:
                events.append(current)
            heading = line[5:]
            parts = re.fullmatch(r"(.+?)\s+—\s+(.+)", heading)
            if not parts:
                raise ValueError(f"Unrecognized world-history timeline heading: {heading}")
            current = {
                "date": parts.group(1).strip(),
                "title": parts.group(2).strip(),
                "paragraphs": [],
            }
        elif current and line.strip():
            paragraph = re.sub(r"\[([^\]]+)\]\([^)]+\)", r"\1", line.strip())
            paragraph = paragraph.replace("**", "")
            current["paragraphs"].append(paragraph)
        elif current and current["paragraphs"] and not line.strip():
            current["paragraphs"].append("")

    if current:
        events.append(current)
    if not events:
        raise ValueError("No dated events found in world_history.md")
    return events


hero_names = {}
for hero in manifest:
    hero_names[hero["name"]] = hero["id"]

extra_names = {
    "Ruvan": "ruvan",
    "Mira": "mira",
    "Ilda": "ilda",
    "Heda": "heda",
    "Kela": "kela",
    "Baret": "baret",
    "Krzemień": "krzemien",
    "Kora": "kora",
    "Jorek": "jorek",
}
name_to_id = {**hero_names, **extra_names}

codex_portraits = json.loads(
    (ROOT / "riftoryn-unity" / "Assets" / "Resources" / "codex.json").read_text(encoding="utf-8")
)["heroes"]
portraits = json.loads(
    (ROOT / "assets" / "heroes" / "portraits" / "portrait-map.json").read_text(encoding="utf-8")
)


def story_link(folder, heading, chapter_index):
    mapped = story_by_folder.get(folder)
    if not mapped:
        return None
    hero, story = mapped
    return f"#{hero['id']}/{story['id']}/rozdzial-{chapter_index}"


def render():
    events = []
    for file_path in sorted((ROOT / "opowiadania").glob("*/*/historia.pl.md")):
        folder = file_path.parent.as_posix().replace(str(ROOT).replace("\\", "/") + "/", "")
        if folder not in story_by_folder:
            continue
        hero, story = story_by_folder[folder]
        text = file_path.read_text(encoding="utf-8")
        pieces = re.split(r"(?m)^## (.+)$", text)
        chapter_index = 0
        for heading, body in zip(pieces[1::2], pieces[2::2]):
            chapter_index += 1
            lines = body.strip().splitlines()
            date_text = next((line.strip() for line in lines if re.search(r"\b(?:PT|ET)\b", line)), None)
            if not date_text:
                continue
            date_part, location = display_parts(date_text)

            mentioned = [hero["id"]]
            for name, hero_id in name_to_id.items():
                pattern = r"(?<![A-Za-zĘÓĄŚŁŻŹĆŃęóąśłżźćń])" + re.escape(name) + r"(?![A-Za-zĘÓĄŚŁŻŹĆŃęóąśłżźćń])"
                if re.search(pattern, body, re.I):
                    mentioned.append(hero_id)
            mentioned = list(dict.fromkeys(mentioned))

            era_match = re.search(r"(PT|ET)", date_text)
            era = era_match.group(1) if era_match else "ET"
            cross_year = re.search(r"\d+\s*/\s*\d+\s+(?:PT|ET)\b", date_text)
            year_match = re.search(r"(\d+)\s+(PT|ET)\b", date_text)
            year = cross_year.group(0) if cross_year else (year_match.group(1) + " " + year_match.group(2) if year_match else era)
            events.append({
                "kind": "story",
                "date": date_text,
                "datePart": date_part,
                "location": location,
                "year": year,
                "era": era,
                "title": heading.strip(),
                "heroes": mentioned,
                "hero": hero["id"],
                "source": file_path.relative_to(ROOT).as_posix(),
                "link": story_link(folder, heading.strip(), chapter_index),
                "_sortKey": era_sort(date_text),
            })

    for history_event in parse_world_history():
        date_text = history_event["date"]
        range_match = re.search(r"\d+\s*[–-]\s*\d+\s+(PT|ET)\b", date_text)
        year_match = re.search(r"(\d+)\s+(PT|ET)\b", date_text)
        era_match = re.search(r"\b(PT|ET)\b", date_text)
        if "przed Traktatem" in date_text and not era_match:
            year = "Przed Traktatem"
            era = "PT"
            sort_key = (-0.5, 1, 1)
        elif range_match:
            year = date_text
            era = range_match.group(1)
            sort_key = era_sort(date_text)
        elif year_match and era_match:
            year = year_match.group(1) + " " + year_match.group(2)
            era = era_match.group(1)
            sort_key = era_sort(date_text)
        else:
            raise ValueError(f"Could not determine the era for world-history event: {date_text}")

        description = "\n".join(history_event["paragraphs"]).strip()
        events.append({
            "kind": "history",
            "date": date_text,
            "datePart": date_text,
            "location": "Historia świata",
            "year": year,
            "era": era,
            "title": history_event["title"],
            "description": description,
            "heroes": [],
            "source": WORLD_HISTORY.relative_to(ROOT).as_posix(),
            "sourceLink": "world_history.md",
            "link": None,
            "_sortKey": sort_key,
        })

    events.sort(key=lambda event: (*event["_sortKey"], event["kind"] != "history"))
    for event in events:
        del event["_sortKey"]

    hero_meta = []
    for entry in codex_portraits:
        portrait_file = portraits.get(entry["id"])
        portrait = f"assets/heroes/portraits/{portrait_file}" if portrait_file else None
        hero_meta.append({
            "id": entry["id"],
            "name": entry["name"],
            "role": entry["role"],
            "element": entry["element"],
            "portrait": portrait,
        })

    for entry in manifest:
        if not any(hero["id"] == entry["id"] for hero in hero_meta):
            portrait_file = portraits.get(entry["id"])
            hero_meta.append({
                "id": entry["id"],
                "name": entry["name"],
                "role": "Bohater",
                "element": "",
                "portrait": f"assets/heroes/portraits/{portrait_file}" if portrait_file else None,
            })

    content = (
        "// Generated by scripts/build_timeline_web.py.\n"
        f"const riftorynTimeline = {json.dumps(events, ensure_ascii=False)};\n"
        f"const riftorynTimelineHeroes = {json.dumps(hero_meta, ensure_ascii=False)};\n"
    )
    WEB_DATA.write_text(content, encoding="utf-8")
    return len(events)


if __name__ == "__main__":
    print(f"Generated {render()} timeline events.")
