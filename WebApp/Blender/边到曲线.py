import bmesh
import bpy

verts=[e for e in bmesh.from_edit_mesh(bpy.context.object.data).verts if e.select]
co1 =bpy.context.object.matrix_world @ verts[0].co 
co2 =bpy.context.object.matrix_world @ verts[1].co 
print(co1)
print(co2)

bpy.ops.curve.primitive_bezier_curve_add()
curve = bpy.context.active_object
bez_points = curve.data.splines[0].bezier_points

co1=bpy.context.object.matrix_world.inverted() @ co1
co2=bpy.context.object.matrix_world.inverted() @ co2 
print(co1)
print(co2)

#bez_points.add(1)

bez_points[0].co =co1 
bez_points[0].handle_left_type  = 'FREE'
bez_points[0].handle_right_type = 'FREE'
bez_points[0].handle_left  = co1
bez_points[0].handle_right =co1
    

bez_points[1].co = co2
bez_points[1].handle_left_type  = 'FREE'
bez_points[1].handle_right_type = 'FREE'
bez_points[1].handle_left  = co2
bez_points[1].handle_right = co2

bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
bpy.context.object.data.bevel_depth = 0.09
bpy.context.object.data.use_fill_caps = True


