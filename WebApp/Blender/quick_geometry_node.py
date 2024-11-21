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

_index =4
def shader(name):
    g = bpy.data.node_groups[_index]
    nodes = g.nodes
    node = [n for n in nodes if n.select][0]
    location = node.location
    new=g.nodes.new(name)
    new.location = mathutils.Vector((location.x+node.dimensions.x+20,location.y))
    node.select = False
    # g.links.new(node.outputs[0],new.inputs[0])
    #nodes.active = [n for n in bpy.context.view_layer.objects.active.active_material.node_tree.nodes if n.select][1]

class GeometryNodeInstanceOnPoints(Operator):
    """ GeometryNodeInstanceOnPoints """
    bl_idname = "geometrynode.nodeinstanceonpoints"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeInstanceOnPoints')
        return {'FINISHED'}
class GeometryNodeJoinGeometry(Operator):
    """ GeometryNodeJoinGeometry """
    bl_idname = "geometrynode.joingeometry"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeJoinGeometry')
        return {'FINISHED'}
class GeometryNodeCurveEndpointSelection(Operator):
    """ GeometryNodeCurveEndpointSelection """
    bl_idname = "geometrynode.curveendpointselection"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurveEndpointSelection')
        return {'FINISHED'}
class GeometryNodeFunctionNodeAlignRotationToVector(Operator):
    """ GeometryNodeFunctionNodeAlignRotationToVector """
    bl_idname = "geometrynode.functionnodealignrotationtovector"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('FunctionNodeAlignRotationToVector')
        return {'FINISHED'}
class GeometryNodeInputTangent(Operator):
    """ GeometryNodeInputTangent """
    bl_idname = "geometrynode.inputtangent"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeInputTangent')
        return {'FINISHED'}
class GeometryNodeFunctionNodeAlignEulerToVector(Operator):
    """ GeometryNodeFunctionNodeAlignEulerToVector """
    bl_idname = "geometrynode.functionnodealigneulertovector"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('FunctionNodeAlignEulerToVector')
        return {'FINISHED'}
class GeometryNodeCurveToMesh(Operator):
    """ GeometryNodeCurveToMesh """
    bl_idname = "geometrynode.curvetomesh"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurveToMesh')
        return {'FINISHED'}
class GeometryNodeCurvePrimitiveCircle(Operator):
    """ GeometryNodeCurvePrimitiveCircle """
    bl_idname = "geometrynode.curveprimitivecircle"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurvePrimitiveCircle')
        return {'FINISHED'}
class GeometryNodeSetCurveRadius(Operator):
    """ GeometryNodeSetCurveRadius """
    bl_idname = "geometrynode.setcurveradius"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSetCurveRadius')
        return {'FINISHED'}
class GeometryNodeSplineParameter(Operator):
    """ GeometryNodeSplineParameter """
    bl_idname = "geometrynode.splineparameter"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSplineParameter')
        return {'FINISHED'}
class GeometryNodeShaderNodeValToRGB(Operator):
    """ GeometryNodeShaderNodeValToRGB """
    bl_idname = "geometrynode.shadernodevaltorgb"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeValToRGB')
        return {'FINISHED'}
class GeometryNodeObjectInfo(Operator):
    """ GeometryNodeObjectInfo """
    bl_idname = "geometrynode.objectinfo"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeObjectInfo')
        return {'FINISHED'}
class GeometryNodeDistributePointsOnFaces(Operator):
    """ GeometryNodeDistributePointsOnFaces """
    bl_idname = "geometrynode.distributepointsonfaces"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeDistributePointsOnFaces')
        return {'FINISHED'}
class GeometryNodeTrimCurve(Operator):
    """ GeometryNodeTrimCurve """
    bl_idname = "geometrynode.trimcurve"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeTrimCurve')
        return {'FINISHED'}
class GeometryNodeFunctionNodeRandomValue(Operator):
    """ GeometryNodeFunctionNodeRandomValue """
    bl_idname = "geometrynode.functionnoderandomvalue"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('FunctionNodeRandomValue')
        return {'FINISHED'}
class GeometryNodeCurvePrimitiveLine(Operator):
    """ GeometryNodeCurvePrimitiveLine """
    bl_idname = "geometrynode.curveprimitiveline"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurvePrimitiveLine')
        return {'FINISHED'}
class GeometryNodeResampleCurve(Operator):
    """ GeometryNodeResampleCurve """
    bl_idname = "geometrynode.resamplecurve"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeResampleCurve')
        return {'FINISHED'}
class GeometryNodeSetPosition(Operator):
    """ GeometryNodeSetPosition """
    bl_idname = "geometrynode.setposition"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSetPosition')
        return {'FINISHED'}
class GeometryNodeAlignX(Operator):
    """ GeometryNodeAlignX """
    bl_idname = "geometrynode.alignx"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        nodes = bpy.data.node_groups[_index].nodes
        nodes = [n for n in nodes if n.select]
        nodes.sort(key=lambda element: element.location.x)
        y = nodes[0].location.y
        offset = 120
        x = nodes[0].location.x+nodes[0].dimensions.x+offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            x=nodes[i].location.x+nodes[i].dimensions.x+offset
        return {'FINISHED'}
class GeometryNodeShaderNodeTexNoise(Operator):
    """ GeometryNodeShaderNodeTexNoise """
    bl_idname = "geometrynode.shadernodetexnoise"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexNoise')
        return {'FINISHED'}
class GeometryNodeShaderNodeVectorMath(Operator):
    """ GeometryNodeShaderNodeVectorMath """
    bl_idname = "geometrynode.shadernodevectormath"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeVectorMath')
        return {'FINISHED'}
class GeometryNodeRotateInstances(Operator):
    """ GeometryNodeRotateInstances """
    bl_idname = "geometrynode.rotateinstances"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeRotateInstances')
        return {'FINISHED'}
class GeometryNodeAlignY(Operator):
    """ GeometryNodeAlignY """
    bl_idname = "geometrynode.aligny"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        nodes = bpy.data.node_groups[_index].nodes
        nodes = [n for n in nodes if n.select]
        nodes.sort(key=lambda element: element.location.y,reverse = True)
        x = nodes[0].location.x
        offset = 40
        y = nodes[0].location.y-nodes[0].dimensions.y-offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            y=nodes[i].location.y-nodes[i].dimensions.y-offset
        return {'FINISHED'}
#1
class _align(Panel):
    """Shader"""
    bl_label = "几何"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = "几何"

    def draw(self, context):
        row = self.layout.row(align=True)
        row.operator(GeometryNodeAlignX.bl_idname, text="对齐X")
        row.operator(GeometryNodeAlignY.bl_idname, text="对齐Y")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeFunctionNodeAlignEulerToVector.bl_idname, text="AlignEulerToVector")
        row.operator(GeometryNodeFunctionNodeAlignRotationToVector.bl_idname, text="AlignRotationToVector")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeShaderNodeValToRGB.bl_idname, text="ColorRamp")
        row.operator(GeometryNodeCurvePrimitiveCircle.bl_idname, text="CurveCircle")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeCurvePrimitiveLine.bl_idname, text="CurveLine")
        row.operator(GeometryNodeInputTangent.bl_idname, text="CurveTangent")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeCurveToMesh.bl_idname, text="CurveToMesh")
        row.operator(GeometryNodeDistributePointsOnFaces.bl_idname, text="DistributePointsOnFaces")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeCurveEndpointSelection.bl_idname, text="EndpointSelection")
        row.operator(GeometryNodeInstanceOnPoints.bl_idname, text="InstanceOnPoints")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeJoinGeometry.bl_idname, text="JoinGeometry")
        row.operator(GeometryNodeShaderNodeTexNoise.bl_idname, text="Noise")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeObjectInfo.bl_idname, text="ObjectInfo")
        row.operator(GeometryNodeFunctionNodeRandomValue.bl_idname, text="RandomValue")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeResampleCurve.bl_idname, text="ResampleCurve")
        row.operator(GeometryNodeRotateInstances.bl_idname, text="RotateInstances")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeSetCurveRadius.bl_idname, text="SetCurveRadius")
        row.operator(GeometryNodeSetPosition.bl_idname, text="SetPosition")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeSplineParameter.bl_idname, text="SplineParameter")
        row.operator(GeometryNodeTrimCurve.bl_idname, text="TrimCurve")

        row = self.layout.row(align=True)
        row.operator(GeometryNodeShaderNodeVectorMath.bl_idname, text="VectorMath")

#2
classes = [
    _align,
    
    GeometryNodeInstanceOnPoints,
    GeometryNodeJoinGeometry,
    GeometryNodeCurveEndpointSelection,
    GeometryNodeFunctionNodeAlignRotationToVector,
    GeometryNodeInputTangent,
    GeometryNodeFunctionNodeAlignEulerToVector,
    GeometryNodeCurveToMesh,
    GeometryNodeCurvePrimitiveCircle,
    GeometryNodeSetCurveRadius,
    GeometryNodeSplineParameter,
    GeometryNodeShaderNodeValToRGB,
    GeometryNodeObjectInfo,
    GeometryNodeDistributePointsOnFaces,
    GeometryNodeTrimCurve,
    GeometryNodeFunctionNodeRandomValue,
    GeometryNodeCurvePrimitiveLine,
    GeometryNodeResampleCurve,
    GeometryNodeSetPosition,
    GeometryNodeAlignX,
    GeometryNodeShaderNodeTexNoise,
    GeometryNodeShaderNodeVectorMath,
    GeometryNodeRotateInstances,
    GeometryNodeAlignY,
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