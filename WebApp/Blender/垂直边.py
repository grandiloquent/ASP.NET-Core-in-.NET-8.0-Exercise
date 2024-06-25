import bpy
import bmesh

bm = bmesh.from_edit_mesh(bpy.context.object.data)
edge = [e for e in bm.edges if e.select][0]
midpoint = (edge.verts[0].co + edge.verts[1].co) / 2
edge_direction = edge.verts[1].co - edge.verts[0].co
perpendicular_direction = edge_direction.normalized().cross((0, 0, 1))
# (1,0,0)
new_vert = bm.verts.new(midpoint)
new_vert_p = bm.verts.new((new_vert.co.x+perpendicular_direction.x,new_vert.co.y+perpendicular_direction.y,new_vert.co.z+perpendicular_direction.z))
new_edge = bm.edges.new([new_vert, new_vert_p])
bmesh.update_edit_mesh(bpy.context.object.data)