import bpy
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.fbx(filepath=r'D:\github\bere\riftoryn-unity\Assets\Resources\Pets\spark-preview.fbx')
print('IMPORTED_ACTIONS', [(a.name, tuple(a.frame_range)) for a in bpy.data.actions])
print('IMPORTED_RIGS',[(o.name,len(o.data.bones)) for o in bpy.data.objects if o.type=='ARMATURE'])
print('IMPORTED_MESHES',len([o for o in bpy.data.objects if o.type=='MESH']))
