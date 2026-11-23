import json
import unittest

from scripts.build_hero_traits import OUTPUT, profiles


class HeroTraitsTests(unittest.TestCase):
    def test_export_matches_proposals_and_retains_uncertainty(self):
        expected = profiles()
        self.assertEqual(json.loads(OUTPUT.read_text(encoding="utf-8")), expected)
        for profile in expected["profiles"]:
            with self.subTest(hero=profile["id"]):
                self.assertEqual(len(profile["scores"]), 7)
                self.assertEqual(len(profile["tentative"]), 7)
                self.assertEqual(sum(profile["scores"]), 21)
                self.assertTrue(all(1 <= value <= 5 for value in profile["scores"]))


if __name__ == "__main__":
    unittest.main()
