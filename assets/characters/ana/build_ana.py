"""Build Ana's provisional model using the project's existing humanoid base.

Run with Blender on Riftoryn_Female_Base.blend. The shared modelling helpers
come from Pyrra's script; this changes the palette and adds Ana's silhouette.
"""
from pathlib import Path

root = Path(__file__).resolve().parents[3]
source = (root / 'assets/characters/pyrra/build_pyrra.py').read_text(encoding='utf-8')
source = source.replace("ROOT=r'D:\\github\\bere'", 'ROOT=' + repr(str(root)))
source = source.replace('Pyrra', 'Ana')
source = source.replace("(.065,.055,.05)", "(.13,.18,.065)")
source = source.replace("(.38,.045,.025)", "(.19,.27,.065)")
source = source.replace("(.045,.025,.02)", "(.11,.065,.035)")
source = source.replace("(.63,.34,.09)", "(.72,.48,.09)")
source = source.replace("(1,.24,.025)", "(.55,.67,.08)")
source = source.replace("(.065,.035,.03)", "(.53,.19,.055)")
source = source.replace("(.58,.32,.22)", "(.72,.43,.29)")
source = source.replace("(.09,.045,.025)", "(.19,.09,.035)")
marker = '# Eksport obejmuje jedną siatkę i szkielet; poszczególne części zachowują wagi.'
extra = '''
# Długie rude włosy, liście i szeroki zielony płaszcz z arkusza Any.
ellipsoid('long_hair_back',(0,.075,1.52),(.105,.065,.17),hair,'DEF-spine.006')
for i, x in enumerate([-.075,-.04,0,.04,.075]):
 panel('long_hair_lock_'+str(i),[(x-.018,.105,1.61),(x+.018,.105,1.61),(x+.025,.135,1.37-(i%2)*.035),(x,.14,1.32-(i%2)*.025)],hair,'DEF-spine.006',.012)
panel('leaf_hair_left',[(-.06,.0,1.72),(-.17,.025,1.79),(-.11,.045,1.68)],ice,'DEF-spine.006',.012)
panel('leaf_hair_right',[(-.06,.0,1.72),(-.13,-.01,1.83),(-.09,.035,1.70)],trim,'DEF-spine.006',.012)
panel('cloak_gold_chevron_left',[(-.15,.09,1.43),(0,.145,1.30),(0,.155,1.265),(-.17,.105,1.40)],trim,'DEF-spine.003')
panel('cloak_gold_chevron_right',[(.15,.09,1.43),(0,.145,1.30),(0,.155,1.265),(.17,.105,1.40)],trim,'DEF-spine.003')
'''
assert marker in source
source = source.replace(marker, extra + '\n' + marker)
exec(compile(source, str(__file__), 'exec'))
