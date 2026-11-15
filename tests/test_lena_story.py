import json
import re
import unittest
from pathlib import Path

from scripts.build_lena_stories import STORIES
from scripts.build_story_archive import CATALOG, load_story

ROOT = Path(__file__).resolve().parents[1]
RESOURCES = ROOT / 'riftoryn-unity/Assets/Resources'


class LenaStoryTests(unittest.TestCase):
    def test_unity_and_archive_match_the_approved_manuscripts(self):
        entries = next(hero[-1] for hero in CATALOG if hero[0] == 'lena')
        self.assertEqual(len(entries), len(STORIES))
        for (source, folder), entry in zip(STORIES, entries):
            with self.subTest(folder=folder):
                text = (ROOT / source).read_text(encoding='utf-8')
                parts = re.split(r'(?m)^## [IVX]+\. (.+)$', text)
                chapters = json.loads((RESOURCES / folder / 'story.json').read_text(encoding='utf-8'))['chapters']
                self.assertEqual(len(chapters), (len(parts) - 1) // 2)
                for index, chapter in enumerate(chapters):
                    self.assertEqual(chapter['title'], f'{index + 1}. {parts[2 * index + 1]}')
                    paragraphs = [p.strip() for p in re.split(r'\n\s*\n', parts[2 * index + 2]) if p.strip()]
                    paragraphs = [re.sub(r'\*(.*?)\*', r'\1', re.sub(r'\*\*(.*?)\*\*', r'\1', p)) for p in paragraphs]
                    self.assertEqual([block['text'] for block in chapter['blocks'] if 'text' in block], paragraphs)
                self.assertEqual(entry[2], folder)
                archive = load_story(*entry)
                self.assertNotIn('draft', archive)
                self.assertEqual([chapter['blocks'] for chapter in archive['chapters']],
                                 [chapter['blocks'] for chapter in chapters])

    def test_coming_soon_card_still_opens_all_three_stories(self):
        menu = (ROOT / 'riftoryn-unity/Assets/Scripts/RiftorynMenu.cs').read_text(encoding='utf-8')
        self.assertIn('hero.id=="lena"', menu)
        for _, folder in STORIES:
            self.assertIn(f'OpenSailaStory("{folder}"', menu)


if __name__ == '__main__':
    unittest.main()
