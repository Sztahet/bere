import json
import unittest
from collections import Counter
from pathlib import Path


DATA = Path(__file__).resolve().parents[1] / 'dokumentacja' / 'bohaterowie-16'
EXPECTED = {
    'kael', 'thora', 'brom', 'morrak', 'nera', 'niva', 'liora', 'sarna',
    'rian', 'saila', 'zet', 'miko', 'koneva', 'pyrra', 'ana', 'lumen',
}


class HeroDesignDataTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.files = sorted(DATA.glob('*.json'))
        cls.heroes = [json.loads(path.read_text(encoding='utf-8')) for path in cls.files]

    def test_roster_roles_and_elements(self):
        ids = [h['id'] for h in self.heroes]
        self.assertEqual(set(ids), EXPECTED)
        self.assertEqual(len(ids), len(set(ids)))
        self.assertEqual({p.stem for p in self.files}, EXPECTED)
        self.assertEqual(Counter(h['rola'] for h in self.heroes),
                         {'tank': 4, 'wsparcie': 4, 'specjalista': 4, 'przywolywacz': 4})
        self.assertEqual({h['zywiol'] for h in self.heroes},
                         {'ogien', 'lod', 'blyskawice', 'natura', 'ziemia', 'swiatlo', 'ciemnosc'})

    def test_stats_skills_and_pets(self):
        for hero in self.heroes:
            with self.subTest(hero=hero['id']):
                stats = hero['statystyki']
                self.assertEqual(hero['status'], 'projekt_do_testow')
                self.assertGreater(stats['hp'], 0)
                self.assertGreater(stats['predkosc_m_s'], 0)
                self.assertGreater(stats['atak_podstawowy'], 0)
                self.assertGreater(stats['zasieg_ataku_m'], 0)
                self.assertGreater(stats['odnowienie_ataku_s'], 0)
                self.assertEqual(set(hero['umiejetnosci']), {'Q', 'E', 'R'})
                for skill in [hero['pasywna'], *hero['umiejetnosci'].values()]:
                    self.assertTrue(skill['nazwa'] and skill['opis'] and skill['cel'])
                    self.assertGreaterEqual(skill['odnowienie_s'], 0)
                    self.assertTrue(skill['parametry'])
                    for value in skill['parametry'].values():
                        if isinstance(value, (int, float)):
                            self.assertGreaterEqual(value, 0)
                for skill in hero['umiejetnosci'].values():
                    self.assertGreater(skill['odnowienie_s'], 0)
                if hero['rola'] == 'przywolywacz':
                    self.assertTrue(hero['towarzysz']['nazwa'])
                    self.assertGreater(hero['towarzysz']['hp'], 0)
                    self.assertGreater(hero['towarzysz']['predkosc_m_s'], 0)
                else:
                    self.assertIsNone(hero['towarzysz'])


if __name__ == '__main__':
    unittest.main()
