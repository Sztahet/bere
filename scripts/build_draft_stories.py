"""Generate Unity draft stories from the same manuscripts used by the HTML archive."""
import json
import uuid
from pathlib import Path

from build_story_archive import CATALOG, ROOT, RESOURCES, load_story

HEROES = {"nocten": "Nocten", "piecarius": "Piecarius", "morrak": "Morrak"}


def generate():
    for hero_id, _, _, _, _, entries in CATALOG:
        if hero_id not in HEROES:
            continue
        for number, entry in enumerate(entries, 1):
            folder = ROOT / RESOURCES / f"{HEROES[hero_id]}Draft{number}"
            folder.mkdir(parents=True, exist_ok=True)
            story = load_story(*entry)
            (folder / "story.json").write_text(
                json.dumps({"chapters": story["chapters"]}, ensure_ascii=False, indent=2) + "\n",
                encoding="utf-8",
            )
            for meta, folder_asset in ((folder.with_name(folder.name + ".meta"), True),
                                       (folder / "story.json.meta", False)):
                if not meta.exists():
                    importer = "DefaultImporter" if folder_asset else "TextScriptImporter"
                    text = "fileFormatVersion: 2\nguid: " + uuid.uuid4().hex + "\n"
                    if folder_asset:
                        text += "folderAsset: yes\n"
                    text += importer + ":\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n"
                    meta.write_text(text, encoding="utf-8")


if __name__ == "__main__":
    generate()

