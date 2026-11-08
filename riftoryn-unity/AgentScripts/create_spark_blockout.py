import bpy, math
from mathutils import Vector
bpy.ops.object.select_all(action='SELECT'); bpy.ops.object.delete(use_global=False)
scene=bpy.context.scene; scene.unit_settings.system='METRIC'
def mat(name,c):
 m=bpy.data.materials.new(name);m.diffuse_color=(*c,1);return m
orange=mat('Futro — rudy',(.63,.17,.055)); cream=mat('Futro — jasne',(.91,.72,.51)); dark=mat('Łapy i uszy',(.18,.065,.045)); eye=mat('Oczy',(.035,.018,.015)); fire=mat('Ogon — żar',(1,.39,.045)); neutral=mat('Skala postaci',(.32,.34,.40)); ground=mat('Podłoże',(.12,.095,.15))
fox=[]
def uv(name,loc,scale,material):
 bpy.ops.mesh.primitive_uv_sphere_add(segments=16,ring_count=8,location=loc);o=bpy.context.object;o.name=name;o.scale=scale;o.data.materials.append(material);fox.append(o);return o
def cone(name,loc,rad,depth,material,rotation=(0,0,0)):
 bpy.ops.mesh.primitive_cone_add(vertices=8,radius1=rad,radius2=.005,depth=depth,location=loc,rotation=rotation);o=bpy.context.object;o.name=name;o.data.materials.append(material);fox.append(o);return o
# Coordinates in metres: fox shoulders ~0.30 m, ears extend above the shoulders.
uv('Tułów',(0.40,0,.235),(.245,.105,.12),orange)
uv('Pierś',(0.22,0,.245),(.13,.105,.15),cream)
uv('Głowa',(0.15,0,.36),(.115,.105,.105),orange)
uv('Pysk',(.045,0,.33),(.085,.065,.045),cream)
uv('Nos',(-.032,0,.34),(.018,.022,.014),dark)
for y in (-.072,.072):
 cone('Ucho',(0.16,y,.475),.05,.16,orange)
 for x in (.24,.55):
  uv('Łapa',(x,y,.105),(.035,.035,.11),dark)
  uv('Stopa',(x-.018,y,.025),(.055,.04,.025),dark)
uv('Oko L',(.10,-.089,.382),(.015,.009,.014),eye)
uv('Oko P',(.10,.089,.382),(.015,.009,.014),eye)
tail=uv('Puszysty ogon',(.72,0,.29),(.27,.125,.14),fire);tail.rotation_euler[1]=-.4
tip=uv('Jasny koniec ogona',(.91,0,.39),(.10,.095,.105),cream);tip.rotation_euler[1]=-.4
# Distinct collection lets the model be exported without the scale mannequin.
collection=bpy.data.collections.new('Iskra Spark — model');scene.collection.children.link(collection)
for obj in fox:
 for c in list(obj.users_collection): c.objects.unlink(obj)
 collection.objects.link(obj)
# Unity trial uses a standard 2 m capsule (local scale Y = 1).
bpy.ops.mesh.primitive_uv_sphere_add(segments=20,ring_count=12,location=(-.65,0,1.0));human=bpy.context.object;human.name='Kapsuła gracza Unity — 2.00 m';human.scale=(.425,.425,1.0);human.data.materials.append(neutral)
bpy.ops.mesh.primitive_plane_add(size=6,location=(0,0,-.005));bpy.context.object.data.materials.append(ground)
def label(body,loc,size):
 bpy.ops.object.text_add(location=loc,rotation=(math.pi/2,0,0));o=bpy.context.object;o.data.body=body;o.data.size=size;o.data.align_x='CENTER';o.data.materials.append(cream)
label('KAPSULA GRACZA 2.00 m',(-.65,-.01,2.13),.12)
label('ISKRA / SPARK',(.42,-.01,.64),.10)
bpy.ops.object.camera_add(location=(-2.7,-5.8,2.5));cam=bpy.context.object;cam.rotation_euler=(Vector((-.05,0,1.0))-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO';cam.data.ortho_scale=3.5;scene.camera=cam
scene.render.engine='BLENDER_WORKBENCH';scene.display.shading.light='STUDIO';scene.display.shading.color_type='MATERIAL';scene.display.shading.show_shadows=True;scene.display.shading.show_cavity=True;scene.display.shading.background_type='WORLD';scene.world.color=(.055,.04,.075)
scene.render.resolution_x=1200;scene.render.resolution_y=900;scene.render.resolution_percentage=100
bpy.ops.wm.save_as_mainfile(filepath=r'D:\github\bere\assets\iskra-spark-blockout.blend')
scene.render.filepath=r'D:\github\bere\assets\iskra-spark-skala.png';bpy.ops.render.render(write_still=True)
# Export only the fox to a portable scene file.
bpy.ops.object.select_all(action='DESELECT')
for obj in fox: obj.select_set(True)
bpy.context.view_layer.objects.active=fox[0]
bpy.ops.wm.obj_export(filepath=r'D:\github\bere\assets\iskra-spark-blockout.obj',export_selected_objects=True)
print('MODEL_CREATED',len(fox))
