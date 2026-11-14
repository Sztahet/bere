"""Keep the in-game Splotyt guide aligned with the Polish website article."""
import json
from html.parser import HTMLParser
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / 'riftoryn-unity/Assets/Resources/Splotyt/story.json'


class GuideParser(HTMLParser):
    def __init__(self):
        super().__init__(convert_charrefs=True)
        self.in_article = False
        self.in_section = False
        self.capture = None
        self.text = []
        self.chapters = []
        self.current = None

    def handle_starttag(self, tag, attrs):
        attrs = dict(attrs)
        if tag == 'br' and self.capture:
            self.text.append(' ')
        if tag == 'article':
            self.in_article = True
        elif self.in_article and tag == 'section' and attrs.get('id'):
            self.in_section = True
            self.current = {'title': '', 'blocks': []}
        elif self.in_section and self.capture is None and tag in ('h2', 'h3', 'b', 'p'):
            self.capture = tag
            self.text = []

    def handle_data(self, data):
        if self.capture:
            self.text.append(data)

    def handle_endtag(self, tag):
        if self.capture == tag:
            text = ' '.join(''.join(self.text).split())
            if text:
                if tag == 'h2':
                    self.current['title'] = f"{len(self.chapters) + 1}. {text}"
                else:
                    self.current['blocks'].append({'text': text})
            self.capture = None
            self.text = []
        elif tag == 'section' and self.in_section:
            if not self.current['title'] or not self.current['blocks']:
                raise ValueError('Niekompletny rozdział o splotycie')
            self.chapters.append(self.current)
            self.current = None
            self.in_section = False
        elif tag == 'article':
            self.in_article = False


def parse_guide():
    parser = GuideParser()
    parser.feed((ROOT / 'splotyt.html').read_text(encoding='utf-8'))
    if len(parser.chapters) != 6:
        raise ValueError(f"Oczekiwano 6 rozdziałów, otrzymano {len(parser.chapters)}")
    return {'chapters': parser.chapters}


if __name__ == '__main__':
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT.write_text(json.dumps(parse_guide(), ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print('Zaktualizowano poradnik o splotycie w zasobach gry.')
