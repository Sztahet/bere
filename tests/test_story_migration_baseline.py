"""Temporary migration guard; remove after the migrated readers are verified."""
import hashlib
import json
import unittest
from pathlib import Path

from scripts.build_story_archive import CATALOG, load_story


class StoryMigrationBaselineTests(unittest.TestCase):
    def test_stories_match_before_migration(self):
        path = Path(__file__).with_name('story_migration_baseline.json')
        expected = json.loads(path.read_text(encoding='utf-8'))
        actual = {}
        for hero, _, _, _, _, entries in CATALOG:
            for entry in entries:
                story = load_story(*entry)
                payload = json.dumps(story, ensure_ascii=False, sort_keys=True,
                                     separators=(',', ':')).encode('utf-8')
                actual[f'{hero}/{entry[0]}'] = hashlib.sha256(payload).hexdigest()
        self.assertEqual(expected, actual)
