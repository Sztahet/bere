import json
import unittest
from pathlib import Path

from scripts.build_timeline_web import WEB_DATA, render


class TimelineWebTests(unittest.TestCase):
    def test_timeline_data_regenerates_without_changes(self):
        before = WEB_DATA.read_text(encoding="utf-8")
        count = render()
        after = WEB_DATA.read_text(encoding="utf-8")
        self.assertGreater(count, 200)
        self.assertEqual(before, after)

    def test_timeline_sources_and_locations_exist(self):
        root = Path(__file__).resolve().parents[1]
        data = WEB_DATA.read_text(encoding="utf-8")
        prefix = "const riftorynTimeline = "
        line = next(line for line in data.splitlines() if line.startswith(prefix))
        events = json.loads(line[len(prefix):].rstrip(";"))

        self.assertTrue(events)
        for event in events:
            with self.subTest(source=event["source"], title=event["title"]):
                self.assertTrue((root / event["source"]).is_file())
                self.assertTrue(event["heroes"])
                self.assertTrue(event["location"])
                self.assertTrue(event["link"].startswith("#"))

    def test_archive_and_timeline_files_exist(self):
        root = Path(__file__).resolve().parents[1]
        for relative in (
            "os-czasu-riftoryn.html",
            "riftoryn-unity/Web/timeline.css",
            "riftoryn-unity/Web/timeline.js",
            "riftoryn-unity/Web/timeline-data.js",
        ):
            with self.subTest(file=relative):
                self.assertTrue((root / relative).is_file())
