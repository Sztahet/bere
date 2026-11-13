ROOT=r'D:\github\bere'
CHARACTER='Pyrra'
import bpy, math, os
from mathutils import Vector
rig=bpy.data.objects['Saila_Female_Rig']
body=bpy.data.collections['BodyChan']
parts=[o for o in body.all_objects if o.type=='MESH']

def mat(name,color,metal=0,emission=0):
 m=bpy.data.materials.get(name) or bpy.data.materials.new(name);m.diffuse_color=(*color,1);m.use_nodes=True
 bs=m.node_tree.nodes.get('Principled BSDF');bs.inputs['Base Color'].default_value=(*color,1);bs.inputs['Metallic'].default_value=metal;bs.inputs['Roughness'].default_value=.38 if metal else .7
 if emission:bs.inputs['Emission Color'].default_value=(*color,1);bs.inputs['Emission Strength'].default_value=emission
 return m
navy=mat('Pyrra | grafit',(.065,.055,.05));purple=mat('Pyrra | karmazyn',(.38,.045,.025));deep=mat('Pyrra | ciemna tkanina',(.045,.025,.02));trim=mat('Pyrra | złoty metal',(.63,.34,.09),.65);ice=mat('Pyrra | żar',(1,.24,.025),.2,.35);hair=mat('Pyrra | ciemne włosy',(.065,.035,.03));skin=mat('Pyrra | skóra',(.58,.32,.22));boots=mat('Pyrra | buty',(.09,.045,.025))
for o in parts:
 if o.name in ['Head','Neck']: m=skin
 elif o.name in ['Feet','Shins']:m=boots
 elif o.name in ['Hands','Forearms'] or o.name.lower().startswith(('index_','mid_','ring_','pinky_','thumb_')):m=deep
 elif o.name in ['Thighs','Hips']:m=deep
 else:m=navy
 o.data.materials.clear();o.data.materials.append(m)
new=[]
def bind(obj,bone):
 obj.name='Pyrra_'+obj.name
 obj.data.materials.clear()
 matx=obj.get('_material')
 obj.data.materials.append(bpy.data.materials[matx]);del obj['_material']
 v=obj.vertex_groups.new(name=bone);v.add(list(range(len(obj.data.vertices))),1,'REPLACE')
 mw=obj.matrix_world.copy();obj.parent=rig;obj.matrix_world=mw
 mod=obj.modifiers.new('Szkielet','ARMATURE');mod.object=rig
 new.append(obj);return obj

def ellipsoid(name,loc,scale,material,bone,segments=16,rings=8):
 bpy.ops.mesh.primitive_uv_sphere_add(segments=segments,ring_count=rings,location=loc);o=bpy.context.object;o.name=name;o.scale=scale
 bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o['_material']=material.name;return bind(o,bone)

def mesh(name,verts,faces,material,bone):
 data=bpy.data.meshes.new(name);data.from_pydata(verts,[],faces);data.update();o=bpy.data.objects.new(name,data);bpy.context.scene.collection.objects.link(o);o['_material']=material.name;return bind(o,bone)

def panel(name,coords,material,bone,depth=.008):
 verts=coords+[(x,y+depth,z) for x,y,z in coords];n=len(coords);faces=[tuple(range(n-1,-1,-1)),tuple(range(n,n*2))]+[(i,(i+1)%n,(i+1)%n+n,i+n) for i in range(n)];return mesh(name,verts,faces,material,bone)

# Mankiety, wysoki kołnierz i pas.
ellipsoid('collar',(0,-.012,1.45),(.14,.095,.065),purple,'DEF-spine.004')
ellipsoid('collar_trim',(0,-.013,1.497),(.13,.09,.018),trim,'DEF-spine.004')
ellipsoid('belt',(0,-.045,1.065),(.14,.105,.045),trim,'DEF-spine.001')
ellipsoid('belt_center',(0,-.154,1.07),(.037,.012,.037),ice,'DEF-spine.001',12,6)
# Wielowarstwowa tunika i płaszcz pozostawiają nogom miejsce do ruchu.
panel('front_tabard',[(-.13,-.15,1.09),(.13,-.15,1.09),(.17,-.13,.65),(.04,-.15,.58),(0,-.155,.72),(-.07,-.15,.60),(-.17,-.13,.68)],purple,'DEF-spine')
panel('front_tabard_inset',[(-.075,-.163,1.07),(.075,-.163,1.07),(.10,-.15,.76),(0,-.17,.64),(-.10,-.15,.76)],navy,'DEF-spine',.006)
panel('cape_left',[(-.15,.08,1.43),(-.04,.13,1.45),(-.02,.24,.48),(-.19,.23,.39),(-.28,.16,.58)],purple,'DEF-spine.003')
panel('cape_right',[(.04,.13,1.45),(.15,.08,1.43),(.28,.16,.58),(.19,.23,.39),(.02,.24,.48)],navy,'DEF-spine.003')
panel('cape_mark',[(-.055,.245,.85),(0,.252,.91),(.055,.245,.85),(0,.25,.78)],trim,'DEF-spine.003')
# Naramienniki: metalowa podstawa i asymetryczne odłamki lodu.
ellipsoid('pauldron_L',(.16,.005,1.41),(.085,.068,.035),trim,'DEF-shoulder.L')
ellipsoid('pauldron_R',(-.16,.005,1.41),(.073,.065,.032),purple,'DEF-shoulder.R')
def crystal(name,center,scale,bone):
 x,y,z=center; sx,sy,sz=scale
 vs=[(x-sx,y,z),(x,y-sy,z),(x+sx,y,z),(x,y+sy,z),(x,y,z+sz),(x,y,z-sz*.35)]
 return mesh(name,vs,[(0,1,4),(1,2,4),(2,3,4),(3,0,4),(1,0,5),(2,1,5),(3,2,5),(0,3,5)],ice,bone)
crystal('chest_gem',(0,-.172,1.35),(.025,.017,.035),'DEF-spine.003')
# Zwarta bryła krótkich jasnych włosów i pojedyncze pasma.
ellipsoid('hair_cap',(0,-.015,1.664),(.092,.095,.074),hair,'DEF-spine.006',20,10)
for i in range(7):
 x=-.072+i*.024
 panel('hair_fringe_'+str(i),[(x-.018,-.085,1.72),(x+.016,-.08,1.72),(x+.025,-.125,1.62-(i%3)*.012)],hair,'DEF-spine.006',.012)
for i,x in enumerate([-.08,-.035,.02,.065]):
 panel('hair_spike_'+str(i),[(x-.026,.0,1.70),(x+.015,.035,1.69),(x+.03,.01,1.745-(i%2)*.008)],hair,'DEF-spine.006',.015)
# Drobne oczy nadają twarzy czytelny punkt skupienia.
for sign,side in [(1,'L'),(-1,'R')]:
 ellipsoid('eye_'+side,(sign*.031,-.145,1.623),(.013,.006,.007),deep,'DEF-spine.006',12,6)
 ellipsoid('iris_'+side,(sign*.031,-.151,1.623),(.004,.003,.005),trim,'DEF-spine.006',10,6)

# Wyraziste opaski na rękawicach i butach.
for sign,side in [(1,'L'),(-1,'R')]:
 ellipsoid('bracer_'+side,(sign*.34,.03,1.13),(.045,.037,.058),purple,'DEF-forearm.'+side)
 ellipsoid('boot_trim_'+side,(sign*.08,.015,.22),(.045,.052,.012),trim,'DEF-shin.'+side+'.001')


panel('scarf_tail',[(.055,.12,1.45),(.16,.11,1.43),(.25,.23,.93),(.15,.25,.88)],purple,'DEF-spine.003')
panel('scarf_tail_gold',[(.15,.25,.88),(.25,.23,.93),(.24,.23,.96),(.155,.25,.91)],trim,'DEF-spine.003')
panel('copper_fringe',[(-.03,-.099,1.72),(-.012,-.102,1.72),(.025,-.126,1.62)],trim,'DEF-spine.006')
panel('belt_strap',[(-.11,-.15,1.39),(-.085,-.16,1.41),(.11,-.15,1.13),(.085,-.16,1.12)],trim,'DEF-spine.003')
ellipsoid('pouch',(.13,-.08,1.03),(.045,.035,.065),boots,'DEF-spine.001')

# Zabudowane buty zakrywają modelowe palce i spinają sylwetkę.
for sign,side in [(1,'L'),(-1,'R')]:
 ellipsoid('boot_foot_'+side,(sign*.08,-.035,.07),(.050,.105,.065),boots,'DEF-foot.'+side,20,10)

# Eksport obejmuje jedną siatkę i szkielet; poszczególne części zachowują wagi.
bpy.ops.object.select_all(action='DESELECT')
for o in parts+new:o.select_set(True)
bpy.context.view_layer.objects.active=parts[0]
bpy.ops.object.join()
joined=bpy.context.object
joined.name=CHARACTER+'_BodyAndOutfit'
bpy.ops.object.select_all(action='DESELECT')
joined.select_set(True);rig.select_set(True);bpy.context.view_layer.objects.active=rig
out=os.path.join(ROOT,'assets','characters',CHARACTER.lower());os.makedirs(out,exist_ok=True)
blend=os.path.join(out,CHARACTER+'_PoC.blend');fbx=os.path.join(out,CHARACTER+'_PoC.fbx')
bpy.ops.wm.save_as_mainfile(filepath=blend,compress=True,copy=True)
bpy.ops.export_scene.fbx(filepath=fbx,use_selection=True,object_types={'ARMATURE','MESH'},use_armature_deform_only=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',apply_unit_scale=True)
print('EXPORT',fbx,'verts',len(joined.data.vertices),'materials',len(joined.data.materials))
