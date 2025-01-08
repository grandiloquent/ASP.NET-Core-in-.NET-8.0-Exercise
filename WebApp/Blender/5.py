import bpy
import mathutils

nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
node =  [n for n in nodes if n.select][0]

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

alignNode(node);

def guessGeometryNodes():
    1

bpy.ops.node.add_node(use_transform=True, type="GeometryNodeInstanceOnPoints")
bpy.ops.node.add_node(use_transform=True, type="GeometryNodeSubdivisionSurface")
bpy.ops.node.add_node(use_transform=True, type="GeometryNodeTransform")
bpy.ops.node.add_node(use_transform=True, type="GeometryNodeSetPosition")

[n for n in [m for m in bpy.context.active_object.modifiers if m.type=='NODES'][0].node_group.nodes if n.select][0].type

nodeGroup = [m for m in bpy.context.active_object.modifiers if m.type=='NODES'][0].node_group
node = [n for n in nodeGroup.nodes if n.select][0]

if node.type == 'GROUP_INPUT':
    names =['GeometryNodeInstanceOnPoints']
    for i in names:
            n=nodeGroup.nodes.new(i)
            nodeGroup.links.new(node.outputs[0],n.inputs[0])

if node.type == 'GROUP_OUTPUT':
    names =['GeometryNodeSubdivisionSurface']
    for i in names:
            n=nodeGroup.nodes.new(i)
            nodeGroup.links.new(n.outputs[0],node.inputs[0])    

if node.type == 'SUBDIVISION_SURFACE':
    names =['GeometryNodeTransform',"GeometryNodeSetPosition"]
    for i in names:
            n=nodeGroup.nodes.new(i)
            nodeGroup.links.new(node.outputs[0],n.inputs[0])   
   

