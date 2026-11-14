"""Build Unity story resources from the Polish Adrien manuscripts."""
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RESOURCES = ROOT / "riftoryn-unity" / "Assets" / "Resources"
STORIES = [
    ("RIFTORYN_ADRIEN_01_SWIATLO_KTORE_WSKAZALO_DROGE.md", "AdrienStory1", [
        ("Dwie wąskie linie przecięły kurz.", "Adrien sprawdza sygnalizator na placu."),
        ("Wcześniej zajmowałem się piecami grzewczymi.", "Adrien pracuje nad piecem zasilanym kryształem."),
        ("Wskazała paznokciem mocowanie kryształu.", "Młoda Lena ostrzega Adriena o mocowaniu."),
        ("Właśnie dowiedziałem się, że mój sygnalizator zawiódł.", "Adrien otrzymuje wiadomość o awarii."),
        ("Urządzenie leżało przy prowizorycznym stanowisku.", "Adrien bada uszkodzony sygnalizator przy drodze."),
        ("W raporcie opisałem usterkę.", "Adrien przygotowuje raport po awarii."),
        ("Przez następne dwie godziny sprawdzaliśmy obciążenia", "Lena i Adrien sprawdzają dwa mocowania."),
        ("Wpisałem imię przewodnika z raportu.", "Adrien wpisuje imię przewodnika do notatnika."),
    ]),
    ("RIFTORYN_ADRIEN_02_KOBIETA_KTORA_WIEDZIALA.md", "AdrienStory2", [
        ("Rian siedziała po drugiej stronie stołu.", "Rian składa zeznania przed komisją."),
        ("Jedenastu minut nie zapisałem od razu.", "Adrien słucha zeznań Rian, szkicując mechanizm."),
        ("Rozłożyłem na stole własny szkic zabezpieczenia.", "Adrien pokazuje Rian szkic podczas pierwszej rozmowy."),
        ("Wniosek pisałem trzy razy.", "Adrien przygotowuje wniosek dla komisji."),
        ("Pierwszą próbę przeprowadziliśmy na odłączonym modelu.", "Rian sprawdza model pod nadzorem."),
        ("Kiedy Lucen ładował pierwszy kryształ", "Młody Lucen ładuje kryształ w warsztacie Adriena."),
        ("Poprosiłem dzisiaj o Lucena.", "Lucen i Adrien wspólnie badają konstrukcję."),
        ("Rian po odbyciu kary zaczęła uczyć", "Rian uczy młodych rzemieślników."),
    ]),
    ("RIFTORYN_ADRIEN_03_WYNALAZEK_KTOREGO_NIE_PRZEWIDZIALEM.md", "AdrienStory3", [
        ("Miko przyniósł mi urządzenie zawinięte w płaszcz", "Miko przynosi Adrienowi swój prototyp."),
        ("Wróciłem do jego notatek.", "Adrien i Miko porównują zapisy prób."),
        ("Przy piątym uruchomieniu wynik", "Adrien i Miko testują regulator w warsztacie."),
        ("Przedstawiłem jej wyniki i historię", "Rian analizuje regulatory Adriena i Miko."),
        ("Zauważyłem jednak, że wskazówka", "Adrien i Rian sprawdzają przyczynę odchylenia."),
        ("Na odwrocie listu naszkicowałem", "Adrien szkicuje urządzenia dla areny."),
        ("Pierwszy prototyp urządzenia do zakłócania wież działa.", "Adrien i Lena sprawdzają prototyp w pracowni."),
        ("Poprosiłem też Lenę o dodatkową kontrolę projektu.", "Adrien wraca do projektu i zapisuje plan prób."),
    ]),
]


def build(source, folder, slots):
    text = (ROOT / source).read_text(encoding="utf-8")
    out = RESOURCES / folder / "story.json"
    previous = {}
    if out.exists():
        existing = json.loads(out.read_text(encoding="utf-8"))
        previous = {block["image"]: block for chapter in existing["chapters"]
                    for block in chapter["blocks"] if "image" in block}
    pieces = re.split(r"(?m)^## Rozdział [IVX]+ — (.+)$", text)
    if len(pieces) < 3 or len(pieces) % 2 != 1:
        raise ValueError(f"Nieprawidłowe rozdziały: {source}")
    preface = [p.strip().strip("*") for p in re.split(r"\n\s*\n", pieces[0]) if p.startswith("*Daty sprzed")]
    chapters, found = [], set()
    for index in range(1, len(pieces), 2):
        chapter = {"title": f"{(index + 1) // 2}. {pieces[index]}", "blocks": []}
        paragraphs = [p.strip() for p in re.split(r"\n\s*\n", pieces[index + 1]) if p.strip()]
        if not chapters:
            paragraphs = preface + paragraphs
        for paragraph in paragraphs:
            paragraph = re.sub(r"\*\*(.*?)\*\*", r"\1", paragraph)
            paragraph = re.sub(r"\*(.*?)\*", r"\1", paragraph)
            chapter["blocks"].append({"text": paragraph})
            for number, (anchor, alt) in enumerate(slots, 1):
                if anchor in paragraph:
                    if number in found:
                        raise ValueError(f"Powtórzony punkt {number}: {source}")
                    chapter["blocks"].append(previous.get(number, {"image": number, "alt": alt, "pending": True}))
                    found.add(number)
        chapters.append(chapter)
    if found != set(range(1, len(slots) + 1)):
        raise ValueError(f"Brak punktów {set(range(1, len(slots) + 1)) - found}: {source}")
    out.parent.mkdir(parents=True, exist_ok=True)
    out.write_text(json.dumps({"chapters": chapters}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")


if __name__ == "__main__":
    for entry in STORIES:
        build(*entry)
        print(f"Utworzono {entry[1]}: {len(entry[2])} miejsc na ilustracje")
