import json
import re
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RESOURCE = ROOT / 'riftoryn-unity/Assets/Resources/SailaStory'


class SailaStoryTests(unittest.TestCase):
    def test_shared_content_and_illustrations(self):
        story = json.loads((RESOURCE / 'story.json').read_text(encoding='utf-8-sig'))
        chapters = story['chapters']
        self.assertEqual(len(chapters), 7)
        images = []
        for chapter in chapters:
            self.assertTrue(chapter['title'])
            for block in chapter['blocks']:
                if 'image' in block:
                    self.assertTrue(block.get('alt'))
                    images.append(block['image'])
                    self.assertTrue((RESOURCE / f"{block['image']:02}.jpg").is_file())
                else:
                    self.assertTrue(block.get('text'))
        self.assertEqual(sorted(images), list(range(1, 9)))

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
        resource = ROOT / 'riftoryn-unity/Assets/Resources/SailaAwakening'
        data = json.loads((resource / 'story.json').read_text(encoding='utf-8-sig'))
        draft = (ROOT / 'RIFTORYN_SAILA_ODBLOKOWANIE_OPOWIESC_DRAFT.md').read_text(encoding='utf-8-sig')
        source_chapters = re.split(r'(?m)^## (?=\d+\. )', draft)[1:]
        self.assertEqual(len(data['chapters']), 7)
        self.assertEqual(len(source_chapters), 7)
        for chapter, source in zip(data['chapters'], source_chapters):
            body = source.split('\n', 1)[1].split('### Roboczy opis gatunku')[0]
            paragraphs = [x.strip() for x in re.split(r'\n\s*\n', body.strip())
                          if x.strip() != '---' and 'MIEJSCE NA ILUSTRACJĘ' not in x]
            self.assertEqual([b['text'] for b in chapter['blocks'] if 'text' in b], paragraphs)
        images = [b['image'] for c in data['chapters'] for b in c['blocks'] if 'image' in b]
        self.assertEqual(images, [1, 5, 7])
        for image in images:
            self.assertTrue((resource / f'{image:02}.jpg').is_file())
        page = (ROOT / 'saila-gdy-wieza-zamilkla.html').read_text(encoding='utf-8')
        self.assertIn('data-story="awakening"', page)
        self.assertIn('trzy ilustracje', page)


if __name__ == '__main__':
    unittest.main()
