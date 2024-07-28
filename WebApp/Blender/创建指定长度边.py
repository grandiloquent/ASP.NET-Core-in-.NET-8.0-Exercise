import bpy
import bmesh
import mathutils
activeObject=bpy.context.object;

offset = 1.2422/2

location = bpy.context.scene.cursor.location
bm = bmesh.new()
vert1=bm.verts.new(location)
vert2=bm.verts.new(mathutils.Vector((location.x, location.y, location.z-offset)))
edge=bm.edges.new([vert1,vert2])
me = bpy.data.meshes.new(name='Edge')
ob = bpy.data.objects.new(name='Edge', object_data=me)
bm.to_mesh(ob.data)
bpy.context.collection.objects.link(ob)