import json
import unittest
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
WORLD = ROOT / "riftoryn-unity/Assets/Resources/encyclopedia_world.json"


class WorldRegionTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.world = json.loads(WORLD.read_text(encoding="utf-8"))

    def test_every_region_has_one_profile_and_a_map_settlement(self):
        regions = self.world["regions"]
        expected = {"Vareth", "Nivoren", "Zerath", "Verdan", "Kharum", "Aurel", "Vesper"}
        self.assertEqual({region["name"] for region in regions}, expected)
        self.assertEqual(len(regions), len(expected))
        region_ids = [region["id"] for region in regions]
        self.assertEqual(len(set(region_ids)), len(region_ids))
        place_ids = {place["id"] for place in self.world["places"]}
        self.assertEqual(set(region_ids), place_ids - {"val"})

    def test_region_articles_have_complete_polish_and_english_sections(self):
        expected = {"Krajobraz i osady", "Codzienność", "Gospodarka i splotyt",
                    "Historia i odbudowa", "Sprawy mieszkańców w 12 ET"}
        for region in self.world["regions"]:
            with self.subTest(region=region["name"]):
                for field in ("tagline", "overview"):
                    self.assertTrue(region[field].strip())
                    self.assertTrue(region[field + "En"].strip())
                    self.assertNotEqual(region[field], region[field + "En"])
                self.assertEqual({s["title"] for s in region["sections"]}, expected)
                self.assertEqual(len(region["sections"]), len(expected))
                for section in region["sections"]:
                    for field in ("title", "text"):
                        self.assertTrue(section[field].strip())
                        self.assertTrue(section[field + "En"].strip())
                        self.assertNotEqual(section[field], section[field + "En"])


if __name__ == "__main__":
    unittest.main()
