o = bpy.data.collections.new("MyCollection")
bpy.context.scene.collection.children.link(o)
bpy.context.view_layer.active_layer_collection = bpy.context.view_layer.layer_collection.children[o.name]
cam1 = bpy.data.cameras.new("Camera")
obj = bpy.data.objects.new("Camera", cam1)
o.objects.link(obj)
bpy.context.scene.camera = obj

win = bpy.context.window
scr = win.screen
areas3d  = [area for area in scr.areas if area.type == 'VIEW_3D']
region   = [region for region in areas3d[0].regions if region.type == 'WINDOW']


# Add cube in the other window.
with bpy.context.temp_override(window=win,area=areas3d[0],region=region[0]):
    bpy.ops.view3d.camera_to_view()
import bmesh
bpy.ops.mesh.primitive_plane_add()
bpy.ops.transform.resize(value=(20,20,20))
bpy.ops.object.editmode_toggle()
bpy.ops.mesh.select_mode(use_extend=False, use_expand=False, type='EDGE')
bpy.ops.mesh.select_all(action='DESELECT')
bm = bmesh.from_edit_mesh(bpy.context.object.data)
bm.edges.ensure_lookup_table()
bm.edges[0].select = True
bmesh.update_edit_mesh(bpy.context.object.data)
bpy.ops.mesh.extrude_region_move(TRANSFORM_OT_translate={"value":(0,0,20)})
bpy.ops.mesh.select_all(action='DESELECT')
bm.edges.ensure_lookup_table()
bm.edges[0].select = True
bmesh.update_edit_mesh(bpy.context.object.data)
bpy.ops.mesh.bevel(offset=0.4, segments=15, affect='EDGES')
bpy.ops.object.editmode_toggle()
bpy.ops.object.subdivision_set(level=2, relative=False)
bpy.ops.object.shade_smooth()
mat = bpy.data.materials.new(name="Material")
bpy.context.object.data.materials.append(mat)
mat.use_nodes = True
mat.node_tree.nodes["Principled BSDF"].inputs[0].default_value = (0.0722839, 0.109457, 0.109454, 1)
mat.node_tree.nodes["Principled BSDF"].inputs[2].default_value = 0.897

light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
light_data.energy = 500
light_data.shape = 'DISK'
light_data.size = 6
light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
bpy.context.collection.objects.link(light_object)
light_object.location[2] = 5

light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
light_data.energy = 200
light_data.shape = 'DISK'
light_data.size = 6
light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
bpy.context.collection.objects.link(light_object)
light_object.rotation_euler[0] = 1.39
light_object.rotation_euler[1] = 0.785398

light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
light_data.energy = 200
light_data.shape = 'DISK'
light_data.size = 6
light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
bpy.context.collection.objects.link(light_object)
light_object.rotation_euler[0] = 0.785398
light_object.rotation_euler[1] = -1.5708

bpy.context.scene.render.engine = 'CYCLES'
bpy.context.scene.cycles.device = 'GPU'
bpy.context.scene.cycles.preview_samples = 64
bpy.context.scene.cycles.use_preview_denoising = True
bpy.context.scene.cycles.samples = 1024
bpy.context.scene.view_settings.view_transform = 'Filmic'
bpy.context.scene.view_settings.view_transform = 'Filmic'
bpy.context.scene.view_settings.exposure = 0.1
bpy.context.scene.render.image_settings.file_format = 'PNG'