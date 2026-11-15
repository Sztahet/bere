"""Check or sync the canonical hero portraits into Unity Resources."""

import argparse
import json
import shutil
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "assets" / "heroes" / "portraits"
UNITY = ROOT / "riftoryn-unity" / "Assets" / "Resources" / "Portraits"
MANIFEST = SOURCE / "portrait-map.json"


def check_portraits(sync=False):
    mapping = json.loads(MANIFEST.read_text(encoding="utf-8"))
    errors = []
    if not isinstance(mapping, dict) or not mapping:
        return ["Mapa portretów jest pusta lub ma nieprawidłowy format."]

    for hero, filename in mapping.items():
        if not isinstance(hero, str) or not hero.isascii() or not hero.isalnum() or hero != hero.lower():
            errors.append(f"Nieprawidłowy identyfikator bohatera: {hero!r}")
        if not isinstance(filename, str) or Path(filename).name != filename or not filename.endswith(".png"):
            errors.append(f"Nieprawidłowa nazwa pliku: {filename!r}")
    if errors:
        return errors

    if len(set(mapping.values())) != len(mapping):
        errors.append("Każdy bohater musi mieć osobny plik źródłowy.")
    missing = [filename for filename in mapping.values() if not (SOURCE / filename).is_file()]
    errors.extend(f"Brak portretu źródłowego: {name}" for name in missing)
    unmapped_sources = sorted(p.name for p in SOURCE.glob("*.png") if p.name not in mapping.values())
    unmapped_unity = sorted(p.name for p in UNITY.glob("*.png") if p.stem not in mapping)
    errors.extend(f"Portret źródłowy poza mapą: {name}" for name in unmapped_sources)
    errors.extend(f"Portret Unity poza mapą: {name}" for name in unmapped_unity)
    if errors:
        return errors

    for hero, filename in mapping.items():
        source = SOURCE / filename
        target = UNITY / f"{hero}.png"
        if not target.is_file() or source.read_bytes() != target.read_bytes():
            if sync:
                shutil.copyfile(source, target)
                print(f"Zaktualizowano {target.relative_to(ROOT)}")
            else:
                errors.append(f"Brak zgodnej kopii Unity: {hero} ({filename})")
    return errors


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--sync", action="store_true", help="kopiuj portrety źródłowe do Unity")
    args = parser.parse_args()
    errors = check_portraits(sync=args.sync)
    if errors:
        for error in errors:
            print(error)
        return 1
    print("Portrety źródłowe i kopie Unity są zgodne.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())

