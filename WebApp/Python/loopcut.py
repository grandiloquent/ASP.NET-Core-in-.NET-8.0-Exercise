# import bpy
# import bmesh

# cuts = 2

# context = bpy.context
# ob = context.object
# me = ob.data

# bm = bmesh.from_edit_mesh(me)
# edge = bm.select_history.active
# def edge_loops(edge):
#     def walk(edge):
#         yield edge
#         edge.tag = True
#         for l in edge.link_loops:
#             loop = l.link_loop_radial_next.link_loop_next.link_loop_next
#             if not (len(loop.face.verts) != 4 or loop.edge.tag):
#                 yield from walk(loop.edge)
#     for e in bm.edges:
#         e.tag = False
#     return list(walk(edge))
        
# if isinstance(edge, bmesh.types.BMEdge): 
#     ''' 
#     bmesh.ops.subdivide_edges(
#         bm,
#         edges=edge_loops(edge),
#         cuts=cuts,
#         smooth_falloff='INVERSE_SQUARE',
#         use_grid_fill=True,
#         )
#     '''
#     bmesh.ops.subdivide_edgering(
#         bm,
#         edges=edge_loops(edge),
#         cuts=cuts,
#         profile_shape='INVERSE_SQUARE',
#         profile_shape_factor=0.8,
#         )    
#     bmesh.update_edit_mesh(me)

import bpy
def view3d_find( return_area = False ):

    for area in bpy.context.window.screen.areas:
        if area.type == 'VIEW_3D':
            v3d = area.spaces[0]
            
            for region in area.regions:
                if region.type == 'WINDOW':
                    if return_area: 
                        return region, v3d, area
region, v3d, area = view3d_find(True)
#we need to put those values into a single variable in the following
override = {
    'scene': bpy.context.scene,'region' : region,'area': area,'space': v3d}
#must be in edit mode
bpy.ops.transform.edge_slide(override,value=0.3,  mirror=True, correct_uv=True)