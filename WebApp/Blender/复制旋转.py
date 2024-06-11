import bpy
from math import radians
from mathutils import Matrix
o = bpy.context.object
parent = o
while parent.parent is not None:
    parent=parent.parent

o.select_set(False)
parent.select_set(True)
for o in parent.children_recursive:
    o.select_set(True)

bpy.ops.object.duplicate_move()
for o in parent.children_recursive:
    o.select_set(False)

# example on an active object
obj = bpy.context.active_object

# define some rotation
angle_in_degrees = 90
rot_mat = Matrix.Rotation(radians(angle_in_degrees), 4, 'Z')   # you can also use as axis Y,Z or a custom vector like (x,y,z)

# decompose world_matrix's components, and from them assemble 4x4 matrices
orig_loc, orig_rot, orig_scale = obj.matrix_world.decompose()
orig_loc_mat = Matrix.Translation(orig_loc)
orig_rot_mat = orig_rot.to_matrix().to_4x4()
orig_scale_mat = Matrix.Scale(orig_scale[0],4,(1,0,0)) * Matrix.Scale(orig_scale[1],4,(0,1,0)) @ Matrix.Scale(orig_scale[2],4,(0,0,1))

# assemble the new matrix
obj.matrix_world = orig_loc_mat @ rot_mat @ orig_rot_mat @ orig_scale_mat 