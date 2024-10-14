import bpy

def align(node,yx):
    inputlist = [i for i in node.inputs if i.is_linked][0:1]
    offset=60
    y=yx
    for input in inputlist:
        for link in input.links:
            link.from_node.location.x=node.location.x-link.from_node.dimensions.x-offset
            link.from_node.location.y=y
            y=y-offset-link.from_node.dimensions.y
            align(link.from_node,yx)

nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
nodes = [n for n in nodes if n.select]
node = nodes[0]
y=node.location.y
align(node,y)


