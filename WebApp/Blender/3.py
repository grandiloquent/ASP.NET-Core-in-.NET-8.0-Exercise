import bmesh
bm = bmesh.from_edit_mesh(bpy.context.object.data)

edges = bmesh.from_edit_mesh(bpy.context.object.data).edges

bound_edges=[e for e in bm.edges if e.select]
for e in bound_edges:
  bm.edges.ensure_lookup_table()
  v = edges[0]
  print(e.verts[0].co)
  for x in edges:
    if(abs(x.verts[0].co.x-e.verts[0].co.x)<0.0001)):
      v=x
      break
  bmesh.ops.contextual_create(bm, geom=list([e,v]))

bmesh.update_edit_mesh(bpy.context.object.data)