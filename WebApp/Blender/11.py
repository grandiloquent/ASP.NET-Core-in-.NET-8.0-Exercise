#快捷插件

import bpy
import math
import bmesh
import mathutils

_s = True

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
    bpy.ops.transform.translate(value=(0, 0, translation))

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


def calculate_angle_relative_to_xy():
    """
    Calculates the angle between the vector formed by two selected vertices
    and the XY plane.
    """
    obj = bpy.context.object

    bm = bmesh.from_edit_mesh(obj.data)
    selected_verts = [v for v in bm.verts if v.select]

    if len(selected_verts) != 2:
        print("Error: Please select exactly two vertices.")
        return

    v1_world = obj.matrix_world @ selected_verts[0].co
    v2_world = obj.matrix_world @ selected_verts[1].co

    # Create the vector between the two vertices
    vector = v2_world - v1_world

    # The normal vector of the XY plane is (0, 0, 1)
    plane_normal = mathutils.Vector((0.0, 0.0, 1.0))

    # Calculate the angle between the vector and the plane normal
    # using the dot product formula:
    # dot_product = |vector| * |plane_normal| * cos(angle)
    # cos(angle) = dot_product / (|vector| * |plane_normal|)
    vector_normalized = vector.normalized()
    plane_normal_normalized = plane_normal.normalized()  # Already normalized as length is 1

    dot_product = vector_normalized.dot(plane_normal_normalized)

    # Handle potential floating-point errors that might lead to values slightly outside [-1, 1]
    if dot_product > 1.0:
        dot_product = 1.0
    elif dot_product < -1.0:
        dot_product = -1.0

    angle_radians = math.acos(dot_product)
    angle_degrees = math.degrees(angle_radians)

    # The angle calculated is the angle between the vector and the *normal* of the XY plane.
    # The angle relative to the XY plane itself is 90 degrees minus this angle.
    angle_relative_radians = math.pi / 2 - angle_radians
    angle_relative_degrees = math.degrees(angle_relative_radians)

    print(f"Vertex 1 (World Coords): {v1_world}")
    print(f"Vertex 2 (World Coords): {v2_world}")
    print(f"Vector between vertices: {vector}")
    print(f"Angle with the normal of the XY plane: {angle_degrees:.2f} degrees")
    print(f"Angle relative to the XY plane: {angle_relative_degrees:.2f} degrees")

def recalculate_normals_selected():
    """Recalculates the normals of all selected mesh objects."""
    for obj in bpy.context.selected_objects:
        if obj.type == 'MESH':
            bpy.context.view_layer.objects.active = obj
            bpy.ops.object.mode_set(mode='EDIT')
            bpy.ops.mesh.select_all(action='SELECT')
            bpy.ops.mesh.normals_make_consistent(inside=False)
            bpy.ops.object.mode_set(mode='OBJECT')
    print("Normals recalculated for all selected mesh objects.")




def alignOutputParent(node):
    offset = 100
    outputs = [l for l in node.outputs if len(l.links)>0]
    xn = node.location.x
    global _y
    if _y==0:
        _y = node.location.y;
    for i in outputs:
        tnode = i.links[0].to_node
        x = xn + node.dimensions.x+offset
        tnode.location=mathutils.Vector((x,_y))
        _y =_y - offset - tnode.dimensions.y;
        outputs = [l for l in tnode.outputs if len(l.links)>0]
        if len(outputs)>0 and (node.parent == tnode.parent):
            alignOutputParent(tnode)

def alignNodesParent(node):
    print("alignNodesParent")
    offset = 40
    inputs = [l for l in node.inputs if len(l.links)>0]

    y=node.location.y
    for i in inputs:
        tnode = i.links[0].from_node
        x=node.location.x-offset
        ty=0;
        while True:
                x=x-tnode.dimensions.x
                

                if tnode.dimensions.y>ty:
                    ty=tnode.dimensions.y
                inputs = [l for l in tnode.inputs if len(l.links)>0]
                
                if tnode.parent is not None and node.parent == tnode.parent:
                        
                    print(f'{x}x{y} {node.name} = {node.parent.name} = {tnode.name} = {tnode.parent}')
                    tnode.location=mathutils.Vector((x,y))
                    if len(inputs)>0:
                        tnode=inputs[0].links[0].from_node
                    else:
                        break
                    x=x-offset
                else:
                    break
                
        y=y-ty-offset


def alignNodes(node):
    offset = 40
    inputs = [l for l in node.inputs if len(l.links)>0]

    y=node.location.y
    for i in inputs:
        tnode = i.links[0].from_node
        x=node.location.x-offset
        ty=0;
        while True:
                x=x-tnode.dimensions.x
                

                if tnode.dimensions.y>ty:
                    ty=tnode.dimensions.y
                inputs = [l for l in tnode.inputs if len(l.links)>0]
                
                if tnode.parent is None:
                        
                    #print(f'{x}x{y} {node.name} = {node.parent.name} = {tnode.name} = {tnode.parent}')
                    tnode.location=mathutils.Vector((x,y))
                    if len(inputs)>0:
                        tnode=inputs[0].links[0].from_node
                    else:
                        break
                    x=x-offset
                else:
                    break
                
        y=y-ty-offset
        
class SortNodesInFrame(bpy.types.Operator):
    """ ShaderNode连接 """
    bl_idname = "sn.sortoutputparent"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        print("SortNodesInFrame")

        global _y
        _y=0
        if _s:
                nms = [m for m in bpy.context.active_object.modifiers if m.type=='NODES']
                nodes = [n for n in nms[0].node_group.nodes if n.select]
                node=nodes[0]
                while node.type=="GROUP":
                    nodes = [n for n in node.node_tree.nodes if n.select]
                    node=nodes[0]
                alignNodesParent(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignNodesParent(node);
             
        return {'FINISHED'}
    
class SortNodes(bpy.types.Operator):
    """ ShaderNode连接 """
    bl_idname = "sn.sortnodes"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        
        global _y
        _y=0
        if _s:
                nms = [m for m in bpy.context.active_object.modifiers if m.type=='NODES']
                nodes = [n for n in nms[0].node_group.nodes if n.select]
                node=nodes[0]
                while node.type=="GROUP":
                    nodes = [n for n in node.node_tree.nodes if n.select]
                    node=nodes[0]
                print(node.type)
                alignNodes(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignNodes(node);
             
        return {'FINISHED'}
    
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
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="复制")
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
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="复制")
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
        op_rz = split.operator("object.translate_axis_clipboard_no_prop", text="计算")
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
                bpy.ops.object.duplicate_move_linked()
                s = bpy.context.selected_objects[len(bpy.context.selected_objects)-1]
                for obj in bpy.context.selected_objects:
                    obj.select_set(False)

                s.select_set(True)
                bpy.context.view_layer.objects.active=s
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
                recalculate_normals_selected()
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

            elif self.axis == 'XYZ':
                selected_objects = bpy.context.selected_objects

                if not selected_objects:
                    print("No objects selected. Please select one or more objects to align.")
                else:
                    for obj in selected_objects:
                        align_lowest_to_cursor_z(obj)

            return {'FINISHED'}
#3 
        value=-1
        try:
            value = float(clipboard_text.split()[0])
        except (ValueError, IndexError):
            value=-1

        if self.transform_type == 'QUICK':
                    
            if self.axis == 'X':

                bpy.ops.mesh.primitive_plane_add(enter_editmode=True)
                

            elif self.axis == 'Y':
                bpy.ops.mesh.primitive_cube_add(enter_editmode=True)
              
            elif self.axis == 'Z':
                if value==-1:
                    value=16
                bpy.ops.mesh.primitive_circle_add(vertices=int(value), enter_editmode=True)

            elif self.axis == 'XYZ':
                bpy.ops.mesh.duplicate_move()
                s = bpy.context.selected_objects[len(bpy.context.selected_objects)-1]
                for obj in bpy.context.selected_objects:
                    obj.select_set(False)

                s.select_set(True)
                bpy.context.view_layer.objects.active=s
            return {'FINISHED'}

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
 

            return {'FINISHED'}
            
        else:
            self.report({'WARNING'}, "No mesh object selected.")

        return {'FINISHED'}


class RecalculateNormals(bpy.types.Operator):
    bl_idname = "rn.selected"
    bl_label = "Recalculate Normals"
    bl_options = {"REGISTER", "UNDO"}

    my_string_prop: bpy.props.IntProperty(name="My String")

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):

        recalculate_normals_selected();
             
        return {'FINISHED'}

class DrawCircle(bpy.types.Operator):
    bl_idname = "draw.circle"
    bl_label = "Draw Circle"
    bl_options = {"REGISTER", "UNDO"}

    my_string_prop: bpy.props.IntProperty(name="点数")

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):

        bpy.ops.mesh.primitive_circle_add(vertices=self.my_string_prop, enter_editmode=True)

        return {'FINISHED'}
    
class SimpleSaveOperator(bpy.types.Operator):
    """Simple operator to save the current Blender file."""
    bl_idname = "wm.simple_save"
    bl_label = "Simple Save"

    filepath: bpy.props.StringProperty(
        name="File Path",
        description="Path to save the Blender file",
        default="",
        subtype='FILE_PATH',
    )
    use_relative_path: bpy.props.BoolProperty(
        name="Relative Path",
        description="Save with relative path",
        default=False,
    )
    check_existing: bpy.props.BoolProperty(
        name="Check Existing",
        description="Check and warn on overwriting existing files",
        default=True,
    )
    copy: bpy.props.BoolProperty(
        name="Save Copy",
        description="Save a copy instead of overwriting",
        default=False,
    )

    def execute(self, context):
        try:
            bpy.ops.wm.save_mainfile(
                #filepath=self.filepath,
                #check_existing=self.check_existing,
                #copy=self.copy,
            )
            self.report({'INFO'}, f"File saved to: {self.filepath}")
            return {'FINISHED'}
        except Exception as e:
            self.report({'ERROR'}, f"Error saving file: {e}")
            return {'CANCELLED'}

class BevelEdge(bpy.types.Operator):
    bl_idname = "bevel.edge"
    bl_label = "Bevel Edge"
    bl_options = {"REGISTER", "UNDO"}

    wdith_prop: bpy.props.FloatProperty(name="宽度")
    segments_prop: bpy.props.IntProperty(name="分段")
    verts_prop: bpy.props.BoolProperty(name="点",default=False)

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        if not self.verts_prop:
                bpy.ops.mesh.bevel(offset=self.wdith_prop,segments=self.segments_prop, affect='EDGES')
        else:
                bpy.ops.mesh.bevel(offset=self.wdith_prop,segments=self.segments_prop, affect='VERTICES')

        return {'FINISHED'}
class NodeEditorActions(bpy.types.Operator):
    bl_idname = "node.editor_actions"
    bl_label = "Node Editor Actions"
    bl_options = {"REGISTER", "UNDO"}

    mode_prop:bpy.props.StringProperty(name="模式")

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        if self.mode_prop=="join":
            bpy.ops.node.join()
        elif self.mode_prop=="socket":
            print(f'----------------{self.mode_prop}')
            nms = [m for m in bpy.context.active_object.modifiers if m.type=='NODES']
            n=nms[0].node_group
            clipboard_text = bpy.context.window_manager.clipboard
            lines = clipboard_text.splitlines()
            trimmed_lines = [line.strip() for line in lines]
            string_array = [line for line in trimmed_lines if line]
            panel=n.interface.new_panel(name='Panel')
            for item in string_array:
                nn=n.interface.new_socket(name=item, socket_type="NodeSocketFloat", in_out="INPUT")
                nn.default_value=0
                nn.min_value=0
                n.interface.move_to_parent(nn,panel,0)



        return {'FINISHED'}
           
class MyPieMenu(bpy.types.Menu):
    bl_label = "My Pie Menu"
    bl_idname = "_MT_my.pie_MT_"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = "Object"

    def draw(self, context):
        layout = self.layout
        pie = layout.menu_pie()

        pie.operator("view3d.transform_gizmo_set", text="Move").type = {'TRANSLATE'}

        # You can add more operators or sub-menus here
from typing import Dict,List
addon_key_maps: Dict[bpy.types.KeyMap, List[bpy.types.KeyMapItem]] = {}

classes = [
    SortNodes,
    SortNodesInFrame,
    TransformXYZClipboardPanel,
    TransformAxisClipboardOperator,
    RecalculateNormals,
    DrawCircle,
    SimpleSaveOperator,
    BevelEdge,
    NodeEditorActions
]

def register():
    for c in classes:
        bpy.utils.register_class(c)
    addon_key_config = bpy.context.window_manager.keyconfigs.addon
    if not addon_key_config:
        return


    #key_map = addon_key_config.keymaps.new(name='Window')
    addon_keymaps = []
    wm = bpy.context.window_manager
    for km in wm.keyconfigs.addon.keymaps:
        print(f'>>>>>>>>>>>>>>>>>>>>>>>>{km.name}')
        if km.name == "3D View Generic":
            # Try to find an existing Spacebar mapping to override
            for kmi in km.keymap_items:
                
                if kmi.type == 'W':
                    print(f'v------{kmi.type}')
                    km.keymap_items.remove(kmi)
                elif kmi.type == 'Q':
                    km.keymap_items.remove(kmi)
                elif kmi.type == 'B':
                    km.keymap_items.remove(kmi)
                elif kmi.type == 'V':
                    km.keymap_items.remove(kmi)

            # Create a new keymap item for Spacebar
            km.keymap_items.new(idname=SimpleSaveOperator.bl_idname, type='W', value='PRESS', shift=False)
            kmi = km.keymap_items.new(idname=RecalculateNormals.bl_idname, type='E', value='PRESS', shift=False)
            kmi.properties.my_string_prop=1
            kmi = km.keymap_items.new(idname=DrawCircle.bl_idname, type='Q', value='PRESS', shift=False)
            kmi.properties.my_string_prop=12
            kmi = km.keymap_items.new(idname=BevelEdge.bl_idname, type='B', value='PRESS', shift=False)
            kmi.properties.wdith_prop=0.01
            kmi.properties.segments_prop=1
            kmi.properties.verts_prop=False
            kmi = km.keymap_items.new(idname=BevelEdge.bl_idname, type='V', value='PRESS', shift=False)
            kmi.properties.wdith_prop=0.01
            kmi.properties.segments_prop=1
            kmi.properties.verts_prop=True

            addon_keymaps.append(km)

        
        if km.name == "Node Editor":
            for kmi in km.keymap_items:
                if kmi.type == 'W':
                    km.keymap_items.remove(kmi)

            km.keymap_items.new(idname=SortNodes.bl_idname, type='F3', value='PRESS', shift=False)
            km.keymap_items.new(idname=SortNodesInFrame.bl_idname, type='F1', value='PRESS', shift=False)
            kmi = km.keymap_items.new(idname=NodeEditorActions.bl_idname, type='W', value='PRESS', shift=False)
            kmi.properties.mode_prop="join"
            kmi = km.keymap_items.new(idname=NodeEditorActions.bl_idname, type='E', value='PRESS', shift=False)
            kmi.properties.mode_prop="socket"
            addon_keymaps.append(km)
            
    bpy.types.WindowManager.addon_keymaps = addon_keymaps
    #addon_key_maps[key_map] = []

    #key_map_item = key_map.keymap_items.new(idname=RecalculateNormals.bl_idname, type='E', value='PRESS', shift=False)
    #key_map_item = key_map.keymap_items.new(idname=SimpleSaveOperator.bl_idname, type='W', value='PRESS', shift=False)

    #addon_key_maps[key_map].append(key_map_item)

         
def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)

if __name__ == "__main__":
    register()
# The panel will now appear in the "Object" tab of the 3D Viewport's Sidebar (N-panel)