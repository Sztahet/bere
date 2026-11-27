"""Powtarzalny model przekroju trasy; zob. dokumentacja/symulacja-bohaterow.md."""
from __future__ import annotations

import argparse
import csv
import hashlib
import itertools
import json
import random
import sys
from dataclasses import dataclass, field
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
DATA = ROOT / 'dokumentacja' / 'bohaterowie-16'
DISTANCE = 472.5
TIMEOUT = 137
# Zestawienie jawne: pozostałe zdolności nie są oceniane przez model.
IMPLEMENTED = {
    'kael': {'Q'}, 'thora': {'E'}, 'brom': {'R'}, 'morrak': {'E'},
    'nera': {'Q'}, 'niva': {'E'}, 'liora': {'E'}, 'sarna': {'Q', 'E'},
    'rian': {'Q', 'R'}, 'saila': {'Q', 'R'}, 'zet': {'Q', 'R'},
    'miko': {'Q', 'E', 'R'},
}
# obrażenia, odstęp między strzałami i zasięg z prototypu Unity
TOWER_TYPES = ((13, 1.4, 13.8), (7, 1.5, 15), (9, 1, 13.8),
               (10, 1.3, 15), (10, 1.3, 15), (10, 1.3, 15), (10, 1.3, 15))


def load_roster(directory=DATA):
    roster = {}
    for path in sorted(directory.glob('*.json')):
        hero = json.loads(path.read_text(encoding='utf-8'))
        if hero['id'] in roster:
            raise ValueError(f"Powtórzony identyfikator: {hero['id']}")
        roster[hero['id']] = hero
    if not roster:
        raise ValueError(f'Brak danych bohaterów: {directory}')
    return roster


@dataclass
class Runner:
    data: dict
    hp: float = field(init=False)
    x: float = 0
    shield: float = 0
    finished: bool = False
    cooldown: dict = field(default_factory=dict)
    speed_until: float = 0
    speed_factor: float = 1

    def __post_init__(self):
        self.hp = self.data['statystyki']['hp']

    @property
    def alive(self):
        return self.hp > 0 and not self.finished


@dataclass
class Tower:
    x: float
    kind: int
    hp: float = 100
    next_shot: float = 0
    disabled_until: float = 0
    misfires: bool = False
    overload_until: float = 0


def make_towers(seed, count=10, layout='losowy'):
    rng = random.Random(seed)
    if layout not in ('rowny', 'losowy', 'skupiska'):
        raise ValueError('Nieznany układ wież.')
    positions = []
    for i in range(count):
        if layout == 'skupiska':
            # Pary tworzą trudniejsze odcinki, przy zachowaniu całej trasy.
            group, group_count = i // 2, (count + 1) // 2
            position = DISTANCE * (group + 1) / (group_count + 1) + (-8 if i % 2 == 0 else 8)
        else:
            spacing = DISTANCE / (count + 1)
            position = spacing * (i + 1)
            if layout == 'losowy':
                position += rng.uniform(-.3 * spacing, .3 * spacing)
        positions.append(position)
    return [Tower(x, rng.randrange(len(TOWER_TYPES))) for x in positions]


def allies_near(actor, runners, distance):
    return [r for r in runners if r is not actor and r.alive and abs(r.x-actor.x) <= distance]


def cast(actor, runners, towers, slot, now, rng=None):
    """Tylko jawnie wyliczone efekty; False nie uruchamia czasu odnowienia."""
    hero = actor.data['id']
    if slot not in IMPLEMENTED.get(hero, ()) or now < actor.cooldown.get(slot, 0):
        return False
    spec = actor.data['umiejetnosci'][slot]
    p = spec['parametry']
    near = allies_near(actor, runners, p.get('zasieg_m', p.get('promien_m', 0)))
    target = min(near, key=lambda r: (abs(r.x-actor.x), r.data['id']), default=None)
    active = [t for t in towers if t.hp > 0 and abs(t.x-actor.x) <= p.get('zasieg_m', p.get('promien_m', 0))]
    tower = min(active, key=lambda t: abs(t.x-actor.x), default=None)
    used = False
    if (hero, slot) in {('kael', 'Q'), ('nera', 'Q'), ('morrak', 'E')}:
        if any(t.hp > 0 and abs(t.x-actor.x) < 18 for t in towers):
            actor.shield += p['oslona_wlasna']
            if target:
                target.shield += p['oslona_sojusznika']
            used = True
    elif (hero, slot) == ('thora', 'E'):
        if any(t.hp > 0 and abs(t.x-actor.x) < 18 for t in towers):
            actor.shield += p['oslona']
            used = True
    elif (hero, slot) == ('brom', 'R'):
        if any(t.hp > 0 and abs(t.x-actor.x) < 18 for t in towers):
            for r in [actor, *near]:
                r.shield += p['oslona']
            used = True
    elif hero in ('niva', 'liora', 'sarna') and slot == 'E':
        injured = [r for r in near if r.hp < r.data['statystyki']['hp']]
        if injured:
            recipient = min(injured, key=lambda r: (abs(r.x-actor.x), r.data['id']))
            recipient.hp = min(recipient.data['statystyki']['hp'], recipient.hp+p['leczenie'])
            used = True
    elif (hero, slot) == ('sarna', 'Q'):
        if any(t.hp > 0 and abs(t.x-actor.x) < 18 for t in towers):
            for r in [actor] + ([target] if target else []):
                r.speed_factor = max(r.speed_factor if r.speed_until > now else 1, 1+p['premia_predkosci_procent']/100)
                r.speed_until = max(r.speed_until, now+p['czas_s'])
            used = True
    elif (hero, slot) == ('zet', 'R'):
        if any(t.hp > 0 and abs(t.x-actor.x) < 18 for t in towers):
            actor.speed_factor = p['predkosc_procent']/100
            actor.speed_until = now+p['czas_s']
            used = True
    elif (hero, slot) == ('miko', 'Q') and tower:
        if rng is None:
            raise ValueError('Losowy sabotaż wymaga generatora losowego.')
        outcome = rng.randrange(4)
        if outcome == 0:
            tower.disabled_until = max(tower.disabled_until, now+p['wylaczenie_s'])
        elif outcome == 1:
            tower.hp = 0
        elif outcome == 2:
            tower.misfires = True
        else:
            tower.overload_until = max(tower.overload_until, now+p['czas_pecha_s'])
        used = True
    elif hero in ('rian', 'saila', 'zet', 'miko') and tower:
        selected = active if slot == 'R' and hero in ('saila', 'miko') else [tower]
        for t in selected:
            t.hp = max(0, t.hp-p.get('obrazenia', 0))
            t.disabled_until = max(t.disabled_until, now+p.get('wylaczenie_s', 0))
        used = True
    if used:
        actor.cooldown[slot] = now+spec['odnowienie_s']
    return used


def simulate(team, seed=0, *, towers_count=10, timeout=TIMEOUT, distance=DISTANCE,
             step=.5, layout='losowy', basic_attack=False):
    if not team or len({h['id'] for h in team}) != len(team):
        raise ValueError('Drużyna musi zawierać różnych bohaterów.')
    runners = [Runner(h) for h in sorted(team, key=lambda h: h['id'])]
    if towers_count < 0 or distance <= 0 or timeout <= 0 or step not in (.25, .5, 1):
        raise ValueError('Nieprawidłowe parametry symulacji.')
    towers = make_towers(seed, towers_count, layout)
    for t in towers:
        t.x *= distance / DISTANCE
    rng = random.Random(seed ^ 0xA47)
    time = 0.0
    # Wszystkie zdarzenia są kwantowane do najbliższego kolejnego kroku.
    while time < timeout and any(r.alive for r in runners):
        for actor in runners:
            if actor.alive:
                for slot in ('Q', 'E', 'R'):
                    cast(actor, runners, towers, slot, time, rng)
                if basic_attack and time >= actor.cooldown.get('attack', 0):
                    reach = actor.data['statystyki']['zasieg_ataku_m']
                    targets = [t for t in towers if t.hp > 0 and abs(t.x-actor.x) <= reach]
                    if targets:
                        target = min(targets, key=lambda t: abs(t.x-actor.x))
                        target.hp = max(0, target.hp-actor.data['statystyki']['atak_podstawowy'])
                        actor.cooldown['attack'] = time+actor.data['statystyki']['odnowienie_ataku_s']
        for tower in towers:
            damage, interval, radius = TOWER_TYPES[tower.kind]
            if tower.hp <= 0 or time < tower.disabled_until or time < tower.next_shot:
                continue
            targets = [r for r in runners if r.alive and abs(r.x-tower.x) <= radius]
            if targets:
                target = rng.choice(targets)
                if not tower.misfires or rng.random() >= .5:
                    blocked = min(target.shield, damage)
                    target.shield -= blocked
                    target.hp = max(0, target.hp-(damage-blocked))
                tower.next_shot = time+interval/(1.5 if time < tower.overload_until else 1)
        for actor in runners:
            if actor.alive:
                actor.shield *= .9**step
                actor.x += step*actor.data['statystyki']['predkosc_m_s'] * (actor.speed_factor if time < actor.speed_until else 1)
                if actor.x >= distance:
                    actor.x = distance
                    actor.finished = True
        time += step
    return {'arrived': sum(r.finished for r in runners), 'team_size': len(runners),
            'time_s': time, 'remaining_hp': round(sum(r.hp for r in runners if r.finished), 2),
            'members': {r.data['id']: {'arrived': r.finished, 'hp': round(r.hp, 2),
                                       'distance_m': round(r.x, 1)} for r in runners}}


def evaluate(team, seeds, *, towers_count=10, step=.5, layout='losowy', basic_attack=False):
    results = [simulate(team, seed, towers_count=towers_count, step=step, layout=layout,
                        basic_attack=basic_attack) for seed in seeds]
    return {'members': ','.join(sorted(h['id'] for h in team)),
            'runs': len(results), 'mean_arrived': sum(r['arrived'] for r in results)/len(results),
            'all_arrived_pct': 100*sum(r['arrived'] == len(team) for r in results)/len(results),
            'mean_time_s': sum(r['time_s'] for r in results)/len(results)}


def positive_int(value):
    result = int(value)
    if result <= 0:
        raise argparse.ArgumentTypeError('Wartość musi być dodatnia.')
    return result


def main(argv=None):
    parser = argparse.ArgumentParser(description='Uproszczona symulacja 1D, nie test Unity.')
    sub = parser.add_subparsers(dest='command', required=True)
    for name in ('wszystkie', 'bohater', 'druzyna'):
        command = sub.add_parser(name)
        command.add_argument('--proby', type=positive_int, default=3, help='Liczba identycznych seedów map dla każdego składu (domyślnie 3)')
        command.add_argument('--seed', type=int, default=0)
        command.add_argument('--wieze', type=positive_int, default=10, help='Liczba wież (domyślnie 10; maksymalnie 50)')
        command.add_argument('--uklad', choices=('losowy', 'rowny', 'skupiska'), default='losowy')
        command.add_argument('--krok', type=float, choices=(.25, .5, 1), default=.5, help='Krok czasu w sekundach')
        command.add_argument('--atak', action='store_true', help='Włącz proponowany automatyczny atak podstawowy')
        command.add_argument('--csv', type=Path, help='Zapisz pełne wyniki do CSV')
        if name == 'bohater':
            command.add_argument('id')
        if name == 'druzyna':
            command.add_argument('id', nargs='+')
    sub.add_parser('pokrycie')
    args = parser.parse_args(argv)
    roster = load_roster()
    if args.command == 'pokrycie':
        for hero in roster.values():
            print(f"{hero['id']}: model={','.join(sorted(IMPLEMENTED.get(hero['id'], ()))) or 'brak'}; poza modelem="
                  f"{','.join(sorted({'Q','E','R'}-IMPLEMENTED.get(hero['id'], set()))) or 'brak'}; pasywka i pet: poza modelem")
        return 0
    if args.wieze > 50:
        parser.error('Liczba wież nie może przekraczać 50.')
    ids = [args.id] if args.command == 'bohater' else args.id if args.command == 'druzyna' else None
    if ids is not None:
        unknown = sorted(set(ids)-set(roster))
        if unknown or len(ids) != len(set(ids)) or len(ids) > 5:
            parser.error(f'Nieprawidłowy skład: nieznane={unknown}, bez duplikatów, 1–5 osób.')
        teams = [[roster[i] for i in ids]]
    else:
        teams = itertools.combinations(roster.values(), 5)
    seeds = list(range(args.seed, args.seed+args.proby))
    results = [evaluate(team, seeds, towers_count=args.wieze, step=args.krok,
                        layout=args.uklad, basic_attack=args.atak) for team in teams]
    # Czas szybszej porażki nie może poprawiać pozycji w rankingu.
    results.sort(key=lambda x: (-x['mean_arrived'], -x['all_arrived_pct'],
                                x['mean_time_s'] if x['all_arrived_pct'] == 100 else 0,
                                x['members']))
    digest = hashlib.sha256()
    for path in sorted(DATA.glob('*.json')):
        digest.update(path.name.encode())
        digest.update(path.read_bytes())
    print('MODEL 1D — NIE wynik Unity ani ocena pełnych zestawów Q/E/R')
    print(f'Wieże: {args.wieze}; układ: {args.uklad}; krok: {args.krok} s; atak podstawowy: {"tak" if args.atak else "nie"}')
    print(f"Składów: {len(results)}; prób na skład: {len(seeds)}; seedy: {seeds}; SHA256 danych: {digest.hexdigest()}")
    for i, result in enumerate(results[:10], 1):
        print(f"{i:2}. {result['members']}: średnio {result['mean_arrived']:.2f} na mecie, "
              f"wszyscy {result['all_arrived_pct']:.1f}%, czas {result['mean_time_s']:.1f} s")
    if args.csv:
        args.csv.parent.mkdir(parents=True, exist_ok=True)
        with args.csv.open('w', newline='', encoding='utf-8-sig') as f:
            writer = csv.DictWriter(f, fieldnames=['members', 'runs', 'mean_arrived', 'all_arrived_pct', 'mean_time_s'])
            writer.writeheader()
            writer.writerows(results)
        metadata = {'model': '1d-v2', 'data_sha256': digest.hexdigest(),
                    'towers_count': args.wieze, 'layout': args.uklad, 'step_s': args.krok,
                    'basic_attack': args.atak,
                    'seeds': seeds, 'team_count': len(results), 'coverage':
                    {hero: sorted(IMPLEMENTED.get(hero, ())) for hero in roster}}
        metadata_path = Path(str(args.csv)+'.meta.json')
        metadata_path.write_text(json.dumps(metadata, ensure_ascii=False, indent=2)+'\n', encoding='utf-8')
        print(f'CSV: {args.csv}')
        print(f'Metadane: {metadata_path}')
    return 0


if __name__ == '__main__':
    sys.exit(main())
