"""
计算点到光标的距离
1.将点与光标直连的距离
2.用线性代数的"点积"计算投射到XY平面的距离
"""
import bpy
import bmesh
import mathutils
activeObject=bpy.context.object;
bm = bmesh.from_edit_mesh(activeObject.data)
selectedVert=[e for e in bm.verts if e.select][0]
worldCoordinate=activeObject.matrix_world@selectedVert.co

location = bpy.context.scene.cursor.location
bm = bmesh.new()
vert1=bm.verts.new(location)
vert2=bm.verts.new(worldCoordinate)
edge=bm.edges.new([vert1,vert2])
plane_no = mathutils.Vector((0, 0, 1))  # 投射到 XY 平面
plane_co = location #平面上的点
def proj(v):
        return v - (v - plane_co).dot(plane_no) * plane_no
v1, v2 = [proj(v.co) for v in edge.verts]   
name = str((v1 - v2).length)#str(edge.calc_length())
me = bpy.data.meshes.new(name='Vert')
ob = bpy.data.objects.new(name=name, object_data=me)
bm.to_mesh(ob.data)
bpy.context.collection.objects.link(ob)