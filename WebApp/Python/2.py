#设置源点
#应用缩放、旋转
from random import uniform
import bpy
for i in range (0,12):
  zo=bpy.context.object.dimensions.z
  xo=bpy.context.object.dimensions.x
  bpy.ops.object.duplicate_move()
  scale = uniform(0.9,1.1)
  bpy.context.object.scale = (scale,scale,scale)
  bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
  yn = bpy.context.object.dimensions.y
  bpy.context.object.location.y =bpy.context.object.location.y+yn
  bpy.context.object.location.z =bpy.context.object.location.z-(zo-bpy.context.object.dimensions.z)/2
  bpy.context.object.location.x =bpy.context.object.location.x+(xo-bpy.context.object.dimensions.x)/2