
bl_info = {

"name" : "Shader",
    "description" : "Quick Shader",
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
import mathutils

_s = True

def alignNode(node):
    offset = 40
    inputs = [l for l in node.inputs if len(l.links)>0]
    xn = node.location.x-offset
    y = node.location.y;
    for i in inputs:
        tnode = i.links[0].from_node
        x = xn - tnode.dimensions.x
        tnode.location=mathutils.Vector((x,y))
        y = y - offset - tnode.dimensions.y;
        inputs = [l for l in tnode.inputs if len(l.links)>0]
        if len(inputs)>0:
            alignNode(tnode)

_y=0

def alignParent(node):
    offset = 40
    inputs = [l for l in node.outputs if len(l.links)>0]
    xn = node.location.x+offset
    global _y
    if _y==0:
        _y = node.location.y;
    for i in inputs:
        tnode = i.links[0].to_node
        x = xn + node.dimensions.x
        tnode.location=mathutils.Vector((x,_y))
        print( tnode.location.y)
        _y =_y - offset - tnode.dimensions.y;
        inputs = [l for l in tnode.outputs if len(l.links)>0]
        if len(inputs)>0:
            alignParent(tnode)

def alignOutput(node):
    offset = 40
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
        if len(outputs)>0:
            alignOutput(tnode)
            
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
    offset = 40
    inputs = [l for l in node.inputs if len(l.links)>0]
    xn = node.location.x-offset
    global _y
    if _y==0:
        _y = node.location.y;
    for i in inputs:
        tnode = i.links[0].from_node
        x = xn - tnode.dimensions.x
        tnode.location=mathutils.Vector((x,_y))
        _y =_y - offset - tnode.dimensions.y;
        inputs = [l for l in tnode.inputs if len(l.links)>0]
        if len(inputs)>0 and (node.parent == tnode.parent):
            alignNodesParent(tnode)
            
class SortNodes(Operator):
    """ ShaderNode连接 """
    bl_idname = "sn.sortnodes"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):

        if _s:
                nms = [m for m in bpy.context.active_object.modifiers if m.type=='NODES']
                nodes = [n for n in nms[0].node_group.nodes if n.select]
                node=nodes[0]
                alignNode(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignNode(node);
             
        return {'FINISHED'}

class SortParent(Operator):
    bl_idname = "sn.sortparent"
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
                alignParent(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignParent(node);
        
        return {'FINISHED'}


class SortOutput(Operator):
    """ ShaderNode连接 """
    bl_idname = "sn.sortoutput"
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
                alignOutput(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignOutput(node);

        return {'FINISHED'}

class SortNodesInFrameBack(Operator):
    """ ShaderNode连接 """
    bl_idname = "sn.sortoutputparent"
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
                alignOutputParent(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignOutputParent(node);
             
        return {'FINISHED'}


class SortNodesInFrame(Operator):
    """ ShaderNode连接 """
    bl_idname = "sn.sortoutputparent"
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
                alignNodesParent(node);
        else:
                nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
                node =  [n for n in nodes if n.select][0]                 
                alignNodesParent(node);
             
        return {'FINISHED'}

classes = [
    SortNodesInFrameBack,
    SortParent,
    SortNodes,
    SortNodesInFrame
]
from typing import Dict,List
addon_key_maps: Dict[bpy.types.KeyMap, List[bpy.types.KeyMapItem]] = {}

def register():
    for c in classes:
        bpy.utils.register_class(c)
    addon_key_config = bpy.context.window_manager.keyconfigs.addon
    if not addon_key_config:
        return

    key_map = addon_key_config.keymaps.new(name='Window')
    addon_key_maps[key_map] = []

    key_map_item = key_map.keymap_items.new(idname=SortNodes.bl_idname, type='F1', value='PRESS', shift=False)
    key_map_item = key_map.keymap_items.new(idname=SortNodesInFrame.bl_idname, type='F4', value='PRESS', shift=False)
    key_map_item = key_map.keymap_items.new(idname=SortNodesInFrameBack.bl_idname, type='F3', value='PRESS', shift=False)
    key_map_item = key_map.keymap_items.new(idname=SortParent.bl_idname, type='F5', value='PRESS', shift=False)

    addon_key_maps[key_map].append(key_map_item)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)
    
    for key_map, key_map_items in addon_key_maps.items():
        for item in key_map_items:
            key_map.keymap_items.remove(item)

if __name__ == '__main__':
    register()