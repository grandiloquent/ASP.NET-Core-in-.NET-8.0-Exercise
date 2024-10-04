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

class ShaderNodeMixRGB(Operator):
    """ ShaderNodeMixRGB """
    bl_idname = "shadernode.mixrgb"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeMixRGB')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeValToRGB')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexNoise')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexVoronoi')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexMusgrave')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeBump')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexImage')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeTexGradient')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeMixShader')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeMath')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeMapRange')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeLayerWeight')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeEmission')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeSeparateXYZ')
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
        bpy.context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeCombineXYZ')
        return {'FINISHED'}
#1
class _align(Panel):
    """Shader"""
    bl_label = "Shader"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = "Shader"

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