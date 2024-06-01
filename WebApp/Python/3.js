(() => {
    const numbers = `904,-445,904,-371.546128276601,904,-518.453871723401
    771,-578,844.453871723399,-578,697.546128276601,-578
    638,-445,638,-518.453871723401,638,-371.5461282766
    505,-312,578.4538717234,-312,431.5461282766,-312
    372,-445,372,-371.5461282766,372,-518.453871723399
    `;
    const matches = [...numbers.matchAll(/[\d-.]+/g)].map(x => parseFloat(x[0]));
    const buffer = [];
    for (let i = 0; i < matches.length; i += 6) {
        buffer.push([
            [matches[i], matches[i + 1], 0],
            [matches[i + 2], matches[i + 3], 0],
            [matches[i + 4], matches[i + 5], 0],
        ]);
    }
    console.log(`import bpy

bpy.ops.curve.primitive_bezier_curve_add()
curve = bpy.context.active_object
bez_points = curve.data.splines[0].bezier_points
    
control_points = ${JSON.stringify(buffer)}
# note: a created bezier curve has already 2 control points
bez_points.add(len(control_points) - 2)
    
# now copy the csv data
for i in range(len(control_points)):        
    bez_points[i].co = control_points[i][0]
    bez_points[i].handle_left_type  = 'FREE'
    bez_points[i].handle_right_type = 'FREE'
          
    bez_points[i].handle_left  = control_points[i][1]
    bez_points[i].handle_right = control_points[i][2]
    
bpy.context.object.scale[1] = 0.01
bpy.context.object.scale[2] = 0.01
bpy.context.object.scale[0] = 0.01
bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
bpy.ops.transform.rotate(value=-1.5708, orient_axis='X')
bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
bpy.context.object.location[0] = 0
bpy.context.object.location[1] = 0
bpy.context.object.location[2] = 0`);
})();