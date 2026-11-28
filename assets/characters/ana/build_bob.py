"""Create a small, editable, unrigged panda blockout for the Ana prototype."""
import bpy
import math
from pathlib import Path

root = Path(__file__).resolve().parents[3]
out = root / 'assets/characters/ana'
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)

def material(name, color):
    m = bpy.data.materials.new(name)
    m.diffuse_color = (*color, 1)
    m.use_nodes = True
    bs = m.node_tree.nodes.get('Principled BSDF')
    bs.inputs['Base Color'].default_value = (*color, 1)
    bs.inputs['Roughness'].default_value = .8
    return m

cream = material('Bob | kremowe futro', (.82, .71, .58))
dark = material('Bob | ciemnobrazowe futro', (.105, .065, .055))
black = material('Bob | nos i oczy', (.025, .016, .014))
ivory = material('Bob | blysk oka', (.95, .89, .76))
parts = []
legs = []

def oval(name, location, scale, mat, segments=24, rings=12):
    bpy.ops.mesh.primitive_uv_sphere_add(segments=segments, ring_count=rings, location=location)
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    obj.data.materials.append(mat)
    for face in obj.data.polygons:
        face.use_smooth = True
    parts.append(obj)
    return obj

# Forward is -Y in Blender. The panda is on four paws, with a rounded back.
oval('Tulow', (0, .03, .52), (.43, .68, .42), cream, 32, 16)
oval('Barki', (0, -.30, .54), (.44, .30, .40), dark, 32, 16)
oval('Glowa', (0, -.62, .66), (.33, .30, .30), cream, 32, 16)
for side in (-1, 1):
    oval('Ucho', (side*.255, -.60, .91), (.105, .095, .11), dark)
    side_name = 'L' if side < 0 else 'R'
    legs.append(oval('Bob_Przod_'+side_name, (side*.30, -.37, .22), (.15, .20, .22), dark))
    legs.append(oval('Bob_Tyl_'+side_name, (side*.30, .42, .20), (.16, .20, .20), dark))
    oval('Plama_oka', (side*.145, -.872, .70), (.104, .035, .105), dark)
    oval('Oko', (side*.145, -.904, .715), (.030, .018, .035), black, 16, 8)
    oval('Blysk', (side*.137, -.922, .729), (.010, .006, .010), ivory, 12, 6)
oval('Pysk', (0, -.91, .59), (.16, .12, .105), cream)
oval('Nos', (0, -1.02, .64), (.075, .044, .04), black)
oval('Ogon', (0, .68, .48), (.10, .12, .10), cream)

# Keep the four paws separate, with pivots near the shoulders/hips, so the
# same procedural gait can animate them in Unity without a full rig.
bpy.ops.object.select_all(action='DESELECT')
for obj in parts:
    if obj not in legs: obj.select_set(True)
bpy.context.view_layer.objects.active = parts[0]
bpy.ops.object.join()
body = bpy.context.object
body.name = 'Bob_Cialo'
bpy.ops.object.empty_add(type='PLAIN_AXES', location=(0, 0, 0))
bob = bpy.context.object
bob.name = 'Bob_PoC'
for obj in [body] + legs:
    if obj in legs:
        bpy.ops.object.select_all(action='DESELECT')
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        bpy.context.scene.cursor.location = (obj.location.x, obj.location.y, .38)
        bpy.ops.object.origin_set(type='ORIGIN_CURSOR')
    world = obj.matrix_world.copy()
    obj.parent = bob
    obj.matrix_world = world
bpy.ops.object.select_all(action='DESELECT')
for obj in [bob, body] + legs: obj.select_set(True)
bpy.context.view_layer.objects.active = bob
bpy.ops.wm.save_as_mainfile(filepath=str(out / 'Bob_PoC.blend'), compress=True)
bpy.ops.export_scene.fbx(filepath=str(out / 'Bob_PoC.fbx'), use_selection=True,
    object_types={'MESH', 'EMPTY'}, bake_anim=False, axis_forward='-Z', axis_up='Y')
print('BOB vertices:', sum(len(obj.data.vertices) for obj in [body] + legs))
