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

class ShaderNodeMixRGB(Operator):
    """ ShaderNodeMixRGB """
    bl_idname = "shadernode.mixrgb"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMixRGB')
        return {'FINISHED'}
class ShaderNodeValToRGB(Operator):
    """ ShaderNodeValToRGB """
    bl_idname = "shadernode.valtorgb"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeValToRGB')
        return {'FINISHED'}
class ShaderNodeTexNoise(Operator):
    """ ShaderNodeTexNoise """
    bl_idname = "shadernode.texnoise"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexNoise')
        return {'FINISHED'}
class ShaderNodeTexVoronoi(Operator):
    """ ShaderNodeTexVoronoi """
    bl_idname = "shadernode.texvoronoi"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexVoronoi')
        return {'FINISHED'}
class ShaderNodeTexMusgrave(Operator):
    """ ShaderNodeTexMusgrave """
    bl_idname = "shadernode.texmusgrave"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexMusgrave')
        return {'FINISHED'}
class ShaderNodeBump(Operator):
    """ ShaderNodeBump """
    bl_idname = "shadernode.bump"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeBump')
        return {'FINISHED'}
class ShaderNodeTexImage(Operator):
    """ ShaderNodeTexImage """
    bl_idname = "shadernode.teximage"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexImage')
        return {'FINISHED'}
class ShaderNodeTexGradient(Operator):
    """ ShaderNodeTexGradient """
    bl_idname = "shadernode.texgradient"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexGradient')
        return {'FINISHED'}
class ShaderNodeMixShader(Operator):
    """ ShaderNodeMixShader """
    bl_idname = "shadernode.mixshader"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMixShader')
        return {'FINISHED'}
class ShaderNodeMath(Operator):
    """ ShaderNodeMath """
    bl_idname = "shadernode.math"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMath')
        return {'FINISHED'}
class ShaderNodeMapRange(Operator):
    """ ShaderNodeMapRange """
    bl_idname = "shadernode.maprange"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMapRange')
        return {'FINISHED'}
class ShaderNodeLayerWeight(Operator):
    """ ShaderNodeLayerWeight """
    bl_idname = "shadernode.layerweight"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeLayerWeight')
        return {'FINISHED'}
class ShaderNodeEmission(Operator):
    """ ShaderNodeEmission """
    bl_idname = "shadernode.emission"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeEmission')
        return {'FINISHED'}
class ShaderNodeSeparateXYZ(Operator):
    """ ShaderNodeSeparateXYZ """
    bl_idname = "shadernode.separatexyz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeSeparateXYZ')
        return {'FINISHED'}
class ShaderNodeCombineXYZ(Operator):
    """ ShaderNodeCombineXYZ """
    bl_idname = "shadernode.combinexyz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeCombineXYZ')
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
        nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
        nodes = [n for n in nodes if n.select]
        nodes.sort(key=lambda element: element.location.x)
        y = nodes[0].location.y
        offset = 60
        x = nodes[0].location.x+nodes[0].dimensions.x+offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            x=nodes[i].location.x+nodes[i].dimensions.x+offset
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
        nodes = [n for n in bpy.context.view_layer.objects.active.active_material.node_tree.nodes if n.select]
        bpy.context.view_layer.objects.active.active_material.node_tree.links.new(nodes[0].outputs[0],nodes[1].inputs[0])
        return {'FINISHED'}
class ShaderNodeMapping(Operator):
    """ ShaderNodeMapping """
    bl_idname = "shadernode.mapping"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMapping')
        return {'FINISHED'}
class ShaderNodeInvert(Operator):
    """ ShaderNodeInvert """
    bl_idname = "shadernode.invert"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeInvert')
        return {'FINISHED'}
class ShaderNodeBsdfPrincipled(Operator):
    """ ShaderNodeBsdfPrincipled """
    bl_idname = "shadernode.bsdfprincipled"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeBsdfPrincipled')
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
        texCoord = bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexCoord')
        texNoise = texNoise=bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexNoise')
        valToRGB=bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeValToRGB')
        bpy.context.view_layer.objects.active.active_material.node_tree.links.new(texCoord.outputs[3],texNoise.inputs[0])
        bpy.context.view_layer.objects.active.active_material.node_tree.links.new(texNoise.outputs[0],valToRGB.inputs[0])
        offset = 60
        texNoise.location=mathutils.Vector((texCoord.location.x+texCoord.dimensions.x+offset,texCoord.location.y))
        valToRGB.location=mathutils.Vector((texNoise.location.x+texNoise.dimensions.x+offset,texCoord.location.y))
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
        o = bpy.data.collections.new("MyCollection")
        bpy.context.scene.collection.children.link(o)
        bpy.context.view_layer.active_layer_collection = bpy.context.view_layer.layer_collection.children[o.name]
        cam1 = bpy.data.cameras.new("Camera")
        obj = bpy.data.objects.new("Camera", cam1)
        o.objects.link(obj)
        bpy.context.scene.camera = obj

        win = bpy.context.window
        scr = win.screen
        areas3d  = [area for area in scr.areas if area.type == 'VIEW_3D']
        region   = [region for region in areas3d[0].regions if region.type == 'WINDOW']


        # Add cube in the other window.
        with bpy.context.temp_override(window=win,area=areas3d[0],region=region[0]):
            bpy.ops.view3d.camera_to_view()

        # 背景
        import bmesh
        bpy.ops.mesh.primitive_plane_add()
        bpy.ops.transform.resize(value=(20,20,20))
        bpy.ops.object.editmode_toggle()
        bpy.ops.mesh.select_mode(use_extend=False, use_expand=False, type='EDGE')
        bpy.ops.mesh.select_all(action='DESELECT')
        bm = bmesh.from_edit_mesh(bpy.context.object.data)
        bm.edges.ensure_lookup_table()
        bm.edges[0].select = True
        bmesh.update_edit_mesh(bpy.context.object.data)
        bpy.ops.mesh.extrude_region_move(TRANSFORM_OT_translate={"value":(0,0,20)})
        bpy.ops.mesh.select_all(action='DESELECT')
        bm.edges.ensure_lookup_table()
        bm.edges[0].select = True
        bmesh.update_edit_mesh(bpy.context.object.data)
        bpy.ops.mesh.bevel(offset=0.4, segments=15, affect='EDGES')
        bpy.ops.object.editmode_toggle()
        bpy.ops.object.subdivision_set(level=2, relative=False)
        bpy.ops.object.shade_smooth()
        mat = bpy.data.materials.new(name="Material")
        bpy.context.object.data.materials.append(mat)
        mat.use_nodes = True
        mat.node_tree.nodes["Principled BSDF"].inputs[0].default_value = (0.0722839, 0.109457, 0.109454, 1)
        mat.node_tree.nodes["Principled BSDF"].inputs[2].default_value = 0.897

        # 灯光

        light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
        light_data.energy = 200
        light_data.shape = 'DISK'
        light_data.size = 1
        light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
        bpy.context.collection.objects.link(light_object)

        light_data = bpy.data.lights.new(name="my-light-data", type='AREA')
        light_data.energy = 100
        light_data.shape = 'DISK'
        light_data.size = 1
        light_object = bpy.data.objects.new(name="my-light", object_data=light_data)
        bpy.context.collection.objects.link(light_object)

        bpy.context.scene.eevee.use_gtao = True
        bpy.context.scene.cycles.device = 'GPU'

        bpy.context.scene.render.resolution_x = 1600
        bpy.context.scene.render.resolution_y = 1600

        bpy.context.scene.render.engine = 'CYCLES'
        bpy.context.scene.cycles.preview_samples = 10
        bpy.context.scene.cycles.samples = 256
        bpy.context.scene.cycles.use_preview_denoising = True

        bpy.context.scene.render.image_settings.file_format = 'FFMPEG'
        bpy.context.scene.render.ffmpeg.format = 'MPEG4'
        bpy.context.scene.render.image_settings.file_format = 'PNG'
        bpy.context.scene.render.image_settings.color_mode = 'RGBA'
        bpy.context.scene.render.film_transparent = True
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
        row.operator(AreaLight.bl_idname, text="AreaLight")
        row.operator(ShaderNodeBump.bl_idname, text="Bump")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeValToRGB.bl_idname, text="ColorRamp")
        row.operator(ShaderNodeCombineXYZ.bl_idname, text="CombineXYZ")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeEmission.bl_idname, text="Emission")
        row.operator(ShaderNodeTexGradient.bl_idname, text="Gradient")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeTexImage.bl_idname, text="Image")
        row.operator(ShaderNodeInvert.bl_idname, text="Invert")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeLayerWeight.bl_idname, text="LayerWeight")
        row.operator(ShaderNodeMapping.bl_idname, text="Mapping")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeMapRange.bl_idname, text="MapRange")
        row.operator(ShaderNodeMath.bl_idname, text="Math")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeMixRGB.bl_idname, text="Mix")
        row.operator(ShaderNodeMixShader.bl_idname, text="MixShader")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeTexMusgrave.bl_idname, text="Musgrave")
        row.operator(ShaderNodeTexNoise.bl_idname, text="Noise")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeBsdfPrincipled.bl_idname, text="Principled")
        row.operator(Render.bl_idname, text="渲染")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeSeparateXYZ.bl_idname, text="SeparateXYZ")
        row.operator(ShaderNodeTexVoronoi.bl_idname, text="Voronoi")
        row = self.layout.row(align=True)
        row.operator(ShaderNodesAlignX.bl_idname, text="横对齐")
        row.operator(ShaderNodeLinkNodes.bl_idname, text="连接")
        row = self.layout.row(align=True)
        row.operator(ShaderNodesAlignY.bl_idname, text="竖对齐")
        row.operator(ShaderNodeTNC.bl_idname, text="坐标杂色映射")
#2
classes = [
    _align,
    ShaderNodeMixRGB,
    ShaderNodeValToRGB,
    ShaderNodeTexNoise,
    ShaderNodeTexVoronoi,
    ShaderNodeTexMusgrave,
    ShaderNodeBump,
    ShaderNodeTexImage,
    ShaderNodeTexGradient,
    ShaderNodeMixShader,
    ShaderNodeMath,
    ShaderNodeMapRange,
    ShaderNodeLayerWeight,
    ShaderNodeEmission,
    ShaderNodeSeparateXYZ,
    ShaderNodeCombineXYZ,
    ShaderNodesAlignX,
    ShaderNodesAlignY,
    ShaderNodeLinkNodes,
    ShaderNodeMapping,
    ShaderNodeInvert,
    ShaderNodeBsdfPrincipled,
    ShaderNodeTNC,
    AreaLight,
    Render#3
]

def register():
    for c in classes:
        bpy.utils.register_class(c)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)

if __name__ == '__main__':
    register()