import bpy
from math import pi, floor, ceil, radians
from mathutils import Vector, Quaternion, geometry
import numpy as lumpy

import pdb


def create_mult_knot_bezier(seed_in):
    res_per_section = 6
    interpolate_bezier = geometry.interpolate_bezier
    ops_curve = bpy.ops.curve
    ops_mesh = bpy.ops.mesh

    # Create a curve, subdivide and randomize it.
    ops_curve.primitive_bezier_curve_add(enter_editmode=True)
    ops_curve.subdivide(number_cuts=4)
    bpy.ops.transform.vertex_random(offset=5.0, uniform=0.1, normal=0.01, seed=seed_in)

    # After randomizing the curve, recalculate its normals so
    # the cubes calculated by interpolate bezier will be more
    # evenly spaced.
    ops_curve.select_all(action='SELECT')
    ops_curve.normals_make_consistent(calc_length=True)

    # Switch back to object mode and cache references to the curve
    # and its bezier points.
    bpy.ops.object.mode_set(mode='OBJECT')
    bez_curve = bpy.context.active_object
    bez_points = bez_curve.data.splines[0].bezier_points

    # Create an empty list.
    points_on_curve = []

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
        points_on_curve += calc_points
    return points_on_curve, bez_curve, res_per_section, bez_len, ops_mesh




def create_bezier(seed_in):
    count = 10
    ops_mesh = bpy.ops.mesh

    # Create curve and randomize its points.
    bpy.ops.curve.primitive_bezier_curve_add(enter_editmode=True)
    bpy.ops.transform.vertex_random(offset=5.0, uniform=0.1, normal=0.01, seed=seed_in)
    bpy.ops.object.mode_set(mode='OBJECT')

    # pdb.set_trace()

    # Acquire a reference to the bezier points.
    bez_curve = bpy.context.active_object
    bez_points = bez_curve.data.splines[0].bezier_points

    # Get a list of points distributed along the curve.
    points_on_curve = geometry.interpolate_bezier(
        bez_points[0].co,
        bez_points[0].handle_right,
        bez_points[1].handle_left,
        bez_points[1].co,
        count)

    return points_on_curve, bez_curve, ops_mesh, count

    # # Create an empty object to serve as parent to all cubes.
    # ops.object.empty_add(type='PLAIN_AXES', location=bez_curve.location)
    # group = context.active_object

    # # Create cubes.
    # cube_rad = 0.5 / count
    # for point in points_on_curve:
    #     ops_mesh.primitive_cube_add(radius=cube_rad, location=point)
    #     cube = context.active_object
    #     cube.parent = group



def bezier_multi_seg(knots=[], step=0.0, closed_loop=False):
    knots_len = len(knots)
    if knots_len == 1:
        knot = knots[0]
        coord = knot.co.copy()
        return {'coord': coord, 'tangent': knot.handle_right - coord}

    if closed_loop:
        scaled_t = (step % 1.0) * knots_len
        index = int(scaled_t)
        a = knots[index]
        b = knots[(index + 1) % knots_len]
    else:
        if step <= 0.0:
            knot = knots[0]
            coord = knot.co.copy()
            return {'coord': coord, 'tangent': knot.handle_right - coord}
        if step >= 1.0:
            knot = knots[-1]
            coord = knot.co.copy()
            return {'coord': coord, 'tangent': coord - knot.handle_left}

        scaled_t = step * (knots_len - 1)
        index = int(scaled_t)
        a = knots[index]
        b = knots[index + 1]

    pt0 = a.co
    pt1 = a.handle_right
    pt2 = b.handle_left
    pt3 = b.co
    u = scaled_t - index

    coord = bezier_step(pt0=pt0, pt1=pt1, pt2=pt2, pt3=pt3, step=u)
    tangent = bezier_tangent(pt0=pt0, pt1=pt1, pt2=pt2, pt3=pt3, step=u)
    

    return {'coord': coord, 'tangent': tangent}



def bezier_tangent(pt0=Vector(), pt1=Vector(), pt2=Vector(), pt3=Vector(), step=0.5):
    # Return early if step is out of bounds [0, 1].
    if step <= 0.0:
        return pt1 - pt0
    if step >= 1.0:
        return pt3 - pt2

    # Find coefficients.
    u = 1.0 - step
    ut6 = u * step * 6.0
    tsq3 = step * step * 3.0
    usq3 = u * u * 3.0

    # Find tangent and return.
    return (pt1 - pt0) * usq3 + (pt2 - pt1) * ut6 + (pt3 - pt2) * tsq3


def bezier_step(pt0=Vector(), pt1=Vector(), pt2=Vector(), pt3=Vector(), step=0.0):
    # Return early if step is out of bounds [0, 1].
    if step <= 0.0:
        return pt0.copy()
    if step >= 1.0:
        return pt3.copy()

    # Find coefficients.
    u = 1.0 - step
    tcb = step * step
    ucb = u * u
    tsq3u = tcb * 3.0 * u
    usq3t = ucb * 3.0 * step
    tcb *= step
    ucb *= u

    # Find point and return.
    return pt0 * ucb + pt1 * usq3t + pt2 * tsq3u + pt3 * tcb


# Smoother Step for translations.
def smootherstep(arr, st, vout):
    sz = len(arr)
    if sz == 0:
        return vout

    if sz == 1 or st <= 0:
        vout.x = arr[0].x
        vout.y = arr[0].y
        vout.z = arr[0].z
        return vout

    if st >= 1.0:
        vout.x = arr[sz - 1].x
        vout.y = arr[sz - 1].y
        vout.z = arr[sz - 1].z
        return vout

    st = st * st * st * (st * (st * 6.0 - 15.0) + 10.0)
    st *= sz - 1
    i = floor(st)
    t = st - i
    j = min(i + 1, sz - 1)
    u = 1.0 - t
    vout.x = u * arr[i].x + t * arr[j].x
    vout.y = u * arr[i].y + t * arr[j].y
    vout.z = u * arr[i].z + t * arr[j].z
    return vout


# Normalized lerp for Quaternions.
def nlerp(arr, st, qout):
    sz = len(arr)
    if sz == 0:
        return qout

    if sz == 1 or st <= 0.0:
        qout.x = arr[0].x
        qout.y = arr[0].y
        qout.z = arr[0].z
        qout.w = arr[0].w
        return qout

    j = sz - 1
    if st >= 1.0:
        qout.x = arr[j].x
        qout.y = arr[j].y
        qout.z = arr[j].z
        qout.w = arr[j].w
        return qout

    st *= j
    i = floor(st)
    t = st - i
    j = min(i + 1, sz - 1)
    u = 1.0 - t

    qout.x = u * arr[i].x + t * arr[j].x
    qout.y = u * arr[i].y + t * arr[j].y
    qout.z = u * arr[i].z + t * arr[j].z
    qout.w = u * arr[i].w + t * arr[j].w
    qout.normalize()
    return qout


# Lerp for scalar values.
def lerparr(arr, step):
    sz = len(arr)
    if sz == 0:
        return 0.0
    if sz == 1 or step <= 0.0:
        return arr[0]
    j = sz - 1
    if step >= 1.0:
        return arr[j]
    step *= j
    i = floor(step)
    t = step - i
    j = min(i + 1, sz - 1)
    return (1.0 - t) * arr[i] + t * arr[j]
    

    


def translate_bezier(t_opt, r_opt, s_opt, scn, currobj, currot, currsz):
    randseed = lumpy.random.randint(1000)

    if t_opt == "bezier":
        points_on_curve, bez_curve, ops_mesh, count = create_bezier(randseed)
    elif t_opt == "multi_bezier":
        points_on_curve, bez_curve, res_per_section, bez_len, ops_mesh = create_mult_knot_bezier(randseed)
    else:
        print("you shouldn't effing be here u dumdum")

    # pdb.set_trace()

    curve = bpy.context.active_object
    spline = curve.data.splines[0]
    bezier_points = spline.bezier_points
    is_cyclic = spline.use_cyclic_u

    # Create Suzanne monkey head.
    # bpy.ops.mesh.primitive_cube_add(size=0.1)
    # suzanne = bpy.context.active_object
    # suzanne.rotation_mode = 'QUATERNION'

    # Create three keyframes for each bezier point.
    key_frame_count = 3.0 * len(bezier_points)
    frame_start = scn.frame_start
    frame_end = scn.frame_end
    frame_len = frame_end - frame_start
    frame_skip = int(max(1.0, frame_len / key_frame_count))
    frame_range = range(frame_start, frame_end, frame_skip)
    frame_to_percent = 1.0 / frame_len

    rots = []
    for i in range(6):
        rot = Quaternion(tuple(2.0 * lumpy.random.rand(4)))
        rots.append(rot)

    for frame in frame_range:
        # Set to current frame.
        scn.frame_set(frame)

        # Calculate coordinates for frame by converting to a percent.
        frame_percent = frame * frame_to_percent
        coord_tangent = bezier_multi_seg(knots=bezier_points,
                                         step=frame_percent,
                                         closed_loop=is_cyclic)

        # Set location.
        coord = coord_tangent['coord']
        currobj.location = coord
        currobj.keyframe_insert(data_path='location')

        if r_opt == "facing_bezier":
            # Set rotation based on tangent.
            tangent = coord_tangent['tangent']
            tangent.normalize()
            rotation = tangent.to_track_quat('-Y', 'Z')
            currobj.rotation_quaternion = rotation
            currobj.keyframe_insert(data_path='rotation_quaternion')
        elif r_opt == "random":
            print("brah you should nOT BE HERE HOW MANY TIMES DO U HAVE TO GO THRU THIS")


def translate_straight(t_opt, r_opt, s_opt, currobj, currot, currsz, currpt):
    pts = []
    for i in range(8):
        pt = Vector(tuple(1.5 * lumpy.random.rand((3))))
        pts.append(pt)

    currframe = 0
    fcount = len(pts)
    invfcount = 1.0 / (fcount - 1)
    frange = bpy.context.scene.frame_end - bpy.context.scene.frame_start
    fincr = ceil(frange / (fcount - 1))

    rots = []
    for i in range(6):
        rot = Quaternion(tuple(2.0 * lumpy.random.rand(4)))
        rots.append(rot)

    currsz = 1.0
    szs = []
    for i in range(6):
        sz = 2.5 * lumpy.random.rand(1)[0]
        szs.append(sz)

    for f in range(0, fcount, 1):
        fprc = f * invfcount
        bpy.context.scene.frame_set(currframe)

        if t_opt == "straight":
        # Translate.
            currobj.location = smootherstep(pts, fprc, currpt)
            currobj.keyframe_insert(data_path='location')
        elif t_opt != "none":
            print("bruh you should not be here u fucked up")


        if r_opt == "random":
            # Rotate.
            currobj.rotation_quaternion = nlerp(rots, fprc, currot)
            currobj.keyframe_insert(data_path='rotation_quaternion')
        elif r_opt == "facing_bezier":
            print("ok again you should not be here gtfo")

        if s_opt == "scale":
            # Scale.
            currsz = lerparr(szs, fprc)
            currobj.scale = [currsz, currsz, currsz]
            currobj.keyframe_insert(data_path='scale')

        currframe += fincr



def est_scene():
    #if anything is on the scene already, delete it
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()
    
    #add camera
    scn = bpy.context.scene
    cam_data = bpy.data.cameras.new("Camera")
    cam_obj = bpy.data.objects.new("Camera", cam_data)
    cam_obj.location = (8, 8, 8)
    cam_obj.rotation_euler = (radians(56), radians(0.0), radians(180-45))
    scn.collection.objects.link(cam_obj)
    scn.camera = cam_obj
    # cam_obj.select = True
    # scn.objects.active = cam_obj

    #add light
    light_data = bpy.data.lights.new(name="sun", type="SUN")
    lamp_obj = bpy.data.objects.new(name="sun", object_data=light_data)
    scn.collection.objects.link(lamp_obj)
    lamp_obj.location =(0.0, 0.0, 5.0)
    # lamp_obj.select = True
    # scn.objects.active = lamp_obj

    fps = 24
    #total time in seconds
    total_time = 10
    scn.frame_start = 0
    scn.frame_end = int(total_time * fps) + 1

    return scn


def export(scn, t_opt, s_opt, r_opt, i):
    # #export as pngs, ffmpeg, and multilayer exr
    settings = scn.render.image_settings

    #THIS IS IMPORTANT FOR VULCAN
    # bpy.context.scene.render.engine = 'CYCLES'

    outdirname = "t_"+t_opt+"_s_"+s_opt+"_r_"+r_opt+"_"+str(i)

    # settings.file_format = "PNG"
    # outfilepath_png = "/Users/lilhuang/Desktop/Blender/Suzanne_datasets/"+outdirname+"_png/frame_"
    # scn.render.filepath = outfilepath_png
    # bpy.ops.render.render(animation=True)

    settings.file_format = "OPEN_EXR_MULTILAYER"
    outfilepath_exr = "/Users/lilhuang/Desktop/Blender/Suzanne_datasets/"+outdirname+"_exr/multilayer_"
    scn.render.filepath = outfilepath_exr
    bpy.ops.render.render(animation=True, write_still=True)

    # # #export as ffmpeg
    # settings.file_format = "FFMPEG"
    # outfilepath_vid = "/Users/lilhuang/Desktop/Blender/Suzanne_datasets/"+outdirname+"/video_"
    # scn.render.filepath = outfilepath_vid
    # bpy.ops.render.render(animation=True)

    



def export_test(scn):
    #export as pngs or jpegs
    settings = scn.render.image_settings

    #IMPORTANT FOR VULCAN
    # bpy.context.scene.render.engine = 'CYCLES'

    settings.file_format = "PNG"
    # outfilepath_png = "/fs/cfar-projects/anim_inb/TEST_BLENDER/frame_"
    outfilepath_png = "/Users/lilhuang/Desktop/Blender/TEST_BLENDER_2/frame_"
    scn.render.filepath = outfilepath_png
    bpy.ops.render.render(animation=True)



def mainmain():
    translate_options = ["none", "straight", "bezier", "multi_bezier"]
    rotation_options = ["none", "random", "facing_bezier"]
    scale_options = ["none", "scale"]
    # translate_options = ["straight"]
    # rotation_options = ["random"]
    # scale_options = ["none"]

    for i in range(2, 10):
    # for i in range(1):
        for t_opt in translate_options:
            for r_opt in rotation_options:
                for s_opt in scale_options:
                    mainloop(t_opt, r_opt, s_opt, i)



def mainloop(t_opt, r_opt, s_opt, i):
    if t_opt == "bezier" or t_opt == "multi_bezier":
        if r_opt == "random" or s_opt == "scale":
            return
    elif t_opt == "straight" or t_opt == "none":
        if r_opt == "facing_bezier":
            return

    scn = est_scene()

    currpt = Vector((0.0, 0.0, 0.0))
    cubesz = 1.0
    currsz = 1.0
    
    # add cube to scene
    # bpy.ops.mesh.primitive_cube_add(location=currpt, size=cubesz)
    # bpy.ops.mesh.primitive_cube_add(location=currpt)

    # add Suzanne to scene
    bpy.ops.mesh.primitive_monkey_add(location=currpt)
    currobj = bpy.context.object

    # Rotations. Quaternion component order is w, x, y, z.
    currobj.rotation_mode = 'QUATERNION'
    currot = Quaternion()

    if t_opt == "bezier" or t_opt == "multi_bezier":
        translate_bezier(t_opt, r_opt, s_opt, scn, currobj, currot, currsz)
    else:
        translate_straight(t_opt, r_opt, s_opt, currobj, currot, currsz, currpt)

    export(scn, t_opt, r_opt, s_opt, i)
    # export_test(scn)


def test():
    mainloop("straight", "none", "none", 0)


if __name__ == '__main__':
    mainmain()
    # test()




