import bpy
import math
from mathutils import Vector


parent = bpy.context.object

corners = [ Vector(corner) for corner in  parent.bound_box] 
offset = round(max([v.y for v in corners]),4)
#bpy.ops.transform.translate(value=(max([v.x for v in corners])*-2,0,0))
for i in range(1,11):  
  o = parent.copy()
  if i == 1:
    o.location.y =  o.location.y+parent.dimensions.y
  elif i==2:
    o.location.y =  o.location.y-parent.dimensions.y
  elif i==3:
    o.location.x =  o.location.x+parent.dimensions.x
  elif i==4:
    o.location.x =  o.location.x-parent.dimensions.x
  elif i==5:
    o.location.z =  o.location.z+parent.dimensions.z
  elif i==6:
    o.location.z =  o.location.z-parent.dimensions.z
  elif i==7:
    o.location.z =  o.location.z+parent.dimensions.z
    o.location.x =  o.location.x+parent.dimensions.x*math.sin(math.radians(45))
    o.location.y =  o.location.y+parent.dimensions.y*math.cos(math.radians(45))
  elif i==8:
    o.location.z =  o.location.z+parent.dimensions.z
    o.location.x =  o.location.x-parent.dimensions.x*math.sin(math.radians(45))
    o.location.y =  o.location.y-parent.dimensions.y*math.cos(math.radians(45))
  elif i==9:
    o.location.z =  o.location.z+parent.dimensions.z
    o.location.x =  o.location.x-parent.dimensions.x*math.sin(math.radians(45))
    o.location.y =  o.location.y+parent.dimensions.y*math.cos(math.radians(45))
  elif i==10:
    o.location.z =  o.location.z+parent.dimensions.z
    o.location.x =  o.location.x+parent.dimensions.x*math.sin(math.radians(45))
    o.location.y =  o.location.y-parent.dimensions.y*math.cos(math.radians(45))
  bpy.context.collection.objects.link(o)
  #parent.location.y+=parent.dimensions.y*i;
#bpy.ops.transform.translate(value=(0,0,max([v.z for v in corners])*2))