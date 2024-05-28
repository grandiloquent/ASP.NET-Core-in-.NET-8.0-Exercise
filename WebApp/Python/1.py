import bpy

bpy.ops.curve.primitive_bezier_curve_add()
curve = bpy.context.active_object
bez_points = curve.data.splines[0].bezier_points

control_points = [[[567.5,-288.499999999999,0],[567.5,-288.499999999999,0],[567.5,-288.499999999999,0]],[[531.5,-484.5,0],[297.5,-390.5,0],[531.5,-484.5,0]],[[706.5,-371.499999999999,0],[706.5,-371.499999999999,0],[706.5,-371.499999999999,0]]]
# note: a created bezier curve has already 2 control points
bez_points.add(len(control_points) - 2)

# now copy the csv data
for i in range(len(control_points)):        
    bez_points[i].co = control_points[i][0]
    bez_points[i].handle_left_type  = 'FREE'
    bez_points[i].handle_right_type = 'FREE'
               
    bez_points[i].handle_left  = control_points[i][1]
    bez_points[i].handle_right = control_points[i][2]