#快捷插件

import bpy
import math
import bmesh
import mathutils

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

def align_lowest_to_z_zero(obj):
    """
    Aligns the lowest part of the given object to the global Z-axis zero.

    Args:
        obj: The Blender object to align.
    """

    if obj.type not in ['MESH', 'CURVE', 'SURFACE', 'META', 'FONT', 'ARMATURE', 'LATTICE', 'EMPTY']:
        print(f"Object '{obj.name}' is not a supported type for this operation.")
        return

    # Ensure the object is the active object and in object mode
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.mode_set(mode='OBJECT')

    # Get the world matrix of the object
    world_matrix = obj.matrix_world

    # Calculate the object's bounding box in world space
    bbox_world = [world_matrix @ mathutils.Vector(corner) for corner in obj.bound_box]

    # Find the lowest Z coordinate in world space
    lowest_z = min(v.z for v in bbox_world)

    # Calculate the required translation to move the lowest point to Z=0
    translation = -lowest_z

    # Apply the translation to the object
    bpy.ops.transform.translate(value=(0, 0, translation))

    print(f"Lowest part of '{obj.name}' aligned to Z=0.")
def align_lowest_to_cursor_z(obj):
    """
    Aligns the lowest part of the given object to the 3D cursor's Z-axis location.

    Args:
        obj: The Blender object to align.
    """

    if obj.type not in ['MESH', 'CURVE', 'SURFACE', 'META', 'FONT', 'ARMATURE', 'LATTICE', 'EMPTY']:
        print(f"Object '{obj.name}' is not a supported type for this operation.")
        return

    # Ensure the object is the active object and in object mode
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.mode_set(mode='OBJECT')

    # Get the world matrix of the object
    world_matrix = obj.matrix_world

    # Calculate the object's bounding box in world space
    bbox_world = [world_matrix @ mathutils.Vector(corner) for corner in obj.bound_box]

    # Find the lowest Z coordinate in world space
    lowest_z = min(v.z for v in bbox_world)

    # Get the current Z location of the 3D cursor in world space
    cursor_z = bpy.context.scene.cursor.location.z

    # Calculate the required translation to move the lowest point to the cursor's Z
    translation = cursor_z - lowest_z

    # Apply the translation to the object
    bpy.ops.object.transform(value=(0, 0, translation))

    print(f"Lowest part of '{obj.name}' aligned to the 3D cursor's Z location ({cursor_z:.3f}).")

def align_y_axis_center_to_cursor_y(obj):
    """
    Aligns the center of the object along its local Y-axis to the
    3D cursor's Y-coordinate in global space.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global Y-coordinate
    cursor_y_global = bpy.context.scene.cursor.location.y

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Calculate the object's bounding box in world space
    bbox_world = [obj.matrix_world @ mathutils.Vector(v) for v in obj.bound_box]

    # Find the minimum and maximum Y-coordinates of the bounding box
    min_y_world = min(v.y for v in bbox_world)
    max_y_world = max(v.y for v in bbox_world)

    # Calculate the center Y-coordinate of the bounding box in world space
    center_y_world = (min_y_world + max_y_world) / 2

    # Calculate the required translation along the global Y-axis
    translation_y = cursor_y_global - center_y_world

    # Apply the translation to the object's location
    obj.location.y += translation_y

    print(f"Object '{obj.name}' Y-axis center aligned to cursor's Y-axis.")
 
def align_leftmost_y_to_cursor_y(obj):
    """
    Aligns the leftmost point of the object (along its local Y-axis in world space)
    to the 3D cursor's Y-coordinate in global space.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global Y-coordinate
    cursor_y_global = bpy.context.scene.cursor.location.y

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Calculate the object's bounding box in world space
    bbox_world = [obj.matrix_world @ mathutils.Vector(v) for v in obj.bound_box]

    # Find the minimum Y-coordinate of the bounding box in world space
    min_y_world = min(v.y for v in bbox_world)

    # Calculate the required translation along the global Y-axis
    translation_y = cursor_y_global - min_y_world

    # Apply the translation to the object's location
    obj.location.y += translation_y

    print(f"Object '{obj.name}' leftmost Y aligned to cursor's Y-axis.")
 

def align_rightest_y_to_cursor_y(obj):
    """
    Aligns the vertex with the highest local Y-coordinate of the object's
    mesh to the 3D cursor's global Y-coordinate.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global Y-coordinate
    cursor_y_global = bpy.context.scene.cursor.location.y

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Get the mesh data
    mesh = obj.data

    if not mesh.vertices:
        print(f"Warning: Object '{obj.name}' has no vertices.")
        return

    # Find the vertex with the highest local Y-coordinate
    highest_y_local = float('-inf')
    vertex_to_align = None

    for v in mesh.vertices:
        if v.co.y > highest_y_local:
            highest_y_local = v.co.y
            vertex_to_align = v

    if vertex_to_align is None:
        print(f"Warning: Could not find a vertex in object '{obj.name}'.")
        return

    # Calculate the world space position of the rightest Y vertex
    rightest_y_world = obj.matrix_world @ vertex_to_align.co

    # Calculate the required translation along the global Y-axis
    translation_y = cursor_y_global - rightest_y_world.y

    # Apply the translation to the object's location
    obj.location.y += translation_y

    print(f"Object '{obj.name}' rightest Y aligned to cursor's Y-axis.")
 
 
def select_every_in_collection():
    
    for i, obj in enumerate(bpy.context.collection.objects):
        obj.select_set(True)
        print(f"Selected: {obj.name}")

def select_all_children_in_group():
    for child_obj in bpy.context.active_object.children_recursive:
        child_obj.select_set(True)
def align_x_axis_center_to_cursor_x(obj):
    """
    Aligns the center of the object along its local X-axis to the
    3D cursor's X-coordinate in global space.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global X-coordinate
    cursor_x_global = bpy.context.scene.cursor.location.x

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Calculate the object's bounding box in world space
    bbox_world = [obj.matrix_world @ mathutils.Vector(v) for v in obj.bound_box]

    # Find the minimum and maximum X-coordinates of the bounding box
    min_x_world = min(v.x for v in bbox_world)
    max_x_world = max(v.x for v in bbox_world)

    # Calculate the center X-coordinate of the bounding box in world space
    center_x_world = (min_x_world + max_x_world) / 2

    # Calculate the required translation along the global X-axis
    translation_x = cursor_x_global - center_x_world

    # Apply the translation to the object's location
    obj.location.x += translation_x

    print(f"Object '{obj.name}' X-axis center aligned to cursor's X-axis.")
def align_leftest_x_to_cursor_x(obj):
    """
    Aligns the vertex with the lowest local X-coordinate of the object's
    mesh to the 3D cursor's global X-coordinate.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global X-coordinate
    cursor_x_global = bpy.context.scene.cursor.location.x

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Get the mesh data
    mesh = obj.data

    if not mesh.vertices:
        print(f"Warning: Object '{obj.name}' has no vertices.")
        return

    # Find the vertex with the lowest local X-coordinate
    lowest_x_local = float('inf')
    vertex_to_align = None

    for v in mesh.vertices:
        if v.co.x < lowest_x_local:
            lowest_x_local = v.co.x
            vertex_to_align = v

    if vertex_to_align is None:
        print(f"Warning: Could not find a vertex in object '{obj.name}'.")
        return

    # Calculate the world space position of the leftest X vertex
    leftest_x_world = obj.matrix_world @ vertex_to_align.co

    # Calculate the required translation along the global X-axis
    translation_x = cursor_x_global - leftest_x_world.x

    # Apply the translation to the object's location
    obj.location.x += translation_x

    print(f"Object '{obj.name}' leftest X aligned to cursor's X-axis.")


def align_rightest_x_to_cursor_x(obj):
    """
    Aligns the vertex with the highest local X-coordinate of the object's
    mesh to the 3D cursor's global X-coordinate.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global X-coordinate
    cursor_x_global = bpy.context.scene.cursor.location.x

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Get the mesh data
    mesh = obj.data

    if not mesh.vertices:
        print(f"Warning: Object '{obj.name}' has no vertices.")
        return

    # Find the vertex with the highest local X-coordinate
    highest_x_local = float('-inf')
    vertex_to_align = None

    for v in mesh.vertices:
        if v.co.x > highest_x_local:
            highest_x_local = v.co.x
            vertex_to_align = v

    if vertex_to_align is None:
        print(f"Warning: Could not find a vertex in object '{obj.name}'.")
        return

    # Calculate the world space position of the rightest X vertex
    rightest_x_world = obj.matrix_world @ vertex_to_align.co

    # Calculate the required translation along the global X-axis
    translation_x = cursor_x_global - rightest_x_world.x

    # Apply the translation to the object's location
    obj.location.x += translation_x

    print(f"Object '{obj.name}' rightest X aligned to cursor's X-axis.")


def align_topest_z_to_cursor_z(obj):
    """
    Aligns the vertex with the highest local Z-coordinate of the object's
    mesh to the 3D cursor's global Z-coordinate.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global Z-coordinate
    cursor_z_global = bpy.context.scene.cursor.location.z

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Get the mesh data
    mesh = obj.data

    if not mesh.vertices:
        print(f"Warning: Object '{obj.name}' has no vertices.")
        return

    # Find the vertex with the highest local Z-coordinate
    highest_z_local = float('-inf')
    vertex_to_align = None

    for v in mesh.vertices:
        if v.co.z > highest_z_local:
            highest_z_local = v.co.z
            vertex_to_align = v

    if vertex_to_align is None:
        print(f"Warning: Could not find a vertex in object '{obj.name}'.")
        return

    # Calculate the world space position of the topest Z vertex
    topest_z_world = obj.matrix_world @ vertex_to_align.co

    # Calculate the required translation along the global Z-axis
    translation_z = cursor_z_global - topest_z_world.z

    # Apply the translation to the object's location
    obj.location.z += translation_z

    print(f"Object '{obj.name}' topest Z aligned to cursor's Z-axis.")


def align_z_center_to_cursor_z(obj):
    """
    Aligns the center of the object along its local Z-axis to the
    3D cursor's Z-coordinate in global space.

    Args:
        obj: The Blender object to modify.
    """
    if obj is None or obj.type != 'MESH':
        print("Error: Selected object is not a mesh.")
        return

    # Get the 3D cursor's global Z-coordinate
    cursor_z_global = bpy.context.scene.cursor.location.z

    # Switch to object mode and ensure the object is active and selected
    bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    # Calculate the object's bounding box in world space
    bbox_world = [obj.matrix_world @ mathutils.Vector(v) for v in obj.bound_box]

    # Find the minimum and maximum Z-coordinates of the bounding box
    min_z_world = min(v.z for v in bbox_world)
    max_z_world = max(v.z for v in bbox_world)

    # Calculate the center Z-coordinate of the bounding box in world space
    center_z_world = (min_z_world + max_z_world) / 2

    # Calculate the required translation along the global Z-axis
    translation_z = cursor_z_global - center_z_world

    # Apply the translation to the object's location
    obj.location.z += translation_z

    print(f"Object '{obj.name}' Z-axis center aligned to cursor's Z-axis.")


#5

  
class TransformXYZClipboardPanel(bpy.types.Panel):
    """Creates a Panel to transform based on clipboard data with XYZ split actions (no PointerProperty)."""
    bl_label = "Transform XYZ from Clipboard (No PointerProp)"
    bl_idname = "OBJECT_PT_transform_xyz_clipboard_no_prop"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = "Object"

    clipboard_data: bpy.props.StringProperty(
        name="Clipboard Data",
        description="Text from clipboard (space-separated floats)",
        default="",
    )

    def draw(self, context):
        layout = self.layout

        row = layout.row()
        row.prop(self, "clipboard_data", text="")

        layout.label(text="Translation:")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_tx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_tx.axis = 'X'
        op_tx.transform_type = 'TRANSLATE'
        split = split.split(factor=0.5)
        op_ty = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_ty.axis = 'Y'
        op_ty.transform_type = 'TRANSLATE'
        op_tz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_tz.axis = 'Z'
        op_tz.transform_type = 'TRANSLATE'
        op_tz = split.operator("object.translate_axis_clipboard_no_prop", text="XYZ")
        op_tz.axis = 'XYZ'
        op_tz.transform_type = 'TRANSLATE'
        

        layout.label(text="Scale:")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_sx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_sx.axis = 'X'
        op_sx.transform_type = 'SCALE'
        split = split.split(factor=0.5)
        op_sy = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_sy.axis = 'Y'
        op_sy.transform_type = 'SCALE'
        op_sz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_sz.axis = 'Z'
        op_sz.transform_type = 'SCALE'
        op_sz = split.operator("object.translate_axis_clipboard_no_prop", text="XYZ")
        op_sz.axis = 'XYZ'
        op_sz.transform_type = 'SCALE'

        layout.label(text="Extrude:")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_rx.axis = 'X'
        op_rx.transform_type = 'EXTRUDE'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'EXTRUDE'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'EXTRUDE'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="拉伸")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'EXTRUDE'
        
        layout.label(text="功能")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="复制分离")
        op_rx.axis = 'X'
        op_rx.transform_type = 'OTHER'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Bevel")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'OTHER'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Mirror")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'OTHER'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Solidify")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'OTHER'
        
        layout.label(text="便捷")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="Plane")
        op_rx.axis = 'X'
        op_rx.transform_type = 'QUICK'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Cube")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'QUICK'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="16")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'QUICK'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Z0")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'QUICK'

        layout.label(text="EDGES")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="1")
        op_rx.axis = 'X'
        op_rx.transform_type = 'EDGES'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="2")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'EDGES'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="3")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'EDGES'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="5")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'EDGES'

        layout.label(text="ROTATE")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_rx.axis = 'X'
        op_rx.transform_type = 'ROTATE'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'ROTATE'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'ROTATE'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="全选")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'ROTATE'
        
        layout.label(text="ALIGN_Y")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_rx.axis = 'X'
        op_rx.transform_type = 'ALIGN_Y'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'ALIGN_Y'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'ALIGN_Y'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="XYZ")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'ALIGN_Y'

        layout.label(text="ALIGN_X")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_rx.axis = 'X'
        op_rx.transform_type = 'ALIGN_X'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'ALIGN_X'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'ALIGN_X'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="XYZ")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'ALIGN_X'

        layout.label(text="ALIGN_Z")
        row = layout.row(align=True)
        split = row.split(factor=0.33)
        op_rx = split.operator("object.translate_axis_clipboard_no_prop", text="X")
        op_rx.axis = 'X'
        op_rx.transform_type = 'ALIGN_Z'
        split = split.split(factor=0.5)
        op_ry = split.operator("object.translate_axis_clipboard_no_prop", text="Y")
        op_ry.axis = 'Y'
        op_ry.transform_type = 'ALIGN_Z'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="Z")
        op_rz.axis = 'Z'
        op_rz.transform_type = 'ALIGN_Z'
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="XYZ")
        op_rz.axis = 'XYZ'
        op_rz.transform_type = 'ALIGN_Z'

#1

class TransformAxisClipboardOperator(bpy.types.Operator):
    """Translates, scales, or rotates the active object along a specific axis based on clipboard float."""
    bl_idname = "object.translate_axis_clipboard_no_prop" # Base id, will be adjusted in registration
    bl_label = "Transform Axis"
    bl_options = {'REGISTER', 'UNDO'} # Important: Include 'UNDO'
    axis: bpy.props.EnumProperty(
        items=[('X', "X", "Along X"),
               ('Y', "Y", "Along Y"),
               ('Z', "Z", "Along Z"),
               ('XYZ', "XYZ", "Along XYZ")
               ],
        default='X',
        description="Axis to transform along/around"
    )
    transform_type: bpy.props.EnumProperty(
        items=[('TRANSLATE', "Translate", "Move the object"),
               ('SCALE', "Scale", "Resize the object"),
               ('EXTRUDE', "Rotate", "Rotate the object"),
               ('OTHER', "Other", "Some Actions"),
               ('QUICK', "Quick", "Some Actions"),
               ('EDGES', "Edges", "Some Actions"),
               ('ROTATE', "ROTATE", "Some Actions"),
               ('ALIGN_Y', "ALIGN_Y", "Some Actions"),
               ('ALIGN_X', "ALIGN_X", "Some Actions"),
               ('ALIGN_Z', "ALIGN_Z", "Some Actions")#2
               ],
        default='TRANSLATE',
        description="Type of transformation to apply"
    )

    def execute(self, context):
        #panel = bpy.context.window_manager.addons[__name__.partition('.')[0]].preferences.active_section == 'OBJECT_PT_transform_xyz_clipboard_no_prop'
        clipboard_text = bpy.context.window_manager.clipboard
        
        if self.transform_type == 'OTHER':
                    
            if self.axis == 'X':
                duplicate_separate(1)
                
            elif self.axis == 'Y':
                bpy.ops.object.modifier_add(type='BEVEL')
                bpy.context.object.modifiers["Bevel"].width = 0.022
                bpy.context.object.modifiers["Bevel"].segments = 2

            elif self.axis == 'Z':
                bpy.ops.object.modifier_add(type='MIRROR')

            elif self.axis == 'XYZ':
                bpy.ops.object.modifier_add(type='SOLIDIFY')

            return {'FINISHED'}
        
        
        if self.transform_type == 'QUICK':
                    
            if self.axis == 'X':
                bpy.ops.mesh.primitive_plane_add(enter_editmode=True)

            elif self.axis == 'Y':
                bpy.ops.mesh.primitive_cube_add(enter_editmode=True)

            elif self.axis == 'Z':
                bpy.ops.mesh.primitive_circle_add(vertices=16, enter_editmode=True)

            elif self.axis == 'XYZ':
                selected_objects = bpy.context.selected_objects

                if not selected_objects:
                    print("No objects selected. Please select one or more objects to align.")
                else:
                    for obj in selected_objects:
                        align_lowest_to_cursor_z(obj)

            return {'FINISHED'}
        
        if self.transform_type == 'ROTATE':
                    
            if self.axis == 'X':
                bpy.ops.transform.rotate(value=math.radians(90), constraint_axis=(True, False, False))
            elif self.axis == 'Y':
                bpy.ops.transform.rotate(value=math.radians(90), constraint_axis=(False,True,  False))

            elif self.axis == 'Z':
                bpy.ops.transform.rotate(value=math.radians(90), constraint_axis=(False, False,True))

            elif self.axis == 'XYZ':
                select_all_children_in_group()

            return {'FINISHED'}  

        if self.transform_type == 'ALIGN_Y':
                    
            if self.axis == 'X':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_leftmost_y_to_cursor_y(obj)

            elif self.axis == 'Y':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_y_axis_center_to_cursor_y(obj)

            elif self.axis == 'Z':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_rightest_y_to_cursor_y(obj)
            elif self.axis == 'XYZ':
                selected_objects = bpy.context.selected_objects

                if not selected_objects:
                    print("No objects selected. Please select one or more objects to align.")
                else:
                    for obj in selected_objects:
                        align_lowest_to_cursor_z(obj)

            return {'FINISHED'}

        if self.transform_type == 'ALIGN_X':
                    
            if self.axis == 'X':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_leftest_x_to_cursor_x(obj)

            elif self.axis == 'Y':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_x_axis_center_to_cursor_x(obj)

            elif self.axis == 'Z':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_rightest_x_to_cursor_x(obj)

            elif self.axis == 'XYZ':
                selected_objects = bpy.context.selected_objects

                if not selected_objects:
                    print("No objects selected. Please select one or more objects to align.")
                else:
                    for obj in selected_objects:
                        align_lowest_to_cursor_z(obj)

            return {'FINISHED'}

        if self.transform_type == 'ALIGN_Z':
                    
            if self.axis == 'X':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_topest_z_to_cursor_z(obj)

            elif self.axis == 'Y':
                selected_objects = bpy.context.selected_objects
                if selected_objects:
                    for obj in selected_objects:
                        align_z_center_to_cursor_z(obj)

            elif self.axis == 'Z':
                selected_objects = bpy.context.selected_objects

                if not selected_objects:
                    print("No objects selected. Please select one or more objects to align.")
                else:
                    for obj in selected_objects:
                        align_lowest_to_cursor_z(obj)
                        align_y_axis_center_to_cursor_y(obj)

            elif self.axis == 'XYZ':
                selected_objects = bpy.context.selected_objects

                if not selected_objects:
                    print("No objects selected. Please select one or more objects to align.")
                else:
                    for obj in selected_objects:
                        align_lowest_to_cursor_z(obj)

            return {'FINISHED'}
#3 
        try:
            value = float(clipboard_text.split()[0])
            obj = context.object
            if obj and obj.type == 'MESH':
                if self.transform_type == 'TRANSLATE':
                    if self.axis == 'X':
                        bpy.ops.transform.translate(value=(value, 0, 0))
                        self.report({'INFO'}, f"Translated X by: {value:.3f}")
                    elif self.axis == 'Y':
                        bpy.ops.transform.translate(value=(0, value, 0))
                        self.report({'INFO'}, f"Translated Y by: {value:.3f}")
                    elif self.axis == 'Z':
                        bpy.ops.transform.translate(value=(0, 0, value))
                        self.report({'INFO'}, f"Translated Z by: {value:.3f}")
                    elif self.axis == 'XYZ':
                        bpy.ops.transform.translate(value=(value, value, value))
                        self.report({'INFO'}, f"Translated Z by: {value:.3f}")
                elif self.transform_type == 'SCALE':
                    if self.axis == 'X':
                        bpy.ops.transform.resize(value=(value, 1, 1))
                        self.report({'INFO'}, f"Scaled X by: {value:.3f}")
                    elif self.axis == 'Y':
                        bpy.ops.transform.resize(value=(1, value, 1))
                        self.report({'INFO'}, f"Scaled Y by: {value:.3f}")
                    elif self.axis == 'Z':
                        bpy.ops.transform.resize(value=(1, 1, value))
                        self.report({'INFO'}, f"Scaled Z by: {value:.3f}")
                    elif self.axis == 'XYZ':
                        bpy.ops.transform.resize(value=(value, value, value))
                        self.report({'INFO'}, f"Scaled Z by: {value:.3f}")
                elif self.transform_type == 'EXTRUDE':
                    radians = math.radians(value)
                    if self.axis == 'X':
                        bpy.ops.mesh.extrude_region_move()
                        bpy.ops.transform.translate(value=(value, 0, 0))
                        self.report({'INFO'}, f"Rotated X by: {value:.3f}°")
                    elif self.axis == 'Y':
                        bpy.ops.mesh.extrude_region_move()
                        bpy.ops.transform.translate(value=( 0,value, 0))
                        self.report({'INFO'}, f"Rotated Y by: {value:.3f}°")
                    elif self.axis == 'Z':
                        bpy.ops.mesh.extrude_region_move()
                        bpy.ops.transform.translate(value=( 0, 0,value))
                        self.report({'INFO'}, f"Rotated Z by: {value:.3f}°")
                    elif self.axis == 'XYZ':
                        bpy.ops.mesh.extrude_region_shrink_fatten(TRANSFORM_OT_shrink_fatten={"value":value, "use_even_offset":False})
                        self.report({'INFO'}, f"Rotated Z by: {value:.3f}°")
                elif self.transform_type == 'EDGES':
                    
                    if self.axis == 'X':
                        bpy.ops.mesh.bevel(offset=value, segments=1, affect='EDGES')

                    elif self.axis == 'Y':
                        bpy.ops.mesh.bevel(offset=value, segments=2, affect='EDGES')

                    elif self.axis == 'Z':
                        bpy.ops.mesh.bevel(offset=value, segments=3, affect='EDGES')

                    elif self.axis == 'XYZ':
                        bpy.ops.mesh.bevel(offset=value, segments=5, affect='EDGES')
                  
            else:
                self.report({'WARNING'}, "No mesh object selected.")
        except (ValueError, IndexError):
            self.report({'ERROR'}, "Clipboard does not contain a valid float.")
        return {'FINISHED'}

def register():
    bpy.utils.register_class(TransformXYZClipboardPanel)
    bpy.utils.register_class(TransformAxisClipboardOperator)

def unregister():
    bpy.utils.unregister_class(TransformXYZClipboardPanel)
    bpy.utils.unregister_class(TransformAxisClipboardOperator)

if __name__ == "__main__":
    register()
# The panel will now appear in the "Object" tab of the 3D Viewport's Sidebar (N-panel)