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

_index =1
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
        offset = 60
        x = nodes[0].location.x+nodes[0].dimensions.x+offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            x=nodes[i].location.x+nodes[i].dimensions.x+offset
        return {'FINISHED'}
class GeometryNodeAlignNX(Operator):
    """ GeometryNodeAlignNX """
    bl_idname = "geometrynode.alignnx"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        nodes = bpy.data.node_groups[_index].nodes
        nodes = [n for n in nodes if n.select]
        nodes.sort(key=lambda element: element.location.x,reverse=True)
        y = nodes[0].location.y
        offset = 60
        x = nodes[0].location.x-nodes[0].dimensions.x-offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            x=nodes[i].location.x-nodes[i].dimensions.x-offset
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
class GeometryNodeAlignNY(Operator):
    """ GeometryNodeAlignNY """
    bl_idname = "geometrynode.alignny"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        nodes = bpy.data.node_groups[_index].nodes
        nodes = [n for n in nodes if n.select]
        nodes.sort(key=lambda element: element.location.y)
        x = nodes[0].location.x
        offset = 40
        y = nodes[0].location.y+nodes[0].dimensions.y+offset
        for i in range(1,len(nodes)):
            nodes[i].location=mathutils.Vector((x,y))
            y=nodes[i].location.y+nodes[i].dimensions.y+offset
        return {'FINISHED'}
    
class GeometryNodeMeshIcoSphere(Operator):
    """ GeometryNodeMeshIcoSphere """
    bl_idname = "geometrynode.meshicosphere"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeMeshIcoSphere')
        return {'FINISHED'}
class GeometryNodeInputSceneTime(Operator):
    """ GeometryNodeInputSceneTime """
    bl_idname = "geometrynode.inputscenetime"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeInputSceneTime')
        return {'FINISHED'}
class GeometryNodeShaderNodeCombineXYZ(Operator):
    """ GeometryNodeShaderNodeCombineXYZ """
    bl_idname = "geometrynode.shadernodecombinexyz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeCombineXYZ')
        return {'FINISHED'}
class GeometryNodeCaptureAttribute(Operator):
    """ GeometryNodeCaptureAttribute """
    bl_idname = "geometrynode.captureattribute"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCaptureAttribute')
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
class GeometryNodeSetMaterial(Operator):
    """ GeometryNodeSetMaterial """
    bl_idname = "geometrynode.setmaterial"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSetMaterial')
        return {'FINISHED'}
class ShaderNodeMath(Operator):
    """ GeometryNodeShaderNodeMath """
    bl_idname = "geometrynode.shadernodemath"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMath')
        return {'FINISHED'}
class ShaderNodeSeparateXYZ(Operator):
    """ GeometryNodeShaderNodeSeparateXYZ """
    bl_idname = "geometrynode.shadernodeseparatexyz"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeSeparateXYZ')
        return {'FINISHED'}
class ShaderNodeMapRange(Operator):
    """ GeometryNodeShaderNodeMapRange """
    bl_idname = "geometrynode.shadernodemaprange"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMapRange')
        return {'FINISHED'}
class ShaderNodeValue(Operator):
    """ GeometryNodeShaderNodeValue """
    bl_idname = "geometrynode.shadernodevalue"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeValue')
        return {'FINISHED'}
class GeometryNodeRealizeInstances(Operator):
    """ GeometryNodeGeometryNodeRealizeInstances """
    bl_idname = "geometrynode.realizeinstances"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeRealizeInstances')
        return {'FINISHED'}
class GeometryNodeSetShadeSmooth(Operator):
    """ GeometryNodeGeometryNodeSetShadeSmooth """
    bl_idname = "geometrynode.setshadesmooth"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSetShadeSmooth')
        return {'FINISHED'}
class GeometryNodeCollectionInfo(Operator):
    """ GeometryNodeGeometryNodeCollectionInfo """
    bl_idname = "geometrynode.collectioninfo"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCollectionInfo')
        return {'FINISHED'}
class GeometryNodeSampleNearest(Operator):
    """ GeometryNodeGeometryNodeSampleNearest """
    bl_idname = "geometrynode.samplenearest"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSampleNearest')
        return {'FINISHED'}
class GeometryNodeSampleIndex(Operator):
    """ GeometryNodeGeometryNodeSampleIndex """
    bl_idname = "geometrynode.sampleindex"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSampleIndex')
        return {'FINISHED'}
class GeometryNodeInputNormal(Operator):
    """ GeometryNodeGeometryNodeInputNormal """
    bl_idname = "geometrynode.inputnormal"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeInputNormal')
        return {'FINISHED'}
class GeometryNodeProximity(Operator):
    """ GeometryNodeGeometryNodeProximity """
    bl_idname = "geometrynode.proximity"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeProximity')
        return {'FINISHED'}
class ShaderNodeMix(Operator):
    """ GeometryNodeShaderNodeMix """
    bl_idname = "geometrynode.shadernodemix"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeMix')
        return {'FINISHED'}
class GeometryNodeCurveSetHandles(Operator):
    """ GeometryNodeGeometryNodeCurveSetHandles """
    bl_idname = "geometrynode.curvesethandles"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurveSetHandles')
        return {'FINISHED'}
class GeometryNodeSetCurveHandlePositions(Operator):
    """ GeometryNodeGeometryNodeSetCurveHandlePositions """
    bl_idname = "geometrynode.setcurvehandlepositions"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeSetCurveHandlePositions')
        return {'FINISHED'}
class GeometryNodeCurveSplineType(Operator):
    """ GeometryNodeGeometryNodeCurveSplineType """
    bl_idname = "geometrynode.curvesplinetype"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurveSplineType')
        return {'FINISHED'}
class NodeGroupInput(Operator):
    """ GeometryNodeNodeGroupInput """
    bl_idname = "geometrynode.nodegroupinput"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('NodeGroupInput')
        return {'FINISHED'}
class GeometryNodeDeleteGeometry(Operator):
    """ GeometryNodeGeometryNodeDeleteGeometry """
    bl_idname = "geometrynode.deletegeometry"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeDeleteGeometry')
        return {'FINISHED'}
class GeometryNodeCurveToPoints(Operator):
    """ GeometryNodeGeometryNodeCurveToPoints """
    bl_idname = "geometrynode.curvetopoints"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeCurveToPoints')
        return {'FINISHED'}
class GeometryNodeTransform(Operator):
    """ GeometryNodeGeometryNodeTransform """
    bl_idname = "geometrynode.transform"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeTransform')
        return {'FINISHED'}
class FunctionNodeRotationToEuler(Operator):
    """ GeometryNodeFunctionNodeRotationToEuler """
    bl_idname = "geometrynode.functionnoderotationtoeuler"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('FunctionNodeRotationToEuler')
        return {'FINISHED'}
class GeometryNodeInputIndex(Operator):
    """ GeometryNodeGeometryNodeInputIndex """
    bl_idname = "geometrynode.inputindex"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeInputIndex')
        return {'FINISHED'}
class FunctionNodeRotateEuler(Operator):
    """ GeometryNodeFunctionNodeRotateEuler """
    bl_idname = "geometrynode.functionnoderotateeuler"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('FunctionNodeRotateEuler')
        return {'FINISHED'}
class ShaderNodeTexGradient(Operator):
    """ GeometryNodeShaderNodeTexGradient """
    bl_idname = "geometrynode.shadernodetexgradient"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeTexGradient')
        return {'FINISHED'}
class GeometryNodeInputPosition(Operator):
    """ GeometryNodeGeometryNodeInputPosition """
    bl_idname = "geometrynode.inputposition"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeInputPosition')
        return {'FINISHED'}
class ShaderNodeVectorRotate(Operator):
    """ GeometryNodeShaderNodeVectorRotate """
    bl_idname = "geometrynode.shadernodevectorrotate"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('ShaderNodeVectorRotate')
        return {'FINISHED'}
class GeometryNodeScaleInstances(Operator):
    """ GeometryNodeGeometryNodeScaleInstances """
    bl_idname = "geometrynode.scaleinstances"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('GeometryNodeScaleInstances')
        return {'FINISHED'}
#1
class _align(Panel):
    """Shader"""
    bl_label = "几何"
    bl_space_type = "NODE_EDITOR"
    bl_region_type = "UI"
    bl_category = "几何"

    def draw(self, context):
        row = self.layout.row(align=True)
        row.operator(GeometryNodeAlignX.bl_idname, text="X")
        row.operator(GeometryNodeAlignNX.bl_idname, text="-X")
        row.operator(GeometryNodeAlignY.bl_idname, text="Y")
        row.operator(GeometryNodeAlignNY.bl_idname, text="-Y")

#4
        row = self.layout.row(align=True)
        row.operator(GeometryNodeFunctionNodeAlignEulerToVector.bl_idname, text="AlignEulerToVector")
        row.operator(GeometryNodeFunctionNodeAlignRotationToVector.bl_idname, text="AlignRotationToVector")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeCaptureAttribute.bl_idname, text="CaptureAttribute")
        row.operator(GeometryNodeCollectionInfo.bl_idname, text="CollectionInfo")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeShaderNodeValToRGB.bl_idname, text="ColorRamp")
        row.operator(GeometryNodeShaderNodeCombineXYZ.bl_idname, text="CombineXYZ")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeCurvePrimitiveCircle.bl_idname, text="CurveCircle")
        row.operator(GeometryNodeCurvePrimitiveLine.bl_idname, text="CurveLine")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeInputTangent.bl_idname, text="CurveTangent")
        row.operator(GeometryNodeCurveToMesh.bl_idname, text="CurveToMesh")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeCurveToPoints.bl_idname, text="CurveToPoints")
        row.operator(GeometryNodeDeleteGeometry.bl_idname, text="DeleteGeometry")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeDistributePointsOnFaces.bl_idname, text="DistributePointsOnFaces")
        row.operator(GeometryNodeCurveEndpointSelection.bl_idname, text="EndpointSelection")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeProximity.bl_idname, text="Geometry Proximity")
        row.operator(ShaderNodeTexGradient.bl_idname, text="Gradient")
        row = self.layout.row(align=True)
        row.operator(NodeGroupInput.bl_idname, text="GroupInput")
        row.operator(GeometryNodeInputIndex.bl_idname, text="Index")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeInstanceOnPoints.bl_idname, text="InstanceOnPoints")
        row.operator(GeometryNodeJoinGeometry.bl_idname, text="JoinGeometry")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeMapRange.bl_idname, text="MapRange")
        row.operator(ShaderNodeMath.bl_idname, text="Math")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeMeshIcoSphere.bl_idname, text="MeshIcoSphere")
        row.operator(ShaderNodeMix.bl_idname, text="Mix")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeShaderNodeTexNoise.bl_idname, text="Noise")
        row.operator(GeometryNodeInputNormal.bl_idname, text="Normal")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeObjectInfo.bl_idname, text="ObjectInfo")
        row.operator(GeometryNodeInputPosition.bl_idname, text="Position")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeFunctionNodeRandomValue.bl_idname, text="RandomValue")
        row.operator(GeometryNodeRealizeInstances.bl_idname, text="RealizeInstances")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeResampleCurve.bl_idname, text="ResampleCurve")
        row.operator(FunctionNodeRotateEuler.bl_idname, text="RotateEuler")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeRotateInstances.bl_idname, text="RotateInstances")
        row.operator(FunctionNodeRotationToEuler.bl_idname, text="RotationToEuler")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeSampleIndex.bl_idname, text="SampleIndex")
        row.operator(GeometryNodeSampleNearest.bl_idname, text="SampleNearest")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeScaleInstances.bl_idname, text="ScaleInstances")
        row.operator(GeometryNodeInputSceneTime.bl_idname, text="SceneTime")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeSeparateXYZ.bl_idname, text="SeparateXYZ")
        row.operator(GeometryNodeSetCurveRadius.bl_idname, text="SetCurveRadius")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeSetCurveHandlePositions.bl_idname, text="SetHandlePositions")
        row.operator(GeometryNodeCurveSetHandles.bl_idname, text="SetHandlesType")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeSetMaterial.bl_idname, text="SetMaterial")
        row.operator(GeometryNodeSetPosition.bl_idname, text="SetPosition")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeSetShadeSmooth.bl_idname, text="SetShadeSmooth")
        row.operator(GeometryNodeCurveSplineType.bl_idname, text="SetSplineType")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeSplineParameter.bl_idname, text="SplineParameter")
        row.operator(GeometryNodeTransform.bl_idname, text="Transform")
        row = self.layout.row(align=True)
        row.operator(GeometryNodeTrimCurve.bl_idname, text="TrimCurve")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeValue.bl_idname, text="Value")
        row.operator(GeometryNodeShaderNodeVectorMath.bl_idname, text="VectorMath")
        row = self.layout.row(align=True)
        row.operator(ShaderNodeVectorRotate.bl_idname, text="VectorRotate")
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
    GeometryNodeAlignNX,
    GeometryNodeShaderNodeTexNoise,
    GeometryNodeShaderNodeVectorMath,
    GeometryNodeRotateInstances,
    GeometryNodeAlignY,
    GeometryNodeAlignNY,
    GeometryNodeMeshIcoSphere,
    GeometryNodeInputSceneTime,
    GeometryNodeShaderNodeCombineXYZ,
    GeometryNodeCaptureAttribute,
    GeometryNodeSetMaterial,
    ShaderNodeMath,
    ShaderNodeSeparateXYZ,
    ShaderNodeMapRange,
    ShaderNodeValue,
    GeometryNodeRealizeInstances,
    GeometryNodeSetShadeSmooth,
    GeometryNodeCollectionInfo,
    GeometryNodeSampleNearest,
    GeometryNodeSampleIndex,
    GeometryNodeInputNormal,
    GeometryNodeProximity,
    ShaderNodeMix,
    GeometryNodeCurveSetHandles,
    GeometryNodeSetCurveHandlePositions,
    GeometryNodeCurveSplineType,
    NodeGroupInput,
    GeometryNodeDeleteGeometry,
    GeometryNodeCurveToPoints,
    GeometryNodeTransform,
    FunctionNodeRotationToEuler,
    GeometryNodeInputIndex,
    FunctionNodeRotateEuler,
    ShaderNodeTexGradient,
    GeometryNodeInputPosition,
    ShaderNodeVectorRotate,
    GeometryNodeScaleInstances#3
]

def register():
    for c in classes:
        bpy.utils.register_class(c)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)

if __name__ == '__main__':
    register()