"""Check or sync artwork from the library into the Unity project."""

import argparse
import json
import shutil
from pathlib import Path

if __package__:
    from .build_story_archive import CATALOG, PORTRAITS, RESOURCES, ROOT, story_illustration_copies
    from .sync_stories import sync as sync_stories
else:
    from build_story_archive import CATALOG, PORTRAITS, RESOURCES, ROOT, story_illustration_copies
    from sync_stories import sync as sync_stories

ASSETS = Path("assets")
IMAGE_SUFFIXES = {".png", ".jpg", ".jpeg", ".svg", ".webp"}


def library_path(folder, name):
    path = Path(name)
    if path.anchor or ".." in path.parts:
        raise ValueError(f"Nieprawidłowa ścieżka grafiki: {name}")
    return folder / path


def artwork_pairs():
    manifest = json.loads((ROOT / ASSETS / "unity-artwork.json").read_text(encoding="utf-8"))
    for source, target in manifest["folders"].items():
        source_folder = library_path(ASSETS, source)
        target_folder = library_path(RESOURCES, target)
        for image in sorted((ROOT / source_folder).iterdir()):
            if image.is_file() and image.suffix.lower() in IMAGE_SUFFIXES:
                yield source_folder / image.name, target_folder / image.name
    for source, target in manifest["files"].items():
        yield library_path(ASSETS, source), library_path(RESOURCES, target)

    portrait_map = json.loads((ROOT / PORTRAITS / "portrait-map.json").read_text(encoding="utf-8"))
    for hero, filename in portrait_map.items():
        yield library_path(PORTRAITS, filename), library_path(RESOURCES / "Portraits", f"{hero}.png")

    for _, _, _, _, _, entries in CATALOG:
        for _, _, source in entries:
            yield from story_illustration_copies(source)


def check_artwork(sync=False):
    sources = {}
    errors = []
    for source, target in artwork_pairs():
        if target in sources:
            errors.append(f"Więcej niż jedno źródło dla {target.as_posix()}")
        sources[target] = source
        if not (ROOT / source).is_file():
            errors.append(f"Brak grafiki źródłowej: {source.as_posix()}")

    for image in sorted((ROOT / RESOURCES).rglob("*")):
        if image.is_file() and image.suffix.lower() in IMAGE_SUFFIXES:
            relative = image.relative_to(ROOT)
            if relative not in sources:
                errors.append(f"Grafika Unity bez przypisanego źródła: {relative.as_posix()}")
    if errors:
        return errors

    for target, source in sources.items():
        destination = ROOT / target
        original = ROOT / source
        if destination.is_file() and destination.read_bytes() == original.read_bytes():
            continue
        if sync:
            destination.parent.mkdir(parents=True, exist_ok=True)
            shutil.copyfile(original, destination)
            print(f"Zaktualizowano {target.as_posix()}")
        else:
            errors.append(f"Kopia Unity nie odpowiada źródłu: {target.as_posix()}")
    return errors


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--sync", action="store_true", help="aktualizuj kopie Unity i dane czytnika")
    args = parser.parse_args()
    errors = check_artwork(sync=args.sync)
    if errors:
        for error in errors:
            print(error)
        return 1
    if args.sync:
        sync_stories()
    print("Grafiki w assets i kopie Unity są zgodne.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
