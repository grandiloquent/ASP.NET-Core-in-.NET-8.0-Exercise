bl_info = {
    "name" : "Alignment",
    "description" : "一个快速对齐的插件",
    "author" : "",
    "version" : (0, 0, 1),
    "blender" : (2, 80, 0),
    "location" : "View3D",
    "warning" : "",
    "support" : "COMMUNITY",
    "doc_url" : "",
    "category" : "3D View"
}

import bpy
from bpy.types import Operator
from bpy.types import Panel
import bmesh
from mathutils import Vector
from math import radians
from mathutils import Matrix

def align(value):
    l = bpy.context.scene.cursor.location
    o = bpy.context.object
    center = o.matrix_world @ (0.125 * sum((Vector(b) for b in o.bound_box), Vector()))
    corners = [ bpy.context.object.matrix_world @ Vector(corner) for corner in  bpy.context.object.bound_box] 
    parent = o
    while parent.parent is not None:
        parent=parent.parent

    o.select_set(False)
    parent.select_set(True)
    bpy.context.view_layer.objects.active = parent
    if value == 1:
        x = l.x-min([v.x for v in corners])
        bpy.ops.transform.translate(value=(x,0,0))
    elif value==2:
        x =l.x-center.x
        bpy.ops.transform.translate(value=(x,0,0))
    elif value==3:
        x =l.x-max([v.x for v in corners])
        bpy.ops.transform.translate(value=(x,0,0))
    elif value == 4:
        y = l.y-min([v.y for v in corners])
        bpy.ops.transform.translate(value=(0,y,0))
    elif value==5:
        y =l.y-center.y
        bpy.ops.transform.translate(value=(0,y,0))
    elif value==6:
        y =l.y-max([v.y for v in corners])
        bpy.ops.transform.translate(value=(0,y,0))
    elif value == 7:
        z = l.z-min([v.z for v in corners])
        bpy.ops.transform.translate(value=(0,0,z))
    elif value==8:
        z =l.z-center.z
        bpy.ops.transform.translate(value=(0,0,z))
    elif value==9:
        z =l.z-max([v.z for v in corners])
        bpy.ops.transform.translate(value=(0,0,z))

    return None

class _align_n_x(Operator):
    """ Align a group object by select object """
    bl_idname = "align.nx"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(1)
        return {'FINISHED'}

class _align_x(Operator):
    """ Align a group object by select object """
    bl_idname = "align.x"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(2)
        return {'FINISHED'}
    
class _align_p_x(Operator):
    """ Align a group object by select object """
    bl_idname = "align.px"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(3)
        return {'FINISHED'}

class _align_n_y(Operator):
    """ Align a group object by select object """
    bl_idname = "align.ny"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(4)
        return {'FINISHED'}

class _align_y(Operator):
    """ Align a group object by select object """
    bl_idname = "align.y"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(5)
        return {'FINISHED'}
    
class _align_p_y(Operator):
    """ Align a group object by select object """
    bl_idname = "align.py"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(6)
        return {'FINISHED'}

class _align_n_z(Operator):
    """ Align a group object by select object """
    bl_idname = "align.nz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(7)
        return {'FINISHED'}

class _align_z(Operator):
    """ Align a group object by select object """
    bl_idname = "align.z"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(8)
        return {'FINISHED'}
    
class _align_p_z(Operator):
    """ Align a group object by select object """
    bl_idname = "align.pz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        align(9)
        return {'FINISHED'}

class _modifier_mirror(Operator):
    """ Quick add modifier """
    bl_idname = "modifier.mirror"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        bpy.ops.object.modifier_add(type='MIRROR')
        return {'FINISHED'}

class _modifier_bevel(Operator):
    """ Quick add modifier """
    bl_idname = "modifier.bevel"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        bpy.ops.object.modifier_add(type='BEVEL')
        bpy.context.object.modifiers["Bevel"].width = 0.018
        bpy.context.object.modifiers["Bevel"].segments = 2
        bpy.context.object.modifiers["Bevel"].miter_outer = 'MITER_ARC'
        bpy.context.object.modifiers["Bevel"].harden_normals = True
        return {'FINISHED'}

class _modifier_solidify(Operator):
    """ Quick add modifier """
    bl_idname = "modifier.solidify"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        bpy.ops.object.modifier_add(type='SOLIDIFY')
        return {'FINISHED'}


def align_vert(mode):
    l = bpy.context.scene.cursor.location
    s = bpy.context.object.matrix_world @ [e for e in bmesh.from_edit_mesh(bpy.context.object.data).verts if e.select][0].co
    x = l.x-s.x
    y=l.y-s.y
    z=l.z-s.z
    bpy.ops.object.mode_set(mode='OBJECT')
    o = bpy.context.object
    parent = o
    while parent.parent is not None:
        parent=parent.parent

    o.select_set(False)
    parent.select_set(True)
    bpy.context.view_layer.objects.active = parent
    if mode==1:
        bpy.ops.transform.translate(value=(x,0,0))
    elif mode==2:
        bpy.ops.transform.translate(value=(0,y,0))
    elif mode==3:
        bpy.ops.transform.translate(value=(0,0,z))
    return None

class _align_v_x(Operator):
    """ Align a object by select vert """
    bl_idname = "align.vx"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        align_vert(1)
        return {'FINISHED'}

class _align_v_y(Operator):
    """ Align a object by select vert """
    bl_idname = "align.vy"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        align_vert(2)
        return {'FINISHED'}

class _align_v_z(Operator):
    """ Align a object by select vert """
    bl_idname = "align.vz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        align_vert(3)
        return {'FINISHED'}

def select_group():
    o = bpy.context.active_object
    p = o
    while p.parent is not None:
        p = p.parent

    for o in p.children_recursive:
        o.select_set(True)

    p.select_set(True)
    return None

class _select_group(Operator):
    """ Selection group """
    bl_idname = "select.group"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        select_group()
        return {'FINISHED'}

def edge_loops(edge,bm):
    def walk(edge):
        yield edge
        edge.tag = True
        for l in edge.link_loops:
            loop = l.link_loop_radial_next.link_loop_next.link_loop_next
            if not (len(loop.face.verts) != 4 or loop.edge.tag):
                yield from walk(loop.edge)
    for e in bm.edges:
        e.tag = False
    return list(walk(edge))

def loopcut(cuts):
    context = bpy.context
    ob = context.object
    me = ob.data

    bm = bmesh.from_edit_mesh(me)
    edge = bm.select_history.active

    if isinstance(edge, bmesh.types.BMEdge): 
        ''' 
        bmesh.ops.subdivide_edges(
            bm,
            edges=edge_loops(edge),
            cuts=cuts,
            smooth_falloff='INVERSE_SQUARE',
            use_grid_fill=True,
            )
        '''
        bmesh.ops.subdivide_edgering(
            bm,
            edges=edge_loops(edge,bm),
            cuts=cuts,
            profile_shape='INVERSE_SQUARE',
            profile_shape_factor=0.0,
            )    
        bmesh.update_edit_mesh(me)
    return None

class _loopcut_one(Operator):
    """ Selection group """
    bl_idname = "loopcut.one"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        loopcut(1);
        return {'FINISHED'}

def duplicate_move_z(mode):
    o = bpy.context.object
    parent = o
    while parent.parent is not None:
        parent=parent.parent

    o.select_set(False)
    parent.select_set(True)
    for o in parent.children_recursive:
        o.select_set(True)

    bpy.ops.object.duplicate_move()
    corners = [ Vector(corner) for corner in  bpy.context.object.bound_box] 

    if mode==1:
        bpy.ops.transform.translate(value=(max([v.x for v in corners])*-2,0,0))
    elif mode==3:
        bpy.ops.transform.translate(value=(0,(max([v.y for v in corners])*2)+.4,0))
    elif mode==3:
        bpy.ops.transform.translate(value=(0,0,max([v.z for v in corners])*2))

    return None

class _duplicate_move_z(Operator):
    """ Selection group """
    bl_idname = "duplicate.movez"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        duplicate_move_z(3)
        return {'FINISHED'}

class _duplicate_separate(Operator):
    """ Selection group """
    bl_idname = "duplicate.separate"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        duplicate_separate(1)
        return {'FINISHED'}

def duplicate_separate(mode):

    bpy.ops.mesh.duplicate_move()
    bpy.ops.mesh.separate(type='SELECTED')
    s = bpy.context.selected_objects[len(bpy.context.selected_objects)-1]
    for obj in bpy.context.selected_objects:
        obj.select_set(False)

    s.select_set(True)
    bpy.context.view_layer.objects.active=s
    bpy.ops.object.mode_set(mode='EDIT')
    mesh=bmesh.from_edit_mesh(bpy.context.object.data)
    for f in mesh.faces:
        f.select = True

    for f in mesh.edges:
        f.select = True

    bmesh.update_edit_mesh(bpy.context.object.data)
    bpy.ops.mesh.normals_make_consistent(inside=False)
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.mode_set(mode='EDIT')
    return None

class _loopcut_three(Operator):
    """ Selection group """
    bl_idname = "loopcut.three"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        loopcut(3)
        return {'FINISHED'}
class _loopcut_seven(Operator):
    """ Selection group """
    bl_idname = "loopcut.seven"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        loopcut(int(bpy.context.window_manager.clipboard))
        return {'FINISHED'}

class _loopcut_two(Operator):
    """ Selection group """
    bl_idname = "loopcut.two"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        loopcut(2)
        return {'FINISHED'}

def view_axis(mode):
    win = bpy.context.window
    scr = win.screen
    areas3d  = [area for area in scr.areas if area.type == 'VIEW_3D']
    region   = [region for region in areas3d[0].regions if region.type == 'WINDOW']


    # Add cube in the other window.
    with bpy.context.temp_override(window=win,area=areas3d[0],region=region[0]):
        t="TOP"
        if mode==1:
            t="LEFT"
        elif mode==2:
            t="RIGHT"
        elif mode==3:
            t="BOTTOM"
        elif mode==4:
            t="TOP"
        elif mode==5:
            t="FRONT"
        elif mode==6:
            t="BACK"
            
        bpy.ops.view3d.view_axis(type=t, align_active=False) 
        if areas3d[0].spaces.active.region_3d.is_perspective:
            bpy.ops.view3d.view_persportho()
            bpy.ops.view3d.view_persportho()
        else:
            pass
    return None

class _view_axis_left(Operator):
    """ Selection group """
    bl_idname = "view.axisleft"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        view_axis(1)
        return {'FINISHED'}
class _view_axis_right(Operator):
    """ Selection group """
    bl_idname = "view.axisright"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        view_axis(2)
        return {'FINISHED'}
class _view_axis_bottom(Operator):
    """ Selection group """
    bl_idname = "view.axisbottom"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        view_axis(3)
        return {'FINISHED'}
class _view_axis_top(Operator):
    """ Selection group """
    bl_idname = "view.axistop"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        view_axis(4)
        return {'FINISHED'}
class _view_axis_front(Operator):
    """ Selection group """
    bl_idname = "view.axisfront"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        view_axis(5)
        return {'FINISHED'}
class _view_axis_back(Operator):
    """ Selection group """
    bl_idname = "view.axisback"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        view_axis(6)
        return {'FINISHED'}

def duplicate_rotate(mode):
    # https://blender.stackexchange.com/questions/44760/rotate-objects-around-their-origin-along-a-global-axis-scripted-without-bpy-op
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
    return None

class _duplicate_rotate(Operator):
    """ Selection group """
    bl_idname = "duplicate.rotate"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        duplicate_rotate(1)
        return {'FINISHED'}

def add_plane(mode):
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.mesh.primitive_plane_add()
    bpy.ops.object.mode_set(mode='EDIT')
    return None

class _add_plane(Operator):
    """ Selection group """
    bl_idname = "add.plane"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT"

    def execute(self, context):
        add_plane(1)
        return {'FINISHED'}

def quick_resize(mode):
   #if bpy.context.window_manager.clipboard.isnumeric() is False:
      #return None
   v = float(bpy.context.window_manager.clipboard)
   if mode==0:
      bpy.ops.transform.resize(value=(v, 1, 1))
   elif mode==1:
      bpy.ops.transform.resize(value=( 1,v, 1))
   elif mode==2:
      bpy.ops.transform.resize(value=( 1, 1,v))
   elif mode==3:   
      bpy.ops.transform.resize(value=( v, v,v))
   return None

class _quick_resize_x(Operator):
    """ Selection group """
    bl_idname = "quick.resizex"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_resize(0)
        return {'FINISHED'}
class _quick_resize_y(Operator):
    """ Selection group """
    bl_idname = "quick.resizey"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_resize(1)
        return {'FINISHED'}
class _quick_resize_z(Operator):
    """ Selection group """
    bl_idname = "quick.resizez"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_resize(2)
        return {'FINISHED'}
class _quick_resize_xyz(Operator):
    """ Selection group """
    bl_idname = "quick.resizexyz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_resize(3)
        return {'FINISHED'}

def quick_translate(mode):
   #if bpy.context.window_manager.clipboard.isnumeric() is False:
      #return None
   v = float(bpy.context.window_manager.clipboard)
   if mode==0:
      bpy.ops.transform.translate(value=(v, 0, 0))
   elif mode==1:
      bpy.ops.transform.translate(value=( 0,v, 0))
   elif mode==2:
      bpy.ops.transform.translate(value=( 0, 0,v))
   elif mode==3:   
      bpy.ops.transform.translate(value=( v, v,v))
   return None


class _quick_translate_x(Operator):
    """ Selection group """
    bl_idname = "quick.translatex"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_translate(0)
        return {'FINISHED'}
class _quick_translate_y(Operator):
    """ Selection group """
    bl_idname = "quick.translatey"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_translate(1)
        return {'FINISHED'}
class _quick_translate_z(Operator):
    """ Selection group """
    bl_idname = "quick.translatez"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_translate(2)
        return {'FINISHED'}
class _quick_translate_xyz(Operator):
    """ Selection group """
    bl_idname = "quick.translatexyz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_translate(3)
        return {'FINISHED'}
def quick_extrude(mode):
   #if bpy.context.window_manager.clipboard.isnumeric() is False:
      #return None
   v = float(bpy.context.window_manager.clipboard)
   if mode==0:
      bpy.ops.mesh.extrude_region_move()
      bpy.ops.transform.translate(value=(v, 0, 0))
   elif mode==1:
      bpy.ops.mesh.extrude_region_move()
      bpy.ops.transform.translate(value=( 0,v, 0))
   elif mode==2:
      bpy.ops.mesh.extrude_region_move()
      bpy.ops.transform.translate(value=( 0, 0,v))
   elif mode==3:   
      bpy.ops.mesh.extrude_region_move()
      bpy.ops.transform.translate(value=( v, v,v))
   return None


class _quick_extrude_x(Operator):
    """ Selection group """
    bl_idname = "quick.extrudex"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_extrude(0)
        return {'FINISHED'}
class _quick_extrude_y(Operator):
    """ Selection group """
    bl_idname = "quick.extrudey"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_extrude(1)
        return {'FINISHED'}
class _quick_extrude_z(Operator):
    """ Selection group """
    bl_idname = "quick.extrudez"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_extrude(2)
        return {'FINISHED'}

class _quick_inset(Operator):
    """ Selection group """
    bl_idname = "quick.inset"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        bpy.ops.mesh.inset(use_even_offset=True, thickness=float(bpy.context.window_manager.clipboard), depth=0)
        return {'FINISHED'}

def quick_rotate(mode):
   if mode==0:
      bpy.ops.transform.rotate(value=1.5708, orient_axis='X', orient_type='GLOBAL')
   elif mode==1:
      bpy.ops.transform.rotate(value=1.5708, orient_axis='Y', orient_type='GLOBAL')
   elif mode==2:    
      bpy.ops.transform.rotate(value=1.5708, orient_axis='Z', orient_type='GLOBAL')
   return None

class _quick_rotate_x(Operator):
    """ Selection group """
    bl_idname = "quick.rotatex"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_rotate(0)
        return {'FINISHED'}
class _quick_rotate_y(Operator):
    """ Selection group """
    bl_idname = "quick.rotatey"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_rotate(1)
        return {'FINISHED'}
class _quick_rotate_z(Operator):
    """ Selection group """
    bl_idname = "quick.rotatez"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "OBJECT" or context.mode == "EDIT_MESH"

    def execute(self, context):
        quick_rotate(2)
        return {'FINISHED'}
class _quick_extrude_normals(Operator):
    """ Quick extrude_normals """
    bl_idname = "quick.extrude_normals"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return context.mode == "EDIT_MESH"

    def execute(self, context):
        value = float(bpy.context.window_manager.clipboard)
        bpy.ops.mesh.extrude_region_shrink_fatten(TRANSFORM_OT_shrink_fatten={"value":value,"use_even_offset":True})
        return {'FINISHED'}

class _align(Panel):
    """将所选对象和其所在的组与光标对齐"""
    bl_label = "对齐"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = "对齐"

    def draw(self, context):
        row = self.layout.row(align=True)
        row.operator(_align_n_x.bl_idname, text="-X")
        row.operator(_align_x.bl_idname, text="X")
        row.operator(_align_p_x.bl_idname, text="+X")
        row = self.layout.row(align=True)
        row.operator(_align_n_y.bl_idname, text="-Y")
        row.operator(_align_y.bl_idname, text="Y")
        row.operator(_align_p_y.bl_idname, text="+Y")
        row = self.layout.row(align=True)
        row.operator(_align_n_z.bl_idname, text="-Z")
        row.operator(_align_z.bl_idname, text="Z")
        row.operator(_align_p_z.bl_idname, text="+Z")
        row = self.layout.row(align=True)
        row.operator(_align_v_x.bl_idname, text="VX")
        row.operator(_align_v_y.bl_idname, text="VY")
        row.operator(_align_v_z.bl_idname, text="VZ")
        row = self.layout.row(align=True)
        row.operator(_modifier_mirror.bl_idname, text="Mirror")
        row.operator(_modifier_bevel.bl_idname, text="Bevel")
        row.operator(_modifier_solidify.bl_idname, text="Solidify")
        row = self.layout.row(align=True)
        row.operator(_select_group.bl_idname, text="选择组")
        row.operator(_duplicate_separate.bl_idname, text="复制分离")
        row.operator(_add_plane.bl_idname, text="新建面")
        row = self.layout.row(align=True)
        row.operator(_duplicate_move_z.bl_idname, text="复制Z")
        row.operator(_duplicate_rotate.bl_idname, text="复制旋转Z")
        row = self.layout.row(align=True)
        row.operator(_loopcut_one.bl_idname, text="分割2")
        row.operator(_loopcut_two.bl_idname, text="分割3")
        row.operator(_loopcut_three.bl_idname, text="分割4")
        row = self.layout.row(align=True)
        row.operator(_loopcut_seven.bl_idname, text="分割")
        row.operator(_quick_extrude_normals.bl_idname, text="拉伸")
        row = self.layout.row(align=True)
        row.operator(_quick_resize_x.bl_idname, text="X")
        row.operator(_quick_resize_y.bl_idname, text="Y")
        row.operator(_quick_resize_z.bl_idname, text="Z")
        row.operator(_quick_resize_xyz.bl_idname, text="XYZ")
        row = self.layout.row(align=True)
        row.operator(_quick_translate_x.bl_idname, text="X")
        row.operator(_quick_translate_y.bl_idname, text="Y")
        row.operator(_quick_translate_z.bl_idname, text="Z")
        row.operator(_quick_translate_xyz.bl_idname, text="XYZ")
        row = self.layout.row(align=True)
        row.operator(_quick_extrude_x.bl_idname, text="X")
        row.operator(_quick_extrude_y.bl_idname, text="Y")
        row.operator(_quick_extrude_z.bl_idname, text="Z")
        row.operator(_quick_inset.bl_idname, text="Inset")
        row = self.layout.row(align=True)
        row.operator(_quick_rotate_x.bl_idname, text="X90")
        row.operator(_quick_rotate_y.bl_idname, text="Y90")
        row.operator(_quick_rotate_z.bl_idname, text="Z90")
        row = self.layout.row(align=True)
        row.operator(_view_axis_left.bl_idname, text="左")
        row.operator(_view_axis_right.bl_idname, text="右")
        row.operator(_view_axis_bottom.bl_idname, text="下")
        row = self.layout.row(align=True)
        row.operator(_view_axis_top.bl_idname, text="上")
        row.operator(_view_axis_front.bl_idname, text="前")
        row.operator(_view_axis_back.bl_idname, text="后")


classes = [
    _align_n_x,
    _align_x,
    _align_p_x,
    _align_n_y,
    _align_y,
    _align_p_y,
    _align_n_z,
    _align_z,
    _align_p_z,
    _align_v_x,
    _align_v_y,
    _align_v_z,
    _modifier_mirror,
    _modifier_bevel,
    _modifier_solidify,
    _select_group,
    _loopcut_one,
    _duplicate_move_z,
    _duplicate_separate,
    _loopcut_three,
    _loopcut_seven,
    _loopcut_two,
    _view_axis_left,
    _view_axis_right,
    _view_axis_bottom,
    _view_axis_top,
    _view_axis_front,
    _view_axis_back,
    _duplicate_rotate,
    _add_plane,
    _quick_resize_x,
    _quick_resize_y,
    _quick_resize_z,
    _quick_resize_xyz,
    _quick_translate_x,
    _quick_translate_y,
    _quick_translate_z,
    _quick_translate_xyz,
    _quick_extrude_x,
    _quick_extrude_y,
    _quick_extrude_z,
    _quick_inset,
    _quick_rotate_x,
    _quick_rotate_y,
    _quick_rotate_z,
    _quick_extrude_normals,
    _align,
]

def register():
    for c in classes:
        bpy.utils.register_class(c)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)


if __name__ == '__main__':
    register()