import json
import unittest
from pathlib import Path

from scripts.build_story_archive import CATALOG, WEB_DATA, load_story, render_data


class StoryArchiveTests(unittest.TestCase):
    def test_offline_data_matches_story_sources(self):
        self.assertEqual(WEB_DATA.read_text(encoding="utf-8"), render_data())

    def test_catalogued_stories_are_readable(self):
        for hero_id, _, _, _, _, entries in CATALOG:
            with self.subTest(hero=hero_id):
                self.assertEqual(len(entries), 3)
                self.assertEqual(len({entry[0] for entry in entries}), len(entries))
                for entry in entries:
                    story = load_story(*entry)
                    self.assertTrue(story["chapters"])
                    for chapter in story["chapters"]:
                        self.assertTrue(chapter["title"])
                        self.assertTrue(any(block.get("text") for block in chapter["blocks"]))

    def test_selected_trilogies_match_unity_resources(self):
        root = Path(__file__).resolve().parents[1]
        folders = {"nocten": "Nocten", "piecarius": "Piecarius", "morrak": "Morrak",
                   "thora": "Thora", "miko": "Miko"}
        found = set()
        for hero_id, _, _, _, _, entries in CATALOG:
            if hero_id not in folders:
                continue
            found.add(hero_id)
            for index, entry in enumerate(entries, 1):
                story = load_story(*entry)
                resource = root / "riftoryn-unity/Assets/Resources" / f"{folders[hero_id]}Draft{index}" / "story.json"
                self.assertEqual(json.loads(resource.read_text(encoding="utf-8"))["chapters"], story["chapters"])
        self.assertEqual(found, set(folders))
