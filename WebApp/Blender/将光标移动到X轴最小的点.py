import bmesh

me = bpy.context.object
bm = bmesh.from_edit_mesh(me.data)
mw  = me.matrix_world
verts = [v for v in bm.verts if v.select]

value = min(verts,key=lambda v:(mw@v.co).x)
bpy.context.scene.cursor.location = mw@value .co
