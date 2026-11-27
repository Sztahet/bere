import copy
import itertools
import unittest

from tools import symulacja_bohaterow as sim


class SimulationTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.roster = sim.load_roster()

    def test_roster_has_4368_distinct_five_person_teams(self):
        self.assertEqual(sum(1 for _ in itertools.combinations(self.roster, 5)), 4368)

    def test_empty_route_and_timeout(self):
        team = [self.roster['kael'], self.roster['zet']]
        result = sim.simulate(team, 12, towers_count=0, timeout=10, distance=10)
        self.assertEqual(result['arrived'], 2)
        self.assertLessEqual(result['time_s'], 3)
        result = sim.simulate(team, 12, towers_count=0, timeout=1, distance=100)
        self.assertEqual(result['arrived'], 0)
        self.assertEqual(result['time_s'], 1)

    def test_seed_and_team_order_are_reproducible(self):
        team = [self.roster[x] for x in ['kael', 'nera', 'zet', 'koneva', 'sarna']]
        self.assertEqual(sim.simulate(team, 42), sim.simulate(list(reversed(team)), 42))
        self.assertEqual(sim.make_towers(42), sim.make_towers(42))

    def test_ability_consumes_cooldown_only_if_successful(self):
        runner = sim.Runner(self.roster['rian'])
        tower = sim.Tower(100, 0)
        self.assertFalse(sim.cast(runner, [runner], [tower], 'Q', 0))
        self.assertEqual(runner.cooldown, {})
        tower.x = 10
        self.assertTrue(sim.cast(runner, [runner], [tower], 'Q', 1))
        self.assertEqual(tower.hp, 0)
        self.assertEqual(runner.cooldown['Q'], 17)
        tower.hp = 100
        self.assertFalse(sim.cast(runner, [runner], [tower], 'Q', 2))
        self.assertEqual(tower.hp, 100)

    def test_hp_and_speed_from_data_change_results(self):
        base = copy.deepcopy(self.roster['kael'])
        base['statystyki']['predkosc_m_s'] = 1
        slow = sim.simulate([base], towers_count=0, distance=10, timeout=8)
        base['statystyki']['predkosc_m_s'] = 5
        fast = sim.simulate([base], towers_count=0, distance=10, timeout=8)
        self.assertEqual(slow['arrived'], 0)
        self.assertEqual(fast['arrived'], 1)
        base['statystyki']['hp'] = 1
        self.assertEqual(sim.Runner(base).hp, 1)

    def test_duplicate_team_is_rejected(self):
        with self.assertRaises(ValueError):
            sim.simulate([self.roster['kael'], self.roster['kael']])


if __name__ == '__main__':
    unittest.main()
