bpy.ops.curve.primitive_bezier_curve_add()
curve = bpy.context.active_object
bez_points = curve.data.splines[0].bezier_points

f = 0.5521252155303955/2
r = 1.24222 /2
z = 1.47149 
l = f*r*2

control_points = [[[0,0,0],[0,0,-l],[0,0,l]],[[-r,0,r],[l-r,0,r],[-l-r,0,r]]]
bez_points.add(len(control_points) - 2)
    
# now copy the csv data
for i in range(len(control_points)):        
  bez_points[i].co = control_points[i][0]
  bez_points[i].handle_left_type  = 'FREE'
  bez_points[i].handle_right_type = 'FREE'
  bez_points[i].handle_left  = control_points[i][1]
  bez_points[i].handle_right = control_points[i][2]
    
bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')