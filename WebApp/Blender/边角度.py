import bmesh
from math import degrees, atan2, pi
from mathutils import Vector
up = Vector((0, 0, 1))
e = [e for e in bmesh.from_edit_mesh(bpy.context.object.data).edges if e.select]
e1 = e[0]
e2 = e[1]
b = set(e1.verts).intersection(e2.verts).pop()
a = e1.other_vert(b).co - b.co
c = e2.other_vert(b).co - b.co
a.negate()
axis = a.cross(c).normalized()
M = axis.rotation_difference(up).to_matrix().to_4x4()  
a = (M @ a).xy.normalized()
c = (M @ c).xy.normalized()
print((pi - atan2(a.cross(c), a.dot(c)))*180/pi)