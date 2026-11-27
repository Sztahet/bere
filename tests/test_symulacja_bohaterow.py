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

    def test_layouts_count_reproducibility_and_variation(self):
        for layout in ('rowny', 'losowy', 'skupiska'):
            towers = sim.make_towers(7, 12, layout)
            self.assertEqual(len(towers), 12)
            self.assertEqual(towers, sim.make_towers(7, 12, layout))
            self.assertTrue(all(0 < t.x < sim.DISTANCE for t in towers))
        self.assertNotEqual([t.x for t in sim.make_towers(7, 12, 'losowy')],
                            [t.x for t in sim.make_towers(8, 12, 'losowy')])
        self.assertEqual([t.x for t in sim.make_towers(7, 12, 'rowny')],
                         [t.x for t in sim.make_towers(8, 12, 'rowny')])

    def test_miko_sabotage_outcomes_and_cooldown(self):
        class FixedRandom:
            def __init__(self, value): self.value = value
            def randrange(self, count): return self.value
        for outcome in range(4):
            actor = sim.Runner(self.roster['miko'])
            tower = sim.Tower(10, 0)
            self.assertTrue(sim.cast(actor, [actor], [tower], 'Q', 0, FixedRandom(outcome)))
            self.assertEqual(actor.cooldown['Q'], 20)
            if outcome == 0: self.assertEqual(tower.disabled_until, 5)
            if outcome == 1: self.assertEqual(tower.hp, 0)
            if outcome == 2: self.assertTrue(tower.misfires)
            if outcome == 3: self.assertEqual(tower.overload_until, 5)

    def test_automatic_basic_attack_is_optional(self):
        hero = copy.deepcopy(self.roster['lumen'])
        hero['statystyki']['atak_podstawowy'] = 100
        without = sim.simulate([hero], 0, towers_count=1, distance=100, timeout=40, basic_attack=False)
        with_attack = sim.simulate([hero], 0, towers_count=1, distance=100, timeout=40, basic_attack=True)
        self.assertGreater(with_attack['members']['lumen']['hp'], without['members']['lumen']['hp'])

    def test_parameter_validation(self):
        with self.assertRaises(ValueError):
            sim.simulate([self.roster['kael']], towers_count=-1)
        with self.assertRaises(ValueError):
            sim.simulate([self.roster['kael']], step=.3)

    def test_teamwork_compares_same_four_without_counting_focal_hero(self):
        records = [
            {'a': 1, 'b': 1, 'c': 1, 'd': 1, 'e': 0},
            {'a': 0, 'b': 0, 'c': 0, 'd': 0, 'f': 1},
        ]
        result = {row['id']: row for row in sim.compare_teamwork(records)}
        self.assertEqual(result['e']['delta_pp'], 100)
        self.assertEqual(result['f']['delta_pp'], -100)
        self.assertEqual(result['e']['self_arrived_pct'], 0)
        self.assertEqual(result['f']['self_arrived_pct'], 100)
        self.assertEqual(result['e']['contexts'], 1)

    def test_teamwork_requires_replacements(self):
        with self.assertRaises(ValueError):
            sim.evaluate_teamwork(dict(list(self.roster.items())[:5]), [0])
        with self.assertRaises(ValueError):
            sim.evaluate_teamwork(self.roster, [])


if __name__ == '__main__':
    unittest.main()
