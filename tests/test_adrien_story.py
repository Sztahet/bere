import json
import unittest
from pathlib import Path
from scripts.build_story_archive import CATALOG, ROOT, RESOURCES, _INDEX, load_story

class StoryConsistencyTests(unittest.TestCase):
    def test_unity_matches_unified_manuscript(self):
        entries = next(row[-1] for row in CATALOG if row[0] == HERO)
        for entry in entries:
            with self.subTest(story=entry[0]):
                chapters = load_story(*entry, include_pending=True)["chapters"]
                for number, chapter in enumerate(chapters, 1):
                    chapter["title"] = f"{number}. {chapter['title']}"
                    for block in chapter["blocks"]: block.pop("src", None)
                path = ROOT / RESOURCES / _INDEX[entry[2]]["unity"] / "story.json"
                self.assertEqual(json.loads(path.read_text(encoding="utf-8"))["chapters"], chapters)

HERO = 'adrien'
