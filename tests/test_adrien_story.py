import json
import re
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RESOURCES = ROOT / 'riftoryn-unity/Assets/Resources'
STORIES = [
    ('AdrienStory1', 'RIFTORYN_ADRIEN_01_SWIATLO_KTORE_WSKAZALO_DROGE.md', 3),
    ('AdrienStory2', 'RIFTORYN_ADRIEN_02_KOBIETA_KTORA_WIEDZIALA.md', 4),
    ('AdrienStory3', 'RIFTORYN_ADRIEN_03_WYNALAZEK_KTOREGO_NIE_PRZEWIDZIALEM.md', 5),
]


class AdrienStoryTests(unittest.TestCase):
    def test_reader_matches_manuscripts_and_has_image_slots(self):
        for folder, source, count in STORIES:
            with self.subTest(folder=folder):
                manuscript = (ROOT / source).read_text(encoding='utf-8')
                pieces = re.split(r'(?m)^## Rozdział [IVX]+ — (.+)$', manuscript)
                chapters = json.loads((RESOURCES / folder / 'story.json').read_text(encoding='utf-8'))['chapters']
                self.assertEqual(len(chapters), count)
                self.assertEqual(len(pieces), count * 2 + 1)
                slots = []
                for idx, chapter in enumerate(chapters):
                    self.assertEqual(chapter['title'], f'{idx + 1}. {pieces[2 * idx + 1]}')
                    paras = [p.strip() for p in re.split(r'\n\s*\n', pieces[2 * idx + 2]) if p.strip()]
                    paras = [re.sub(r'\*(.*?)\*', r'\1', re.sub(r'\*\*(.*?)\*\*', r'\1', p)) for p in paras]
                    if idx == 0 and folder == 'AdrienStory1':
                        paras.insert(0, next(p.strip('*') for p in pieces[0].split('\n\n') if p.startswith('*Daty sprzed')))
                    self.assertEqual([b['text'] for b in chapter['blocks'] if 'text' in b], paras)
                    slots.extend(b for b in chapter['blocks'] if 'image' in b)
                self.assertEqual([b['image'] for b in slots], list(range(1, 9)))
                for block in slots:
                    self.assertTrue(block.get('alt'))
                    if not block.get('pending'):
                        self.assertTrue((RESOURCES / folder / f"{block['image']:02}.jpg").is_file())

    def test_hero_card_opens_all_three_stories(self):
        menu = (ROOT / 'riftoryn-unity/Assets/Scripts/RiftorynMenu.cs').read_text(encoding='utf-8')
        for folder, _, _ in STORIES:
            self.assertIn(f'OpenSailaStory("{folder}"', menu)
        self.assertIn('hero.id=="adrien"', menu)


if __name__ == '__main__':
    unittest.main()
