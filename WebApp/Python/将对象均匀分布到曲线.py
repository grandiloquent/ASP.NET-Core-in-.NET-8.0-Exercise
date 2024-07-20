import bpy
from math import pi, floor, ceil, radians
from mathutils import Vector, Quaternion, geometry

def bezier_tangent(pt0=Vector(), pt1=Vector(), pt2=Vector(), pt3=Vector(), step=0.5):
    if step <= 0.0:
        return pt1 - pt0
    if step >= 1.0:
        return pt3 - pt2

    u = 1.0 - step
    ut6 = u * step * 6.0
    tsq3 = step * step * 3.0
    usq3 = u * u * 3.0

    return (pt1 - pt0) * usq3 + (pt2 - pt1) * ut6 + (pt3 - pt2) * tsq3

def duplicator(
        ob_name, new_ob_name, 
        col_name, amount):

    new_obs = []

    for i in range(0, amount):

        # get the object
        ob = bpy.context.scene.objects.get(ob_name)

        # copy, name and store new_name
        new_ob = ob.copy()
        new_name = new_ob_name + str ( i )
        new_ob.name = new_name
        new_obs.append(new_ob)

        # place it in the collection
        col = bpy.context.scene.collection.children.get(col_name)
        #bpy.context.scene.collection.children.link(newCol) <-- in case you need to add the collection to the scene
        col.objects.link(new_ob)

    return new_obs

res_per_section = 2
interpolate_bezier = geometry.interpolate_bezier
ops_curve = bpy.ops.curve
ops_mesh = bpy.ops.mesh

bpy.ops.object.mode_set(mode='OBJECT')
bez_curve = bpy.context.active_object
bez_points = bez_curve.data.splines[0].bezier_points

# Create an empty list.
points_on_curve = []
point_tangent_pairs=[]
# Loop through the bezier points in the bezier curve.
bez_len = len(bez_points)
i_range = range(1, bez_len, 1)
for i in i_range:

    # Cache a current and next point.
    curr_point = bez_points[i - 1]
    next_point = bez_points[i]

    # Calculate bezier points for this segment.
    calc_points = interpolate_bezier(
        curr_point.co,
        curr_point.handle_right,
        next_point.handle_left,
        next_point.co,
        res_per_section + 1)

    # The last point on this segment will be the
    # first point on the next segment in the spline.
    if i != bez_len - 1:
        calc_points.pop()

    # Concatenate lists.
    #points_on_curve += calc_points
  # Find a list of points along the curve....
  
  # Loop through the calculated points.
    points_len = len(calc_points)
    to_percent = 1.0 / (points_len - 1)
    j_range = range(0, points_len, 1)
    for j in j_range:
        # Convert progress through the loop to a percent.
        j_percent = j * to_percent

        # Calculate the tangent.
        tangent = bezier_tangent(
            pt0=curr_point.co,
            pt1=curr_point.handle_right,
            pt2=next_point.handle_left,
            pt3=next_point.co,
            step=j_percent)

        # Set the vector to unit length.
        tangent.normalize()

        # Place the point and tangent in a dictionary.
        entry = {'co': calc_points[j], 'tan': tangent}

        # Append the dictionary to the list.
        point_tangent_pairs.append(entry)

        # Concatenate lists.
    points_on_curve += point_tangent_pairs

# Create an empty parent under which cubes will be placed.
bpy.ops.object.empty_add(type='PLAIN_AXES', location=bez_curve.location)
group = bpy.context.active_object

add_points =[];
for point in points_on_curve:
    if point['co'] in add_points:
        continue
    add_points.append(point['co'])
    cube = duplicator('1', '1_', 'Collection', 1)[0]
    cube.location=point['co']
    cube.rotation_mode = 'QUATERNION'
    cube.parent = group
    cube.rotation_quaternion = point['tan'].to_track_quat('-Y', 'Z')