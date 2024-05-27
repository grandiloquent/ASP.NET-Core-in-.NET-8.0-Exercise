#设置源点
#应用缩放、旋转
from random import uniform
import bpy
for i in range (0,5):
  bpy.ops.object.duplicate_move()
  scale = uniform(0.8,1.1)
  bpy.context.object.scale = (scale,scale,scale)
  rotation=uniform(3.66519,5.23599);
  bpy.context.object.rotation_euler[2] = rotation
  bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
  zn = bpy.context.object.dimensions.z
  bpy.context.object.location.z =bpy.context.object.location.z+zn