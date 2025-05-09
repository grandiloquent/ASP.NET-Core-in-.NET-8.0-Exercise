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
                if len(inputs)>0:
                    if tnode.parent is not None and node.parent == tnode.parent:
                        
                        print(f'{x}x{y} {node.name} = {node.parent.name} = {tnode.name} = {tnode.parent}')
                        tnode.location=mathutils.Vector((x,y))
                        tnode=inputs[0].links[0].from_node
                        x=x-offset
                    else:
                        break
                        
                else:
                    break
        y=y-ty-offset


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

    key_map_item = key_map.keymap_items.new(idname=SortNodesInFrame.bl_idname, type='F1', value='PRESS', shift=False)

    addon_key_maps[key_map].append(key_map_item)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)
    
    for key_map, key_map_items in addon_key_maps.items():
        for item in key_map_items:
            key_map.keymap_items.remove(item)

if __name__ == '__main__':
    register()