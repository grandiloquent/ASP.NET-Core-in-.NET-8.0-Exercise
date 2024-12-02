import bmesh

me = bpy.context.object
bm = bmesh.from_edit_mesh(me.data)
mw  = me.matrix_world
verts = [v for v in bm.verts if v.select]

z = min(verts,key=lambda v:(mw@v.co).z)
o = 0.3996752202510834

zz=o-(z.co.z-l.z)
print(z.co.z)
print(zz)

bpy.ops.transform.translate(value=(0,0,zz))