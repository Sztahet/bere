import json
import re
import unittest
from pathlib import Path

from scripts.build_timeline_web import WEB_DATA, parse_world_history, render


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
        story_events = [event for event in events if event["kind"] == "story"]

        self.assertTrue(story_events)
        for event in story_events:
            with self.subTest(source=event["source"], title=event["title"]):
                self.assertTrue((root / event["source"]).is_file())
                self.assertTrue(event["heroes"])
                self.assertTrue(event["location"])
                self.assertTrue(event["link"].startswith("#"))

    def test_world_history_events_join_the_story_chronology(self):
        root = Path(__file__).resolve().parents[1]
        data = WEB_DATA.read_text(encoding="utf-8")
        prefix = "const riftorynTimeline = "
        line = next(line for line in data.splitlines() if line.startswith(prefix))
        events = json.loads(line[len(prefix):].rstrip(";"))
        world_events = [event for event in events if event["kind"] == "history"]
        world_history = (root / "world_history.md").read_text(encoding="utf-8")
        timeline_text = world_history.split("## Pełna oś czasu\n", 1)[1].split(
            "\n## Konflikty na przyszłość", 1
        )[0]
        source_titles = [
            re.fullmatch(r"#### .+?\s+—\s+(.+)", heading).group(1).strip()
            for heading in timeline_text.splitlines()
            if heading.startswith("#### ")
        ]

        self.assertEqual(len(world_events), len(parse_world_history()))
        self.assertEqual([event["title"] for event in world_events], source_titles)
        self.assertGreaterEqual(len(world_events), 25)
        for event in world_events:
            with self.subTest(title=event["title"]):
                self.assertEqual(event["heroes"], [])
                self.assertTrue(event["description"])
                self.assertEqual(event["source"], "world_history.md")
                self.assertTrue((root / event["source"]).is_file())

        by_title = {event["title"].casefold(): index for index, event in enumerate(events)}
        self.assertLess(by_title["kruchy rozejm"], by_title["pierwszy pojedynek bram"])
        self.assertLess(by_title["pierwszy pojedynek bram"], by_title["traktat równych ścieżek"])
        self.assertLess(by_title["traktat równych ścieżek"], by_title["pierwsze próby traktatowe"])
        self.assertLess(by_title["traktat równych ścieżek"], by_title["otwarcie archiwum"])
        self.assertLess(by_title["wyścig obron i przełamywania"], by_title["wojna na wyczerpanie"])
        self.assertLess(by_title["wojna na wyczerpanie"], by_title["eksperyment zerath–aurel"])
        self.assertLess(by_title["powódź w korzennej przystani"], by_title["kodeks prób"])

        night = next(
            event for event in world_events
            if event["title"].casefold() == "noc zgasłych wież i seria awarii"
        )
        self.assertIn("pozostają nieustalone", night["description"])

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
