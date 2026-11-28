"""Build provisional Sarna or Lumen on the shared female rig.

Run Blender with Riftoryn_Female_Base.blend, then this script and -- Sarna
or -- Lumen. The existing Pyrra builder provides mesh and export helpers.
"""
import sys
from pathlib import Path

root = Path(__file__).resolve().parents[2]
name = sys.argv[sys.argv.index('--') + 1] if '--' in sys.argv else None
if name not in ('Sarna', 'Lumen'):
    raise ValueError('Pass -- Sarna or -- Lumen')

source = (root / 'assets/characters/pyrra/build_pyrra.py').read_text(encoding='utf-8')
source = source.replace("ROOT=r'D:\\github\\bere'", 'ROOT=' + repr(str(root))).replace('Pyrra', name)
palettes = {
    'Sarna': ['(.68,.60,.43)', '(.25,.34,.10)', '(.19,.105,.06)', '(.68,.43,.10)', '(.55,.68,.13)', '(.13,.065,.035)', '(.61,.35,.23)', '(.20,.11,.055)'],
    'Lumen': ['(.77,.68,.61)', '(.26,.12,.29)', '(.14,.07,.18)', '(.78,.53,.17)', '(1,.68,.27)', '(.12,.055,.105)', '(.52,.29,.21)', '(.18,.09,.12)'],
}
original = ['(.065,.055,.05)', '(.38,.045,.025)', '(.045,.025,.02)', '(.63,.34,.09)', '(1,.24,.025)', '(.065,.035,.03)', '(.58,.32,.22)', '(.09,.045,.025)']
for old, new in zip(original, palettes[name]):
    source = source.replace(old, new)

common = '''
# Pale layered mantle inspired by the modelling sheet.
panel('mantle_left',[(-.15,.09,1.43),(0,.16,1.31),(0,.235,.69),(-.20,.22,.54),(-.29,.15,.75)],navy,'DEF-spine.003')
panel('mantle_right',[(.15,.09,1.43),(0,.16,1.31),(0,.235,.69),(.20,.22,.54),(.29,.15,.75)],navy,'DEF-spine.003')
panel('mantle_trim_left',[(-.15,.075,1.43),(0,.16,1.30),(0,.17,1.26),(-.17,.09,1.40)],trim,'DEF-spine.003')
panel('mantle_trim_right',[(.15,.075,1.43),(0,.16,1.30),(0,.17,1.26),(.17,.09,1.40)],trim,'DEF-spine.003')
'''
sarna = '''
# Long braid, leaf pins and a wooden staff distinguish Sarna's silhouette.
for i in range(8):
 ellipsoid('braid_'+str(i),(-.085+(.012 if i%2 else -.012),.07,1.61-i*.047),(.038,.032,.045),hair,'DEF-spine.006',12,6)
panel('hair_leaf_left',[(-.04,0,1.72),(-.14,.02,1.78),(-.10,.03,1.68)],ice,'DEF-spine.006',.012)
panel('hair_leaf_right',[(-.04,0,1.72),(-.08,-.01,1.81),(-.07,.03,1.70)],ice,'DEF-spine.006',.012)
ellipsoid('staff',(.40,.015,.87),(.018,.018,.70),boots,'DEF-hand.L',12,6)
ellipsoid('staff_seed',(.40,.015,1.57),(.055,.035,.065),ice,'DEF-hand.L',12,6)
'''
lumen = '''
# Raised dark bun and golden crystal details from Lumen's reference.
ellipsoid('hair_bun',(0,.065,1.72),(.085,.075,.070),hair,'DEF-spine.006',20,10)
panel('gold_hairpin',[(-.12,.02,1.74),(.12,.02,1.75),(.12,.03,1.765),(-.12,.03,1.755)],trim,'DEF-spine.006')
crystal('earring_L',(.093,-.015,1.57),(.012,.01,.035),'DEF-spine.006')
crystal('earring_R',(-.093,-.015,1.57),(.012,.01,.035),'DEF-spine.006')
'''
marker = '# Eksport obejmuje jedną siatkę i szkielet; poszczególne części zachowują wagi.'
assert marker in source
source = source.replace(marker, common + (sarna if name == 'Sarna' else lumen) + '\n' + marker)
exec(compile(source, str(__file__), 'exec'))
