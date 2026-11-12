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
        self.assertIn('../Assets/Resources/SailaStory/', reader)
        self.assertIn('story.json', reader)
        self.assertNotIn('W Nivoren dzień zaczyna się od sprawdzenia drogi.', page)
        self.assertFalse(re.search(r'<section\s+class="chapter"', page))


if __name__ == '__main__':
    unittest.main()
