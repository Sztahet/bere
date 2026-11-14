import json
import unittest
from pathlib import Path

from scripts.build_splotyt_story import parse_guide

ROOT = Path(__file__).resolve().parents[1]
RESOURCES = ROOT / 'riftoryn-unity/Assets/Resources'


class SplotytAndLenaTests(unittest.TestCase):
    def test_unity_guide_matches_website(self):
        guide = json.loads((RESOURCES / 'Splotyt/story.json').read_text(encoding='utf-8'))
        self.assertEqual(guide, parse_guide())
        text = ' '.join(block['text'] for chapter in guide['chapters'] for block in chapter['blocks'])
        for keyword in ('Wieże', 'Chłodnie', 'Piece grzewcze', 'Sygnalizatory'):
            self.assertIn(keyword, text)
        self.assertIn('zastosowanie Energia musi', text)
        menu = (ROOT / 'riftoryn-unity/Assets/Scripts/RiftorynMenu.cs').read_text(encoding='utf-8')
        self.assertIn('OpenSailaStory("Splotyt","SPLOTYT","KODEKS")', menu)

    def test_lena_portrait_and_concept(self):
        roster = json.loads((RESOURCES / 'codex.json').read_text(encoding='utf-8'))
        lena = next(hero for hero in roster['heroes'] if hero['id'] == 'lena')
        self.assertTrue(lena['comingSoon'])
        self.assertTrue((RESOURCES / 'Portraits/lena.png').is_file())
        self.assertTrue((ROOT / 'assets/heroes/modeling-concepts/lena.png').is_file())
        previews = (ROOT / 'riftoryn-bohaterowie.js').read_text(encoding='utf-8')
        self.assertIn('Resources/Portraits/lena.png', previews)


if __name__ == '__main__':
    unittest.main()
