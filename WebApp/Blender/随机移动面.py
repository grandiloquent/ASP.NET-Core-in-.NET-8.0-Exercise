import bpy
import bmesh
from random import uniform
bpy.ops.object.mode_set(mode='EDIT')
ob = bpy.context.object
me = ob.data
bm = bmesh.from_edit_mesh(me)
faces = bm.faces
for i in range(21,28,2):
  verts = set(v for f in faces[i:i+2] for v in f.verts)
  offset = uniform(-0.1,0.1)
  for v in verts:
    v.co.y+=offset

bmesh.update_edit_mesh(me)

# import bmesh
# ob = bpy.context.object
# me = ob.data
# bm = bmesh.from_edit_mesh(me)
# faces = [f for f in bm.faces if f.select]
# for f in faces:
#   print(f.index)