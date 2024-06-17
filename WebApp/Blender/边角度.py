import bmesh
from math import degrees, atan2, pi
from mathutils import Vector
e = [e for e in bmesh.from_edit_mesh(bpy.context.object.data).edges if e.select][0]
b=e.verts[0]
print(b.co)
a = Vector((b.co.x+1,b.co.y,b.co.z)) - b.co
c = e.verts[1].co- b.co
a.negate()
axis = a.cross(c).normalized()
M = axis.rotation_difference(up).to_matrix().to_4x4()  
a = (M @ a).xy.normalized()
c = (M @ c).xy.normalized()
print((pi - atan2(a.cross(c), a.dot(c)))*180/pi)