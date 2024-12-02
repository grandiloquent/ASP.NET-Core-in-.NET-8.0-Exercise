import bpy
import bmesh
verts=[e for e in bmesh.from_edit_mesh(bpy.context.selected_objects[0].data).verts if e.select]

co1=bpy.context.selected_objects[0].matrix_world @ verts[0].co
co2=bpy.context.selected_objects[0].matrix_world @ verts[1].co
bm = bmesh.new()
v1 = bm.verts.new(co1)
v2 = bm.verts.new(co2)
bm.edges.new((v1,v2))
me = bpy.data.meshes.new("")
bm.to_mesh(me)
plane = bpy.data.objects.new("", me)
bpy.context.scene.collection.objects.link(plane)