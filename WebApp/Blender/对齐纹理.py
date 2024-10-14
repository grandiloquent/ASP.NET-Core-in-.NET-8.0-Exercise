import bpy

def align(node,yx):
    outputs=node.outputs
    offset=60
    y=yx
    for output in outputs:
        for link in output.links:
            link.to_node.location.x=node.location.x+node.dimensions.x+offset
            link.to_node.location.y=y
            y=y-offset-link.to_node.dimensions.y
            align(link.to_node,yx)

nodes = bpy.context.view_layer.objects.active.active_material.node_tree.nodes
nodes = [n for n in nodes if n.select]
node = nodes[0]
y=node.location.y
align(node,y)


