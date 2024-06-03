import bpy
import json

anim_data = []
a_index=bpy.context.object.animation_data.action.name;
f_index=1;
for k in bpy.data.actions[a_index].fcurves[f_index].keyframe_points:
    anim_data.append({"x":k.co.x, "y":k.co.y,"interpolation":k.interpolation,"xl":k.handle_left.x,"yl":k.handle_left.y,"xr":k.handle_right.x,"yr":k.handle_right.y})

path='C:/Users/Administrator/Desktop/1.json'
with open(path, 'w') as file:
    json.dump(anim_data, file, indent=4)