"""Generate offline archive and Unity story resources from the shared manuscripts."""
import json
import uuid
import shutil
from pathlib import Path
if __package__:
    from .build_story_archive import CATALOG, ROOT, RESOURCES, WEB_DATA, _INDEX, load_story, render_data, story_illustration_copies
else:
    from build_story_archive import CATALOG, ROOT, RESOURCES, WEB_DATA, _INDEX, load_story, render_data, story_illustration_copies


def sync():
    for hero, _, _, _, _, entries in CATALOG:
        for entry in entries:
            folder = ROOT / RESOURCES / _INDEX[entry[2]]["unity"]
            folder.mkdir(parents=True, exist_ok=True)
            for original_path, target_path in story_illustration_copies(entry[2]):
                original = ROOT / original_path
                if not original.is_file():
                    raise FileNotFoundError(original)
                target = ROOT / target_path
                if not target.is_file() or original.read_bytes() != target.read_bytes():
                    shutil.copyfile(original, target)
            story = load_story(*entry, include_pending=True)
            chapters = story["chapters"]
            for number, chapter in enumerate(chapters, 1):
                if hero in {"saila", "adrien", "lena"}:
                    chapter["title"] = f"{number}. {chapter['title']}"
                for block in chapter["blocks"]:
                    block.pop("src", None)
            (folder / "story.json").write_text(json.dumps({"title": story["title"], "chapters": chapters}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
            for meta, directory in ((folder.with_name(folder.name + ".meta"), True), (folder / "story.json.meta", False)):
                if not meta.exists():
                    importer = "DefaultImporter" if directory else "TextScriptImporter"
                    meta.write_text("fileFormatVersion: 2\nguid: " + uuid.uuid4().hex + "\n" + ("folderAsset: yes\n" if directory else "") + importer + ":\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n", encoding="utf-8")
    for _, _, _, _, _, entries in CATALOG:
        for entry in entries:
            english = ROOT / entry[2] / "historia.en.md"
            if not english.is_file():
                continue
            info = _INDEX[entry[2]]
            story = load_story(*entry, include_pending=True, language="en")
            folder = ROOT / RESOURCES / info["unity"]
            for chapter in story["chapters"]:
                for block in chapter["blocks"]: block.pop("src", None)
            out = folder / "story_en.json"
            out.write_text(json.dumps({"title": story["title"], "chapters": story["chapters"]}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
            meta = out.with_name(out.name + ".meta")
            if not meta.exists():
                meta.write_text("fileFormatVersion: 2\nguid: " + uuid.uuid4().hex + "\nTextScriptImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n", encoding="utf-8")
    status_path = ROOT / RESOURCES / "story_approval.json"
    statuses = [{"folder": info["unity"], "draft": bool(info["draft"])} for info in _INDEX.values()]
    status_path.write_text(json.dumps({"stories": statuses}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    status_meta = status_path.with_name(status_path.name + ".meta")
    if not status_meta.exists():
        status_meta.write_text("fileFormatVersion: 2\nguid: " + uuid.uuid4().hex + "\nTextScriptImporter:\n  externalObjects: {}\n  userData:\n  assetBundleName:\n  assetBundleVariant:\n", encoding="utf-8")
    WEB_DATA.write_text(render_data(), encoding="utf-8")


if __name__ == "__main__":
    sync()
