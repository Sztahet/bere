import unittest

from scripts.build_story_archive import render_data
from scripts.sync_artwork import check_artwork


class ArtworkSyncTests(unittest.TestCase):
    def test_all_unity_artwork_matches_sources_in_assets(self):
        self.assertEqual(check_artwork(), [])

    def test_web_archive_uses_source_artwork(self):
        self.assertNotIn("riftoryn-unity/Assets/Resources/", render_data())
