import json
import unittest
import tempfile
from pathlib import Path
from unittest.mock import patch

from scripts.build_story_archive import CATALOG, ROOT, RESOURCES, _INDEX, load_story
from scripts import build_story_archive as archive


class UnifiedStoriesTests(unittest.TestCase):
    def test_stories_do_not_use_surnames(self):
        for hero, _, _, _, _, entries in CATALOG:
            for entry in entries:
                for language in ('pl', 'en'):
                    manuscript = ROOT / entry[2] / f'historia.{language}.md'
                    if not manuscript.is_file():
                        continue
                    with self.subTest(hero=hero, story=entry[0], language=language):
                        self.assertNotRegex(manuscript.read_text(encoding='utf-8').lower(),
                                            r'nazwi' + r'sk|surname|family name|last name')

    def test_all_unity_stories_match_shared_sources(self):
        for hero, _, _, _, _, entries in CATALOG:
            for entry in entries:
                with self.subTest(hero=hero, story=entry[0]):
                    story = load_story(*entry, include_pending=True)
                    for index, chapter in enumerate(story['chapters'], 1):
                        if hero in {'saila', 'adrien', 'lena'}:
                            chapter['title'] = f"{index}. {chapter['title']}"
                        for block in chapter['blocks']:
                            block.pop('src', None)
                    output = ROOT / RESOURCES / _INDEX[entry[2]]['unity'] / 'story.json'
                    self.assertEqual(json.loads(output.read_text(encoding='utf-8'))['chapters'], story['chapters'])

    def test_english_and_pending_images_are_optional(self):
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            folder = root / 'story'
            folder.mkdir()
            (folder / 'historia.pl.md').write_text('# Tytuł\n\n## Rozdział\n\nTekst.\n\n<!-- ilustracja: 01 -->\n', encoding='utf-8')
            (folder / 'ilustracje.json').write_text('{"01":{"pending":true,"alt":"Widok"}}', encoding='utf-8')
            entry = ('test', None, 'story')
            with patch.object(archive, 'ROOT', root), patch.object(archive, '_INDEX', {'story': {'unity': 'Test', 'draft': False}}):
                self.assertNotIn('en', archive.load_translations(entry))
                self.assertEqual(len(archive.load_story(*entry)['chapters'][0]['blocks']), 1)
                (folder / 'historia.en.md').write_text('# Title\n\n## Chapter\n\nText.\n\n<!-- ilustracja: 01 -->\n', encoding='utf-8')
                self.assertEqual(archive.load_translations(entry)['en']['title'], 'Title')
