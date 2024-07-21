"""
以曲线为中心，均匀分布对象，并依据“切线”旋转

用法：
1. 选择一条曲线
2. 将一个3D对象重命名为“1”
3. 粘贴代码执行    
"""
import bpy
from mathutils import Vector
import numpy as np

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

def cubic_bezier_points_extended(control_points, t_values):
    M = np.array([
        [1, 0, 0, 0],
        [-3, 3, 0, 0],
        [3, -6, 3, 0],
        [-1, 3, -3, 1]
    ])
    
    n = (len(control_points) - 1) // 3
    
    bezier_points = []
    
    for t in t_values:
        segment_index = int(t) 
        if segment_index >= n:
            segment_index = n - 1  # Clamp to the last segment for t values out of range
        
        local_t = t - segment_index
        
        cp_index = segment_index * 3
        segment_control_points = control_points[cp_index:cp_index+4]
        
        T = np.array([1, local_t, local_t**2, local_t**3])
        
        point = T @ M @ segment_control_points  # Matrix multiplication to get the point
        bezier_points.append(point)
    
    return np.array(bezier_points)

def numeric_distance_integration(control_points, resolution=1000):
    n_segments = (len(control_points) - 1) // 3
    t_values = np.linspace(0, n_segments, resolution+1)
    bezier_points = cubic_bezier_points_extended(control_points, t_values)
    distance = np.sqrt(np.sum(np.power(bezier_points[:-1,:] - bezier_points[1:,:],2),axis=-1))
    return distance

def cubic_bezier_points_equdistant(control_points, count=20, resolution=1000):
    n_segments = (len(control_points) - 1) // 3
    x = np.linspace(0, n_segments, resolution)
    y = numeric_distance_integration(control_points, resolution=resolution)
    length = np.sum(y)
    t_values_equidistant = np.interp(np.linspace(0, 1, count),y.cumsum()/length,x,)
    return cubic_bezier_points_extended(control_points, t_values_equidistant)


def resample_curve(obj, count=40):
    if obj.type != 'CURVE':
        raise ValueError("Object is not a curve in custom function `resample_curve()`.")

    spline = obj.data.splines[0]
    control_points = []
    for point_index in range(len(spline.bezier_points)-1):
        a = spline.bezier_points[point_index]
        b = spline.bezier_points[point_index+1]
        if point_index == 0:
            control_points.append(a.co.xyz)
        control_points.extend([
            a.handle_right.xyz,
            b.handle_left.xyz,
            b.co.xyz,
        ])
    control_points = [obj.matrix_world @ p for p in control_points]
    control_points = np.array(control_points)
    equidistant_points_np = cubic_bezier_points_equdistant(control_points, count=count)
    equidistant_points = [Vector(p) for p in equidistant_points_np]
    return equidistant_points

def duplicator(
        ob_name, new_ob_name, 
        col_name, amount):

    new_obs = []

    for i in range(0, amount):

        ob = bpy.context.scene.objects.get(ob_name)

        new_ob = ob.copy()
        new_name = new_ob_name + str ( i )
        new_ob.name = new_name
        new_obs.append(new_ob)

        col = bpy.context.scene.collection.children.get(col_name)
        col.objects.link(new_ob)

    return new_obs


ops_curve = bpy.ops.curve
ops_mesh = bpy.ops.mesh

bpy.ops.object.mode_set(mode='OBJECT')
bez_points = resample_curve(bpy.context.active_object)

points_on_curve = []
point_tangent_pairs=[]

bez_len = len(bez_points)
i_range = range(1, bez_len, 1)
to_percent = 1.0 / (bez_len - 1)
for i in i_range:

    curr_point = bez_points[i - 1]
    next_point = bez_points[i]

    i_percent = i * to_percent

    tangent = bezier_tangent(
        pt0=curr_point,
        pt1=curr_point,
        pt2=next_point,
        pt3=next_point,
        step= i_percent)

    tangent.normalize()

    entry = {'co': bez_points[i], 'tan': tangent}

    point_tangent_pairs.append(entry)

points_on_curve += point_tangent_pairs

add_points =[];
for point in points_on_curve:
    if point['co'] in add_points:
        continue
    add_points.append(point['co'])
    cube = duplicator('1', '1_', 'Collection', 1)[0]
    cube.location=point['co']
    cube.rotation_mode = 'QUATERNION'
    cube.rotation_quaternion = point['tan'].to_track_quat('-Y', 'Z')