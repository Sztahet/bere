# Run in Blender with premium_elemental_base_towers.blend open; exports the seven tower material definitions for Unity.
import bpy,json
from pathlib import Path
entries={}
for element in ('Fire','Ice','Lightning','Nature','Earth','Light','Darkness'):
 for obj in bpy.data.scenes['Premium Tower | '+element].objects:
  if obj.type!='MESH' or not obj.name.startswith('Tower_Premium_'):continue
  for slot in obj.material_slots:
   m=slot.material
   if not m or not m.use_nodes:continue
   p=next((n for n in m.node_tree.nodes if n.type=='BSDF_PRINCIPLED'),None)
   e=next((n for n in m.node_tree.nodes if n.type=='EMISSION'),None)
   if p:
    col=p.inputs['Emission Color'].default_value;strength=p.inputs['Emission Strength'].default_value
    data={'metallic':p.inputs['Metallic'].default_value,'smoothness':1-p.inputs['Roughness'].default_value,'emission':[col[i]*strength for i in range(3)],'unlit':False}
   elif e:
    col=e.inputs['Color'].default_value;strength=e.inputs['Strength'].default_value
    data={'metallic':0,'smoothness':0,'emission':[col[i]*strength for i in range(3)],'unlit':True}
   else:continue
   entries[m.name.replace('|','_')]=dict(name=m.name.replace('|','_'),**data)
output=Path(__file__).resolve().parents[1] / 'Assets' / 'Resources' / 'Towers' / 'premium-materials.json'
output.write_text(json.dumps({'materials':list(entries.values())},ensure_ascii=False,indent=2),encoding='utf-8')
print('WROTE',len(entries),output)
