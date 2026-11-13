import json
import re
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RESOURCES = ROOT / 'riftoryn-unity/Assets/Resources'


class SailaStoryTests(unittest.TestCase):
    def check_shared_story(self, folder, draft_name, expected_images):
        resource = RESOURCES / folder
        story = json.loads((resource / 'story.json').read_text(encoding='utf-8-sig'))
        draft = (ROOT / draft_name).read_text(encoding='utf-8-sig')
        source_chapters = re.split(r'(?m)^## (?=\d+\. )', draft)[1:]
        chapters = story['chapters']
        self.assertEqual(len(chapters), 7)
        self.assertEqual(len(source_chapters), 7)
        images = []
        for chapter, source in zip(chapters, source_chapters):
            title, body = source.split('\n', 1)
            self.assertEqual(chapter['title'], title)
            body = body.split('\n### ', 1)[0]
            paragraphs = [p.strip() for p in re.split(r'\n\s*\n', body.strip())
                          if p.strip() not in {'---', '*— Koniec —*'} and 'MIEJSCE NA ILUSTRACJĘ' not in p]
            self.assertEqual([b['text'] for b in chapter['blocks'] if 'text' in b], paragraphs)
            for block in chapter['blocks']:
                if 'image' in block:
                    self.assertTrue(block.get('alt'))
                    images.append(block['image'])
                    self.assertTrue((resource / f"{block['image']:02}.jpg").is_file())
                else:
                    self.assertTrue(block.get('text'))
        self.assertEqual(images, expected_images)

    def test_shared_content_and_illustrations(self):
        self.check_shared_story('SailaStory', 'RIFTORYN_SAILA_MINIKSIAZKA_PROPOZYCJA.md', [1, 2, 3, 7, 4, 5])

    def test_html_reads_unity_story_without_copying_it(self):
        page = (ROOT / 'saila-opowiesc.html').read_text(encoding='utf-8-sig')
        reader = (ROOT / 'riftoryn-unity/Web/saila-reader.js').read_text(encoding='utf-8-sig')
        self.assertIn('riftoryn-unity/Web/saila-reader.js', page)
        self.assertIn('../Assets/Resources/${storyFolder}/', reader)
        self.assertIn('SailaStory', reader)
        self.assertIn('SailaAwakening', reader)
        self.assertIn('story.json', reader)
        self.assertNotIn('W Nivoren dzień zaczyna się od sprawdzenia drogi.', page)
        self.assertFalse(re.search(r'<section\s+class="chapter"', page))

    def test_awakening_shared_draft_and_approved_images(self):
        self.check_shared_story('SailaAwakening', 'RIFTORYN_SAILA_ODBLOKOWANIE_OPOWIESC_DRAFT.md', [1, 2, 4, 5, 6, 7])
        page = (ROOT / 'saila-gdy-wieza-zamilkla.html').read_text(encoding='utf-8')
        self.assertIn('data-story="awakening"', page)
        self.assertIn('Propozycja literacka', page)


if __name__ == '__main__':
    unittest.main()
