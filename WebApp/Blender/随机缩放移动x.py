from mathutils import Vector
from random import uniform
o = bpy.context.object
for i in range(1,12):
    corners = [ bpy.context.object.matrix_world @ Vector(corner) for corner in  bpy.context.object.bound_box] 
    oy = max([v.x for v in corners])
    bpy.context.object.select_set(False)
    o.select_set(True)
    bpy.context.view_layer.objects.active = o
    bpy.ops.object.duplicate_move()
    scale = uniform(0.3,1.6)
    bpy.ops.transform.resize(value=(scale,1,1))
    corners = [ bpy.context.object.matrix_world @ Vector(corner) for corner in  bpy.context.object.bound_box]
    x = oy-min([v.x for v in corners])
    bpy.ops.transform.translate(value=(x+0.02,0,  0))