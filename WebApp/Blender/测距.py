import bpy
import bmesh
co1=bpy.context.selected_objects[0].matrix_world @ [e for e in bmesh.from_edit_mesh(bpy.context.selected_objects[0].data).verts if e.select][0].co
co2=bpy.context.selected_objects[1].matrix_world @ [e for e in bmesh.from_edit_mesh(bpy.context.selected_objects[1].data).verts if e.select][0].co
print((co1.x-co2.x, co1.y-co2.y, co1.z-co2.z))
"""
import bpy
import bmesh
co=bpy.context.object.matrix_world @ [e for e in bmesh.from_edit_mesh(bpy.context.object.data).verts if e.select][0].co
bm = bmesh.new()
v1 = bm.verts.new((co.x, co.y, co.z))
v2 = bm.verts.new((co.x, co.y+0.5, co.z))
bm.edges.new((v1,v2))
me = bpy.data.meshes.new("")
bm.to_mesh(me)
plane = bpy.data.objects.new("", me)
bpy.context.scene.collection.objects.link(plane)
"""