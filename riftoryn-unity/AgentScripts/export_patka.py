import bpy
bpy.ops.wm.open_mainfile(filepath=r'D:\github\bere\assets\patka-animowana.blend')
s=bpy.data.scenes['RIFT_Patka_prosta']
bpy.context.window.scene=s
bpy.ops.object.select_all(action='DESELECT')
for o in s.objects:
 if o.type in {'MESH','ARMATURE'} and o.name!='test': o.select_set(True)
bpy.context.view_layer.objects.active=s.objects['Patka RIG']
bpy.ops.export_scene.fbx(filepath=r'D:\github\bere\riftoryn-unity\Assets\Resources\Pets\patka.fbx',use_selection=True,object_types={'ARMATURE','MESH'},add_leaf_bones=False,bake_anim=True,bake_anim_use_all_actions=True,bake_anim_use_nla_strips=False)
