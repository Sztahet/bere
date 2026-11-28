"""Generate a provisional Blask model with movable legs in a new scene."""
import bpy
from pathlib import Path
from mathutils import Vector

out = Path(__file__).resolve().parent
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)

def material(name, color, emission=0):
    m=bpy.data.materials.new(name);m.diffuse_color=(*color,1);m.use_nodes=True
    shader=m.node_tree.nodes.get('Principled BSDF');shader.inputs['Base Color'].default_value=(*color,1)
    shader.inputs['Roughness'].default_value=.7
    if emission:
        shader.inputs['Emission Color'].default_value=(*color,1);shader.inputs['Emission Strength'].default_value=emission
    return m
cream=material('Blask | jasne futro',(.76,.59,.45))
light=material('Blask | jasne akcenty',(.95,.83,.65))
gold=material('Blask | poroze',(.95,.59,.18),.25)
dark=material('Blask | oczy i nos',(.09,.055,.045))
parts=[]
legs=[]
def oval(name, location, scale, mat):
    bpy.ops.mesh.primitive_uv_sphere_add(segments=20,ring_count=10,location=location)
    obj=bpy.context.object;obj.name=name;obj.scale=scale
    bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);obj.data.materials.append(mat)
    for face in obj.data.polygons:face.use_smooth=True
    parts.append(obj);return obj
def branch(name, start, end, radius, mat):
    a,b=Vector(start),Vector(end);d=b-a
    bpy.ops.mesh.primitive_cone_add(vertices=10,radius1=radius,radius2=radius*.28,depth=d.length,location=(a+b)*.5)
    obj=bpy.context.object;obj.name=name;obj.rotation_euler=d.to_track_quat('Z','Y').to_euler()
    bpy.ops.object.transform_apply(location=False,rotation=True,scale=True)
    obj.data.materials.append(mat);parts.append(obj);return obj

# Forward in Blender is -Y. This is a low-detail blockout, not a final rig.
oval('Tulow',(0,0,.80),(.24,.55,.26),cream)
oval('Klatka',(0,-.29,.86),(.23,.26,.28),light)
branch('Szyja',(0,-.30,.83),(0,-.53,1.25),.13,cream)
oval('Glowa',(0,-.59,1.25),(.16,.19,.15),cream)
oval('Pysk',(0,-.75,1.20),(.10,.15,.08),light)
oval('Nos',(0,-.88,1.23),(.055,.035,.035),dark)
oval('Ogon',(0,.51,.94),(.09,.16,.09),light)
for side in (-1,1):
    oval('Ucho',(side*.17,-.54,1.36),(.12,.09,.045),cream)
    oval('Oko',(side*.125,-.70,1.29),(.023,.018,.024),dark)
    for y in (-.29,.31):
        leg=branch('Noga',(side*.15,y,.72),(side*.16,y+.04,.12),.065,cream)
        hoof=oval('Kopyto',(side*.16,y-.015,.07),(.07,.13,.06),dark)
        legs.append((leg,hoof,side,y))
    branch('Poroze',(side*.09,-.55,1.37),(side*.18,-.52,1.70),.035,gold)
    branch('Galaz',(side*.15,-.53,1.54),(side*.30,-.48,1.68),.022,gold)
    branch('Czubek',(side*.18,-.52,1.70),(side*.28,-.49,1.80),.018,gold)
    branch('Wzor grzbietu',(side*.17,-.20,.99),(side*.20,.30,.98),.012,gold)

# Keep limbs separate with shoulder/hip pivots for Unity's shared gait.
limb_parts={piece for leg,hoof,_,_ in legs for piece in (leg,hoof)}
bpy.ops.object.select_all(action='DESELECT')
for obj in parts:
    if obj not in limb_parts:obj.select_set(True)
bpy.context.view_layer.objects.active=parts[0];bpy.ops.object.join()
body=bpy.context.object;body.name='Blask_Cialo'
limbs=[]
for leg,hoof,side,y in legs:
    bpy.ops.object.select_all(action='DESELECT');leg.select_set(True);hoof.select_set(True)
    bpy.context.view_layer.objects.active=leg;bpy.ops.object.join()
    leg.name='Blask_'+('Przod' if y<0 else 'Tyl')+'_'+('L' if side<0 else 'R')
    bpy.context.scene.cursor.location=(side*.15,y,.72);bpy.ops.object.origin_set(type='ORIGIN_CURSOR')
    limbs.append(leg)
bpy.ops.object.empty_add(type='PLAIN_AXES',location=(0,0,0));obj=bpy.context.object;obj.name='Blask_PoC'
for part in [body]+limbs:
    world=part.matrix_world.copy();part.parent=obj;part.matrix_world=world
bpy.ops.object.select_all(action='DESELECT')
for part in [obj,body]+limbs:part.select_set(True)
bpy.context.view_layer.objects.active=obj
bpy.ops.wm.save_as_mainfile(filepath=str(out/'Blask_PoC.blend'),compress=True)
bpy.ops.export_scene.fbx(filepath=str(out/'Blask_PoC.fbx'),use_selection=True,object_types={'MESH','EMPTY'},bake_anim=False,axis_forward='-Z',axis_up='Y')
print('BLASK vertices:',sum(len(part.data.vertices) for part in [body]+limbs))
