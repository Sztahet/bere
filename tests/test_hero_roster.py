import json
import unittest
from collections import Counter
from pathlib import Path


class HeroRosterTests(unittest.TestCase):
    def test_lightning_support_roster_has_two_distinct_heroes(self):
        root = Path(__file__).resolve().parents[1]
        roster = json.loads((root / "riftoryn-unity/Assets/Resources/codex.json").read_text(encoding="utf-8"))
        supports = [hero for hero in roster["heroes"] if hero["role"] == "Wsparcie" and hero["element"] == "Błyskawice"]
        self.assertEqual({hero["id"] for hero in supports}, {"varek", "nera"})
        self.assertTrue(all(not hero.get("comingSoon", False) for hero in supports))

    def test_at_most_two_heroes_per_role_and_element(self):
        root = Path(__file__).resolve().parents[1]
        roster = json.loads((root / "riftoryn-unity/Assets/Resources/codex.json").read_text(encoding="utf-8"))
        counts = Counter((hero["role"], hero["element"]) for hero in roster["heroes"])
        self.assertEqual({key: count for key, count in counts.items() if count > 2}, {})


if __name__ == "__main__":
    unittest.main()

