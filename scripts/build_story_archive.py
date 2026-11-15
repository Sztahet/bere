"""Prepare the offline HTML archive's data from the story sources."""

import json
import re
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
UNITY = Path("riftoryn-unity")
WEB_DATA = ROOT / UNITY / "Web" / "story-archive-data.js"
PORTRAITS = Path("assets/heroes/portraits")
RESOURCES = UNITY / "Assets" / "Resources"

CATALOG = [
    ("saila", "Saila", "Biały Przesmyk", "saila-cold-specialist-portrait.png", None, [
        ("ciezar-krysztalu", "Ciężar kryształu", "SailaStory"),
        ("gdy-wieza-zamilkla", "Gdy wieża zamilkła", "SailaAwakening"),
        ("to-czego-nie-nauczyl-mnie-ruvan", "To, czego nie nauczył mnie Ruvan", "SailaFinale"),
    ]),
    ("adrien", "Adrien", "Aurel", "adrien-light-specialist-v4.png", None, [
        ("swiatlo-ktore-wskazalo-droge", "Światło, które wskazało drogę", "AdrienStory1"),
        ("kobieta-ktora-wiedziala", "Kobieta, która wiedziała", "AdrienStory2"),
        ("wynalazek-ktorego-nie-przewidzialem", "Wynalazek, którego nie przewidziałem", "AdrienStory3"),
    ]),
    ("lena", "Lena", "Aurel", "../../../riftoryn-unity/Assets/Resources/Portraits/lena.png", None, [
        ("ludzie-sa-ciekawsi-od-krysztalow", "Ludzie są ciekawsi od kryształów", "LenaStory1"),
        ("imiona-ktore-zostaja", "Imiona, które zostają", "LenaStory2"),
        ("znam-kogos-kto-potrafi", "Znam kogoś, kto potrafi", "LenaStory3"),
    ]),
    ("rian", "Rian", "Vareth", "../../../riftoryn-unity/Assets/Resources/Portraits/rian.png", "Cena wyboru", [
        ("jedenascie-minut", "Jedenaście minut", "RIFTORYN_RIAN_01_JEDENASCIE_MINUT.md"),
        ("drugi-olowek", "Drugi ołówek", "RIFTORYN_RIAN_02_DRUGI_OLOWEK.md"),
        ("nie-przestalam", "Nie przestałam", "RIFTORYN_RIAN_03_NIE_PRZESTALAM.md"),
    ]),
    ("lucen", "Lucen", "Aurel", "lucen-light-specialist.png", "Własna droga", [
        ("z-tego-co-ocalalo", "Z tego, co ocalało", "RIFTORYN_LUCEN_01_Z_TEGO_CO_OCALALO.md"),
        ("nauka-kontroli", "Nauka kontroli", "RIFTORYN_LUCEN_02_NAUKA_KONTROLI.md"),
        ("nie-przeciwko-tobie", "Nie przeciwko tobie", "RIFTORYN_LUCEN_03_NIE_PRZECIWKO_TOBIE.md"),
    ]),
    ("taren", "Taren", "Iglica Zerath", "taren-lightning-tank.png", "Ciężar obowiązku", [
        ("to-co-pozostalo", "To, co pozostało", "RIFTORYN_TAREN_01_TO_CO_POZOSTALO.md"),
        ("cena-lojalnosci", "Cena lojalności", "RIFTORYN_TAREN_02_CENA_LOJALNOSCI.md"),
        ("po-drugiej-stronie", "Po drugiej stronie", "RIFTORYN_TAREN_03_PO_DRUGIEJ_STRONIE.md"),
    ]),
]


def markdown_chapters(source, title):
    text = (ROOT / source).read_text(encoding="utf-8")
    pieces = re.split(r"(?m)^## (.+)$", text)
    if len(pieces) < 3 or not pieces[0].splitlines()[0].endswith(f" — {title}"):
        raise ValueError(f"Nie znaleziono tytułu lub rozdziałów w {source}")
    return [
        {
            "title": heading,
            "blocks": [{"text": (re.sub(r"\*(.*?)\*", r"\1", re.sub(r"\*\*(.*?)\*\*", r"\1", paragraph.strip()))
                                  if source.startswith("RIFTORYN_RIAN_") else paragraph.strip())}
                       for paragraph in re.split(r"\n\s*\n", body) if paragraph.strip()],
        }
        for heading, body in zip(pieces[1::2], pieces[2::2])
    ]


def load_story(slug, title, source):
    story = {"id": slug, "title": title}
    if source.endswith(".md"):
        if not source.startswith("RIFTORYN_RIAN_"):
            story["draft"] = True
        story["chapters"] = markdown_chapters(source, title)
        return story

    directory = RESOURCES / source
    story["chapters"] = json.loads((ROOT / directory / "story.json").read_text(encoding="utf-8"))["chapters"]
    for chapter in story["chapters"]:
        chapter["title"] = re.sub(r"^\d+\.\s*", "", chapter["title"])
        visible = []
        for block in chapter["blocks"]:
            if block.get("pending"):
                continue
            if block.get("image"):
                image = directory / f"{block['image']:02}.jpg"
                if not (ROOT / image).is_file():
                    raise FileNotFoundError(image)
                block["src"] = image.as_posix()
            visible.append(block)
        chapter["blocks"] = visible
    return story


def render_data():
    archive = []
    for slug, name, region, portrait, series, entries in CATALOG:
        image = PORTRAITS / portrait
        if not (ROOT / image).is_file():
            raise FileNotFoundError(image)
        archive.append({
            "id": slug,
            "name": name,
            "region": region,
            "portrait": image.as_posix(),
            "series": series,
            "stories": [load_story(*entry) for entry in entries],
        })
    roster_data = json.loads((ROOT / RESOURCES / "codex.json").read_text(encoding="utf-8"))["heroes"]
    roster = []
    for hero in roster_data:
        image = RESOURCES / "Portraits" / f"{hero['id']}.png"
        roster.append({"id": hero["id"], "name": hero["name"], "role": hero["role"],
                       "element": hero["element"], "portrait": image.as_posix() if (ROOT / image).is_file() else None})
    data = ",\n".join(json.dumps(hero, ensure_ascii=False) for hero in archive)
    return (f"// Generated by scripts/build_story_archive.py.\nconst storyArchive = [\n{data}\n];\n"
            f"const storyRoster = {json.dumps(roster, ensure_ascii=False)};\n")


if __name__ == "__main__":
    WEB_DATA.write_text(render_data(), encoding="utf-8")
