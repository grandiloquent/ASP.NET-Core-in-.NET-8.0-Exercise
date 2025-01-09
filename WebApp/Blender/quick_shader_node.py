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

def shader(name):
    nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
    node = [n for n in nodes if n.select][0]
    location = node.location
    new=bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new(name)
    new.location = mathutils.Vector((location.x+node.dimensions.x+20,location.y))
    node.select = False
    bpy.context.view_layer.objects.active.active_material.node_tree.links.new(node.outputs[0],new.inputs[0])
    #nodes.active = [n for n in bpy.context.view_layer.objects.active.active_material.node_tree.nodes if n.select][1]
def alignNode(node):
    offset = 100
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
def removeUnusableNodes():
    nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
    node = [n for n in nodes if n.select][0]
    outputs = [l for l in node.outputs if len(l.links)>0]
    for i in outputs:
        for li in i.links:
            tnode = li.to_node
            print(tnode)
            tlinks = [l for l in tnode.outputs if len(l.links)>0]
            if len(tlinks)==0:
                nodes.remove(tnode)

def guessGeometryNodes():
    nodeGroup = [m for m in bpy.context.active_object.modifiers if m.type=='NODES'][0].node_group
    node = [n for n in nodeGroup.nodes if n.select][0]

    if node.type == 'GROUP_INPUT':
        names =['GeometryNodeInstanceOnPoints']
        for i in names:
                n=nodeGroup.nodes.new(i)
                nodeGroup.links.new(node.outputs[0],n.inputs[0])

#bpy.ops.node.add_node(use_transform=True, type="GeometryNodeSetShadeSmooth")

    if node.type == 'GROUP_OUTPUT':
        names =['GeometryNodeSubdivisionSurface',"GeometryNodeSetShadeSmooth"]
        for i in names:
                n=nodeGroup.nodes.new(i)
                nodeGroup.links.new(n.outputs[0],node.inputs[0])    

    if node.type == 'SUBDIVISION_SURFACE':
        names =['GeometryNodeTransform',"GeometryNodeSetPosition"]
        for i in names:
                n=nodeGroup.nodes.new(i)
                nodeGroup.links.new(node.outputs[0],n.inputs[0])  
# bpy.ops.node.add_node(use_transform=True, type="GeometryNodeInputPosition")
    if node.type == 'SET_POSITION':
        names =['ShaderNodeVectorMath']
        for i in names:
                n=nodeGroup.nodes.new(i)
                if i=='ShaderNodeVectorMath':
                    nodeGroup.links.new(n.outputs[0],node.inputs[2])
                    nn=nodeGroup.nodes.new("GeometryNodeInputPosition")          
                    nodeGroup.links.new(nn.outputs[0],n.inputs[0])


class ShaderNodeBump(Operator):
    """ ShaderNodeBump """
    bl_idname = "shadernode.bump"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        node_tree = bpy.context.view_layer.objects.active.active_material.node_tree
        nodes = node_tree.nodes
        links = node_tree.links
        offset = 60
        x = 0
        y = 0

        

        texCoord = nodes.new('ShaderNodeTexCoord')
        mapping = nodes.new('ShaderNodeMapping')
        x=texCoord.location.x+texCoord.dimensions.x+offset
        mapping.location=mathutils.Vector((x,y))
        links.new(texCoord.outputs[3],mapping.inputs[0])

        texNoise = nodes.new('ShaderNodeTexNoise')
        x=mapping.location.x+mapping.dimensions.x+offset
        texNoise.location=mathutils.Vector((x,y))
        links.new(mapping.outputs[0],texNoise.inputs[0])


        valToRGB = nodes.new('ShaderNodeValToRGB')
        x=texNoise.location.x+texNoise.dimensions.x+offset
        valToRGB.location=mathutils.Vector((x,y))
        links.new(texNoise.outputs[0],valToRGB.inputs[0])


        mix = nodes.new('ShaderNodeMix')
        mix.data_type = 'RGBA'
        x=valToRGB.location.x+valToRGB.dimensions.x+offset
        mix.location=mathutils.Vector((x,y))
        links.new(valToRGB.outputs[0],mix.inputs[0])


        bsdf = [n for n in nodes if n.type=='BSDF_PRINCIPLED'][0]
        outMaterial = [n for n in nodes if n.type=='OUTPUT_MATERIAL'][0]
        links.new(mix.outputs[2],bsdf.inputs[0])
        links.new(bsdf.outputs[0],outMaterial.inputs[0])

        # mapping = [n for n in nodes if n.type=='MAPPING'][0]

        # texNoise = nodes.new('ShaderNodeTexNoise')
        # x=mapping.location.x+mapping.dimensions.x+offset
        # links.new(mapping.outputs[0],texNoise.inputs[0])

        # displacement = nodes.new('ShaderNodeDisplacement')
        # x=texNoise.location.x+texNoise.dimensions.x+offset
        # displacement.location=mathutils.Vector((x,y))
        # links.new(texNoise.outputs[0],displacement.inputs[0])

        # links.new(displacement.outputs[0],outMaterial.inputs[2])

        # bpy.context.object.active_material.displacement_method = 'BOTH'

        # texNoise = nodes.new('ShaderNodeTexNoise')
        # x=mapping.location.x+mapping.dimensions.x+offset
        # links.new(mapping.outputs[0],texNoise.inputs[0])

        # bump = nodes.new('ShaderNodeBump')
        # x=texNoise.location.x+texNoise.dimensions.x+offset
        # bump.location=mathutils.Vector((x,y))
        # links.new(texNoise.outputs[0],bump.inputs[2])
        # links.new(bump.outputs[0],bsdf.inputs[5])

        return {'FINISHED'}
class ShaderNodesAlignX(Operator):
    """ ShaderNodesAlignX """
    bl_idname = "shadernodes.alignx"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        # nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
        # nodes = [n for n in nodes if n.select]
        # nodes.sort(key=lambda element: element.location.x)
        # y = nodes[0].location.y
        # offset = 60
        # x = nodes[0].location.x+nodes[0].dimensions.x+offset
        # for i in range(1,len(nodes)):
        #     nodes[i].location=mathutils.Vector((x,y))
        #     x=nodes[i].location.x+nodes[i].dimensions.x+offset
        guessGeometryNodes()
        return {'FINISHED'}
class ShaderNodesAlignY(Operator):
    """ ShaderNodesAlignY """
    bl_idname = "shadernodes.aligny"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
        nodes = [n for n in nodes if n.select]
        nodes.sort(key=lambda element: element.location.y,reverse = True)
        x = nodes[0].location.x
        offset = 60
        y = nodes[0].location.y-nodes[0].dimensions.y-offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            y=nodes[i].location.y-nodes[i].dimensions.y-offset
        return {'FINISHED'}
class ShaderNodeLinkNodes(Operator):
    """ ShaderNode连接 """
    bl_idname = "shadernodes.linknodes"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        # nodes = [n for n in bpy.context.view_layer.objects.active.active_material.node_tree.nodes if n.select]
        # bpy.context.view_layer.objects.active.active_material.node_tree.links.new(nodes[0].outputs[0],nodes[1].inputs[0])
        
        # node = None
        # nms = [m for m in bpy.context.active_object.modifiers if m.type=='NODES']
        # if len(nms)>0:
        #     nodes = [n for n in nms[0].node_group.nodes if n.select]
        #     if len(nodes)>0:
        #         node=nodes[0]

        # if node is None:
        nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
        node =  [n for n in nodes if n.select][0] 
        
        alignNode(node);
             
        return {'FINISHED'}

class ShaderNodeTNC(Operator):
    """ ShaderNodeTNC """
    bl_idname = "shadernode.tnc"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        node_tree = bpy.context.view_layer.objects.active.active_material.node_tree
        nodes = node_tree.nodes
        node =  [n for n in nodes if n.select][0]

        #  [n for n in bpy.context.view_layer.objects.active.active_material.node_tree.nodes if n.select][0].type
        offset=60;
        x=node.location.x+node.dimensions.x+offset;
        y=node.location.y+offset;
        links = node_tree.links
        
        
        if node.type=="TEX_NOISE":
            names =['ShaderNodeValToRGB','ShaderNodeBump',
                'ShaderNodeMix'];
            for i in names:
                n=node_tree.nodes.new(i)
                n.location=mathutils.Vector((x,y))
                y=y+n.dimensions.y+offset*2;
                if i=="ShaderNodeMix":
                    n.data_type = 'RGBA'
                links.new(node.outputs[0],n.inputs[0])

        if node.type=="TEX_VORONOI":
            names =['ShaderNodeValToRGB','ShaderNodeBump',
                'ShaderNodeMix'];
            for i in names:
                n=node_tree.nodes.new(i)
                n.location=mathutils.Vector((x,y))
                y=y+n.dimensions.y+offset*2;
                if i=="ShaderNodeMix":
                    n.data_type = 'RGBA'
                links.new(node.outputs[0],n.inputs[0])

        if node.type=='BSDF_PRINCIPLED':
           
            n=node_tree.nodes.new('ShaderNodeBump')
            n.inputs[0].default_value = 0.1
            n.location=mathutils.Vector((x,y))
            y=y+n.dimensions.y+offset*2;
            links.new(n.outputs[0],node.inputs[5])
            node = n
            n=node_tree.nodes.new('ShaderNodeTexNoise')
            n.inputs[2].default_value = 30
            n.inputs[3].default_value = 15
            n.inputs[4].default_value = .55
            n.location=mathutils.Vector((x,y))
            y=y+n.dimensions.y+offset*2;
            links.new(n.outputs[0],node.inputs[2])
            node = n
            n=node_tree.nodes.new('ShaderNodeMapping')
            n.location=mathutils.Vector((x,y))
            y=y+n.dimensions.y+offset*2;
            links.new(n.outputs[0],node.inputs[0])
            node = n
            n=node_tree.nodes.new('ShaderNodeTexCoord')
            n.location=mathutils.Vector((x,y))
            y=y+n.dimensions.y+offset*2;
            links.new(n.outputs[3],node.inputs[0])
            return {'FINISHED'}

        if node.type=="NEW_GEOMETRY":
            names =['ShaderNodeValToRGB','ShaderNodeBump',
                'ShaderNodeMix'];
            for i in names:
                n=node_tree.nodes.new(i)
                n.location=mathutils.Vector((x,y))
                y=y+n.dimensions.y+offset*2;
                links.new(node.outputs[0],n.inputs[0])

        if node.type=="BUMP":
            names =['ShaderNodeBump'];
            for i in names:
                n=node_tree.nodes.new(i)
                n.location=mathutils.Vector((x,y))
                y=y+n.dimensions.y+offset*2;
                
                if i == "ShaderNodeBump":
                    links.new(node.outputs[0],n.inputs[3])
                else:
                    links.new(node.outputs[0],n.inputs[0])

        if node.type=="MAPPING":
            names =[ 'ShaderNodeTexNoise','ShaderNodeTexVoronoi',
                    "ShaderNodeTexWave"];
            for i in names:
                n=node_tree.nodes.new(i)
                y = 600
                n.location=mathutils.Vector((x,y))
                x=x+n.width+offset
                links.new(node.outputs[0],n.inputs[0])
                if i=="ShaderNodeTexNoise":
                    nn=node_tree.nodes.new('ShaderNodeMix')
                    nn.data_type = 'RGBA'
                    nn.blend_type = 'LINEAR_LIGHT'
                    nn.inputs[0].default_value = 0.02
                    links.new(node.outputs[0],nn.inputs[6])
                    links.new(n.outputs[0],nn.inputs[7])


        if node.type=="VALTORGB":
            names =[ 'ShaderNodeMix'];
            for i in names:
                n=node_tree.nodes.new(i)
                if i=="ShaderNodeMix":
                    n.data_type = 'RGBA'

                n.location=mathutils.Vector((x,y))
                y=y+n.dimensions.y+offset*2;
                links.new(node.outputs[0],n.inputs[0])  

        if node.type=="MIX":
            names =[ 'ShaderNodeMix','ShaderNodeValToRGB'];
            for i in names:
                n=node_tree.nodes.new(i)
                if i=="ShaderNodeMix":
                    n.data_type = 'RGBA'

                n.location=mathutils.Vector((x,y))
                y=y+n.dimensions.y+offset*2;
                if i=="ShaderNodeMix":
                    links.new(node.outputs[2],n.inputs[0])
                else:
                    links.new(node.outputs[2],n.inputs[0])

        return {'FINISHED'}
    
class AreaLight(Operator):
    """ ShaderNodeAreaLight """
    bl_idname = "shadernode.arealight"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        empty = bpy.data.objects.new("Empty", None)
        bpy.context.collection.objects.link(empty)
        empty.empty_display_type = 'PLAIN_AXES'

        light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
        light_data.energy = 500
        light_data.shape = 'DISK'
        light_data.size = 2
        light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
        light_object.location[2] = 5
        bpy.context.collection.objects.link(light_object)
        ttc = light_object.constraints.new(type='TRACK_TO')
        ttc.target = empty
        return {'FINISHED'}
    
class Render(Operator):
    """ ShaderNodeRender """
    bl_idname = "shadernode.render"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        # o = bpy.data.collections.new("MyCollection")
        # bpy.context.scene.collection.children.link(o)
        # bpy.context.view_layer.active_layer_collection = bpy.context.view_layer.layer_collection.children[o.name]
        # cam1 = bpy.data.cameras.new("Camera")
        # obj = bpy.data.objects.new("Camera", cam1)
        # o.objects.link(obj)
        # bpy.context.scene.camera = obj

        # win = bpy.context.window
        # scr = win.screen
        # areas3d  = [area for area in scr.areas if area.type == 'VIEW_3D']
        # region   = [region for region in areas3d[0].regions if region.type == 'WINDOW']


        # # Add cube in the other window.
        # with bpy.context.temp_override(window=win,area=areas3d[0],region=region[0]):
        #     bpy.ops.view3d.camera_to_view()

        # # 背景
        # import bmesh
        # bpy.ops.mesh.primitive_plane_add()
        # bpy.ops.transform.resize(value=(20,20,20))
        # bpy.ops.object.editmode_toggle()
        # bpy.ops.mesh.select_mode(use_extend=False, use_expand=False, type='EDGE')
        # bpy.ops.mesh.select_all(action='DESELECT')
        # bm = bmesh.from_edit_mesh(bpy.context.object.data)
        # bm.edges.ensure_lookup_table()
        # bm.edges[0].select = True
        # bmesh.update_edit_mesh(bpy.context.object.data)
        # bpy.ops.mesh.extrude_region_move(TRANSFORM_OT_translate={"value":(0,0,20)})
        # bpy.ops.mesh.select_all(action='DESELECT')
        # bm.edges.ensure_lookup_table()
        # bm.edges[0].select = True
        # bmesh.update_edit_mesh(bpy.context.object.data)
        # bpy.ops.mesh.bevel(offset=0.4, segments=15, affect='EDGES')
        # bpy.ops.object.editmode_toggle()
        # bpy.ops.object.subdivision_set(level=2, relative=False)
        # bpy.ops.object.shade_smooth()
        # mat = bpy.data.materials.new(name="Material")
        # bpy.context.object.data.materials.append(mat)
        # mat.use_nodes = True
        # mat.node_tree.nodes["Principled BSDF"].inputs[0].default_value = (0.0722839, 0.109457, 0.109454, 1)
        # mat.node_tree.nodes["Principled BSDF"].inputs[2].default_value = 0.897

        # # 灯光

        # light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
        # light_data.energy = 200
        # light_data.shape = 'DISK'
        # light_data.size = 1
        # light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
        # bpy.context.collection.objects.link(light_object)

        # light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
        # light_data.energy = 100
        # light_data.shape = 'DISK'
        # light_data.size = 1
        # light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
        # bpy.context.collection.objects.link(light_object)

        # bpy.context.scene.eevee.use_gtao = True
        # bpy.context.scene.cycles.device = 'GPU'

        # bpy.context.scene.render.resolution_x = 1600
        # bpy.context.scene.render.resolution_y = 1600

        # bpy.context.scene.render.engine = 'CYCLES'
        # bpy.context.scene.cycles.preview_samples = 10
        # bpy.context.scene.cycles.samples = 256
        # bpy.context.scene.cycles.use_preview_denoising = True

        # bpy.context.scene.render.image_settings.file_format = 'FFMPEG'
        # bpy.context.scene.render.ffmpeg.format = 'MPEG4'
        # bpy.context.scene.render.image_settings.file_format = 'PNG'
        # bpy.context.scene.render.image_settings.color_mode = 'RGBA'
        # bpy.context.scene.render.film_transparent = True
        removeUnusableNodes()
        return {'FINISHED'}
class ShaderNodeDisplacement(Operator):
    """ ShaderNodeShaderNodeDisplacement """
    bl_idname = "shadernode.shadernodedisplacement"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        bpy.ops.node.duplicate_move_keep_inputs()
        return {'FINISHED'}
class GuessLinks(Operator):
    """ ShaderNodeGuessLinks """
    bl_idname = "shadernode.guesslinks"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        node_tree = bpy.context.view_layer.objects.active.active_material.node_tree
        nodes = node_tree.nodes
        selected = [n for n in nodes if n.select][0]
        if selected.inputs[0].name == 'Vector':
            mapping = [n for n in nodes if n.type == 'MAPPING'][0]
            node_tree.links.new(mapping.outputs[0],selected.inputs[0])
        
        return {'FINISHED'}
class Init(Operator):
    """ ShaderNodeInit """
    bl_idname = "shadernode.init"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        bpy.ops.object.select_all(action='SELECT')
        bpy.ops.object.delete(use_global=False)
        bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=6, enter_editmode=False, align='WORLD', location=(0, 0, 0), scale=(1, 1, 1))
        bpy.ops.object.shade_smooth()
        bpy.ops.transform.resize(value=(0.2, 0.2, 0.2))
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
        bpy.ops.object.camera_add(enter_editmode=False, align='VIEW', location=(0, 0, 0), rotation=(0, 0, 0), scale=(1, 1, 1))
        bpy.context.object.location[2] = .74
        bpy.context.scene.render.engine = 'CYCLES'
        bpy.context.scene.cycles.device = 'GPU'
        bpy.context.scene.cycles.use_preview_denoising = True
        bpy.context.scene.cycles.preview_denoiser = 'OPTIX'
        bpy.context.scene.cycles.preview_samples = 10
        bpy.context.scene.cycles.samples = 512
        bpy.context.scene.cycles.denoiser = 'OPTIX'
        bpy.context.scene.render.film_transparent = True
        bpy.context.scene.view_settings.view_transform = 'Filmic'
        bpy.context.scene.view_settings.look = 'Very High Contrast'
        bpy.context.scene.render.resolution_x = 1920
        bpy.context.scene.render.resolution_y = 1920
        bpy.ops.object.light_add(type='AREA', radius=1, align='WORLD', location=(0, 0, 0), scale=(1, 1, 1))
        bpy.context.object.rotation_euler[0] = -1.5708
        bpy.context.object.location[0] = 0.74
        bpy.context.object.location[1] = 0.74
        bpy.context.object.rotation_euler[2] = -0.785398
        bpy.context.object.data.energy = 50
        bpy.ops.object.light_add(type='AREA', radius=1, align='WORLD', location=(0, 0, 0), scale=(1, 1, 1))
        bpy.context.object.rotation_euler[0] = -1.5708
        bpy.context.object.location[0] = -0.74
        bpy.context.object.location[1] = -0.74
        bpy.context.object.rotation_euler[2] = 2.35619
        bpy.context.object.data.energy = 50
        return {'FINISHED'}
#1
class _align(Panel):
    """Shader"""
    bl_label = "着色器"
    bl_space_type = "NODE_EDITOR"
    bl_region_type = "UI"
    bl_category = "着色器"

    def draw(self, context):
#4
        row = self.layout.row(align=True)
        row.operator(GuessLinks.bl_idname, text="GuessLinks")
        row.operator(Init.bl_idname, text="Init")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeTNC.bl_idname, text="点")
        row.operator(ShaderNodeDisplacement.bl_idname, text="复制")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeLinkNodes.bl_idname, text="连接") 
        row.operator(AreaLight.bl_idname, text="区域灯")
        row = self.layout.row(align=True)
        row.operator(Render.bl_idname, text="删除")
        row.operator(ShaderNodesAlignY.bl_idname, text="竖对齐")
        row = self.layout.row(align=True)
        row.operator(ShaderNodesAlignX.bl_idname, text="新建")
        row.operator(ShaderNodeBump.bl_idname, text="杂色")
#2
classes = [
    _align,
    ShaderNodeTNC,
    ShaderNodesAlignY,
    ShaderNodeLinkNodes,
    ShaderNodesAlignX,
    ShaderNodeBump,
    AreaLight,
    Render,
    ShaderNodeDisplacement,
    GuessLinks,
    Init#3
]

addon_key_maps: dict[bpy.types.KeyMap, list[bpy.types.KeyMapItem]] = {}

def register():
    for c in classes:
        bpy.utils.register_class(c)
    addon_key_config = bpy.context.window_manager.keyconfigs.addon
    if not addon_key_config:
        return

    key_map = addon_key_config.keymaps.new(name='Window')
    addon_key_maps[key_map] = []

    key_map_item = key_map.keymap_items.new(idname=ShaderNodeLinkNodes.bl_idname, type='F1', value='PRESS', shift=False)
    key_map_item = key_map.keymap_items.new(idname=ShaderNodeTNC.bl_idname, type='F3', value='PRESS', shift=False)
    key_map_item = key_map.keymap_items.new(idname=Render.bl_idname, type='F4', value='PRESS', shift=False)
    key_map_item = key_map.keymap_items.new(idname=ShaderNodesAlignX.bl_idname, type='F5', value='PRESS', shift=False)

    addon_key_maps[key_map].append(key_map_item)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)
    
    for key_map, key_map_items in addon_key_maps.items():
        for item in key_map_items:
            key_map.keymap_items.remove(item)

if __name__ == '__main__':
    register()