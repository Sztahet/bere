import unittest

from scripts.build_story_archive import CATALOG, WEB_DATA, load_story, render_data


class StoryArchiveTests(unittest.TestCase):
    def test_offline_data_matches_story_sources(self):
        self.assertEqual(WEB_DATA.read_text(encoding="utf-8"), render_data())

    def test_each_hero_has_three_readable_parts(self):
        for hero_id, _, _, _, _, entries in CATALOG:
            with self.subTest(hero=hero_id):
                self.assertEqual(len(entries), 3)
                self.assertEqual(len({entry[0] for entry in entries}), 3)
                for entry in entries:
                    story = load_story(*entry)
                    self.assertTrue(story["chapters"])
                    for chapter in story["chapters"]:
                        self.assertTrue(chapter["title"])
                        self.assertTrue(any(block.get("text") for block in chapter["blocks"]))
