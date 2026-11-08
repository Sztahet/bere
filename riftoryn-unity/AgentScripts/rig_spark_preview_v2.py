import bpy, math
from mathutils import Vector
bpy.ops.wm.open_mainfile(filepath=r'D:\github\bere\assets\iskra-spark-blockout.blend')
scene=bpy.context.scene
collection=bpy.data.collections['Iskra Spark — model']
fox=list(collection.objects)
# Slightly fuller tail and tilted ears improve the fox silhouette without changing its shoulder height.
bpy.data.objects['Puszysty ogon'].scale.y *= 1.16
bpy.data.objects['Ucho'].rotation_euler.x = -.12
bpy.data.objects['Ucho.001'].rotation_euler.x = .12
# Add pointed cheek tufts and a tapered, layered tail to strengthen the fox silhouette.
def tuft(name,location,scale,material,tilt=0):
 bpy.ops.mesh.primitive_cone_add(vertices=8,radius1=1,radius2=0,depth=2,location=location)
 obj=bpy.context.object;obj.name=name;obj.scale=scale;obj.rotation_euler[1]=tilt;obj.data.materials.append(material)
 for c in list(obj.users_collection):c.objects.unlink(obj)
 collection.objects.link(obj);fox.append(obj);return obj
orange=bpy.data.materials['Futro — rudy'];cream=bpy.data.materials['Futro — jasne'];fire=bpy.data.materials['Ogon — żar']
tuft('Policzek L',(.17,-.092,.31),(.045,.027,.07),cream,-.55)
tuft('Policzek P',(.17,.092,.31),(.045,.027,.07),cream,-.55)
tuft('Płomień ogona',(.87,0,.43),(.07,.07,.19),fire,.9)
# Skeleton in metres; each mesh is rigidly weighted to a bone for a simple blockout rig.
bpy.ops.object.armature_add(enter_editmode=True,location=(0,0,0));rig=bpy.context.object;rig.name='Spark_Rig';arm=rig.data;arm.name='Spark_Skeleton';arm.edit_bones.remove(arm.edit_bones[0])
def bone(name,head,tail,parent=None):
 b=arm.edit_bones.new(name);b.head=head;b.tail=tail
 if parent:b.parent=arm.edit_bones[parent]
 return b
bone('Root',(.40,0,.23),(.40,0,.38))
bone('Head',(.22,0,.30),(.10,0,.38),'Root')
bone('Tail',(.57,0,.26),(.82,0,.36),'Root')
for name,x,y in [('Front_L',.24,-.072),('Front_R',.24,.072),('Back_L',.55,-.072),('Back_R',.55,.072)]:bone(name,(x,y,.22),(x,y,.04),'Root')
bpy.ops.object.mode_set(mode='OBJECT')
for c in list(rig.users_collection):c.objects.unlink(rig)
collection.objects.link(rig)
for obj in fox:
 name=obj.name
 if name.startswith(('Głowa','Pysk','Nos','Ucho','Oko','Policzek')):target='Head'
 elif name.startswith(('Puszysty','Jasny','Płomień')):target='Tail'
 elif name.startswith(('Łapa','Stopa')):
  target=('Front_' if obj.location.x<.4 else 'Back_')+('L' if obj.location.y<0 else 'R')
 else:target='Root'
 vg=obj.vertex_groups.new(name=target);vg.add(list(range(len(obj.data.vertices))),1.0,'REPLACE');mod=obj.modifiers.new('Szkielet','ARMATURE');mod.object=rig;obj.parent=rig
rig.animation_data_create()
def action(name,frames):
 rig.animation_data.action=None
 for frame in frames:
  t=2*math.pi*(frame-1)/(frames[-1]-1)
  for b in rig.pose.bones:b.rotation_mode='XYZ';b.rotation_euler=(0,0,0);b.location=(0,0,0)
  if name=='Idle':
   rig.pose.bones['Root'].location.z=.006*math.sin(t);rig.pose.bones['Head'].rotation_euler.y=.035*math.sin(t);rig.pose.bones['Tail'].rotation_euler.z=.12*math.sin(t)
  elif name=='Run':
   rig.pose.bones['Root'].location.z=.012*(1-math.cos(2*t));rig.pose.bones['Root'].rotation_euler.y=.055*math.sin(2*t)
   rig.pose.bones['Head'].rotation_euler.y=-.045*math.sin(2*t+.4);rig.pose.bones['Tail'].rotation_euler.z=.20*math.sin(t-.5)
   for n,phase in [('Front_L',0),('Back_R',.35),('Front_R',math.pi),('Back_L',math.pi+.35)]:rig.pose.bones[n].rotation_euler.x=.62*math.sin(t+phase)
  else:
   # One-shot recoil: crouch and tuck, then settle back to the bind pose.
   strength={1:0,4:1,8:.60,13:.18,19:0}.get(frame,0)
   rig.pose.bones['Root'].location.z=-.035*strength;rig.pose.bones['Root'].rotation_euler.y=-.13*strength
   rig.pose.bones['Head'].rotation_euler.y=.20*strength;rig.pose.bones['Tail'].rotation_euler.z=-.30*strength
   for n in ('Front_L','Front_R','Back_L','Back_R'):rig.pose.bones[n].rotation_euler.x=-.18*strength
  for b in rig.pose.bones:b.keyframe_insert(data_path='rotation_euler',frame=frame);b.keyframe_insert(data_path='location',frame=frame)
 rig.animation_data.action.name=name
 return rig.animation_data.action
idle=action('Idle',[1,13,25,37,49]);run=action('Run',[1,4,7,10,13,16,19,22,25]);hit=action('Hit',[1,4,8,13,19]);rig.animation_data.action=idle
# Select only the model so the mannequin and camera never enter the FBX.
bpy.ops.object.select_all(action='DESELECT')
for obj in fox:obj.select_set(True)
rig.select_set(True);bpy.context.view_layer.objects.active=rig
scene.frame_start=1;scene.frame_end=49;scene.frame_set(1)
bpy.ops.wm.save_as_mainfile(filepath=r'D:\github\bere\assets\iskra-spark-rig-v2.blend')
bpy.ops.export_scene.fbx(filepath=r'D:\github\bere\riftoryn-unity\Assets\Resources\Pets\spark-preview.fbx',use_selection=True,object_types={'ARMATURE','MESH'},add_leaf_bones=False,bake_anim=True,bake_anim_use_all_actions=True,bake_anim_use_nla_strips=False,bake_anim_force_startend_keying=True)
scene.render.filepath=r'D:\github\bere\assets\iskra-spark-rig-v2-podglad.png';bpy.ops.render.render(write_still=True)
print('RIG_RESULT',len(fox),len(rig.pose.bones),idle.name,run.name,hit.name)
