bl_info = {
    "name" : "Quick",
    "description" : "一个快速插件",
    "author" : "",
    "version" : (0, 0, 1),
    "blender" : (2, 80, 0),
    "location" : "View3D",
    "warning" : "",
    "support" : "COMMUNITY",
    "doc_url" : "",
    "category" : "3D View"
}

import bpy
from bpy.types import Operator
from bpy.types import Panel

class _quick_resize(Operator):
    """ Quick resize operator"""
    bl_idname = "quick.resize"
    bl_label = ""
    bl_options = {"REGISTER", "UNDO"}
    x : bpy.props.FloatProperty(default=0.0)
    y : bpy.props.FloatProperty(default=0.0)
    z : bpy.props.FloatProperty(default=0.0)

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        bpy.ops.transform.resize(value=(self.x, self.y, self.z))
        return {'FINISHED'}
      
class _panel(Panel):
    """快捷操作"""
    bl_label = "快捷"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = "快捷"

    def draw(self, context):
        row = self.layout.row(align=True)
        o = row.operator(_quick_resize.bl_idname, text="0.1")
        o.x =.5
        o = row.operator(_quick_resize.bl_idname, text="0.1")
        o.x =.5 

classes = [
    _quick_resize,
    _panel,
]

def register():
    for c in classes:
        bpy.utils.register_class(c)


def unregister():
    for c in classes:
        bpy.utils.unregister_class(c)


if __name__ == '__main__':
    register()