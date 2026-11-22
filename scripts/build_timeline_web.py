"""Generate the offline HTML timeline data from dated story sections."""

import json
import re
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
WEB_DATA = ROOT / "riftoryn-unity" / "Web" / "timeline-data.js"

manifest = json.loads((ROOT / "opowiadania" / "katalog.json").read_text(encoding="utf-8"))
story_by_folder = {
    story["folder"]: (hero, story)
    for hero in manifest
    for story in hero["stories"]
}


def era_sort(date_text):
    era_match = re.search(r"(PT|ET)", date_text)
    era = era_match.group(1) if era_match else "ET"
    range_match = re.search(r"(\d+)\s*[–-]\s*(\d+)\s+(PT|ET)", date_text)
    year_match = re.search(r"(\d+)\s+(PT|ET)\b", date_text)
    if range_match:
        year = int(range_match.group(2))
        era = range_match.group(3)
    elif year_match:
        year = int(year_match.group(1))
        era = year_match.group(2)
    else:
        year = 0

    month = 1
    day = 1
    month_match = re.search(
        r"(\d+)\s+(stycznia|lutego|marca|kwietnia|maja|czerwca|lipca|sierpnia|"
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
            ("koniec marca", 3), ("koniec zimy", 2), ("pierwsza odwilż", 3),
            ("środek zimy", 1), ("późna jesień", 11), ("wiosna", 4),
            ("lato", 7), ("jesień", 10), ("zima", 1), ("marzec", 3),
            ("kwiecień", 4), ("maj", 5), ("czerwiec", 6),
        ]
        for label, season_month in season_months:
            if label in date_text.lower():
                month = season_month
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
            year_match = re.search(r"(\d+)\s+(PT|ET)\b", date_text)
            year = year_match.group(1) + " " + year_match.group(2) if year_match else era
            events.append({
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
            })

    events.sort(key=lambda event: era_sort(event["date"]))
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
