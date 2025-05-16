import bpy
nms = [m for m in bpy.context.active_object.modifiers if m.type=='NODES']
n=nms[0].node_group 
nn=n.interface.new_socket(name="Resolution Y", socket_type="NodeSocketFloat", in_out="INPUT")
nn.default_value=1
nn.min_value=0