"""Build Unity's Lena story resources from the canonical manuscripts."""
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RESOURCES = ROOT / 'riftoryn-unity/Assets/Resources'
STORIES = [
    ('RIFTORYN_LENA_01_LUDZIE_SA_CIEKAWSI_OD_KRYSZTALOW.md', 'LenaStory1'),
    ('RIFTORYN_LENA_02_IMIONA_KTORE_ZOSTAJA.md', 'LenaStory2'),
    ('RIFTORYN_LENA_03_ZNAM_KOGOS_KTO_POTRAFI.md', 'LenaStory3'),
]


def build(source, folder):
    text = (ROOT / source).read_text(encoding='utf-8')
    pieces = re.split(r'(?m)^## ([IVX]+)\. (.+)$', text)
    if len(pieces) < 4 or (len(pieces) - 1) % 3:
        raise ValueError(f'Nieprawidłowe rozdziały: {source}')
    chapters = []
    for index in range(1, len(pieces), 3):
        paragraphs = [p.strip() for p in re.split(r'\n\s*\n', pieces[index + 2]) if p.strip()]
        chapters.append({
            'title': f'{len(chapters) + 1}. {pieces[index + 1]}',
            'blocks': [{'text': re.sub(r'\*(.*?)\*', r'\1', re.sub(r'\*\*(.*?)\*\*', r'\1', p))}
                       for p in paragraphs],
        })
    out = RESOURCES / folder / 'story.json'
    out.parent.mkdir(parents=True, exist_ok=True)
    out.write_text(json.dumps({'chapters': chapters}, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')


if __name__ == '__main__':
    for entry in STORIES:
        build(*entry)
        print(f'Utworzono {entry[1]}')
