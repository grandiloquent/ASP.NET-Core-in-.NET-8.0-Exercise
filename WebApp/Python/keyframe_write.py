import bpy
import json

path='C:/Users/Administrator/Desktop/1.json'
a_index = bpy.context.object.animation_data.action.name
f_index = 0
with open(path, 'r') as file:
    bpy.data.actions[a_index].fcurves[f_index].keyframe_points.clear()
    anim_data = json.load(file)
    i = 0;
    for a in anim_data:
        bpy.data.actions[a_index].fcurves[f_index].keyframe_points.add(1)
        k=bpy.data.actions[a_index].fcurves[f_index].keyframe_points[i]
        k.co=(a['x'],a['y'])
        k.handle_left=(a['xl'],a['yl'])
        k.handle_right=(a['xr'],a['yr'])
        k.interpolation=a['interpolation']
        i=i+1
