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
        y = nodes[0].location.y
        offset = 20
        x = nodes[0].location.x+nodes[0].dimensions.x+offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            x=nodes[i].location.x+nodes[i].dimensions.x+offset
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
        return {'FINISHED'}
#1
class _align(Panel):
    """Shader"""
    bl_label = "着色器"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = "着色器"

    def draw(self, context):
        row = self.layout.row(align=True)
        row.operator(ShaderNodeMixRGB.bl_idname, text="Mix")
        row.operator(ShaderNodeValToRGB.bl_idname, text="ColorRamp")
        row.operator(ShaderNodeMath.bl_idname, text="Math")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeTexImage.bl_idname, text="Image")
        row.operator(ShaderNodeBump.bl_idname, text="Bump")
        row.operator(ShaderNodeMixShader.bl_idname, text="MixShader")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeTexNoise.bl_idname, text="Noise")
        row.operator(ShaderNodeTexVoronoi.bl_idname, text="Voronoi")
        row.operator(ShaderNodeTexMusgrave.bl_idname, text="Musgrave")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeTexGradient.bl_idname, text="Gradient")
        row.operator(ShaderNodeMapRange.bl_idname, text="MapRange")
        row.operator(ShaderNodeLayerWeight.bl_idname, text="LayerWeight")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeEmission.bl_idname, text="Emission")
        row.operator(ShaderNodeSeparateXYZ.bl_idname, text="SeparateXYZ")
        row.operator(ShaderNodeCombineXYZ.bl_idname, text="CombineXYZ")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeMapping.bl_idname, text="Mapping")
        row.operator(ShaderNodeInvert.bl_idname, text="Invert")
        row.operator(ShaderNodeBsdfPrincipled.bl_idname, text="Principled")
        row = self.layout.row(align=True)
        row.operator(ShaderNodesAlignX.bl_idname, text="横对齐")
        row.operator(ShaderNodeLinkNodes.bl_idname, text="连接")
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
    ShaderNodeLinkNodes,
    ShaderNodeMapping,
    ShaderNodeInvert,
    ShaderNodeBsdfPrincipled,
    ShaderNodeTNC,
#3
]

def register():
    for c in classes:
        bpy.utils.register_class(c)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)

if __name__ == '__main__':
    register()