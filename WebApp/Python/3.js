(() => {
    const numbers = `862.5,-252.499999999999,862.5,-252.499999999999,862.5,-252.499999999999
    862.5,-349.117647058823,841.5,-294.735294117647,883.5,-403.5
    898.5,-442.5,898.5,-401.5,898.5,-489.510637094172
    853.5,-529.5,871.971703851545,-518.345576804657,827.73009474356,-545.061554648364
    739.5,-552.5,794.5,-556.5,739.5,-552.5
    739.5,-581.058823529413,739.5,-581.058823529413,739.5,-581.058823529413
    818.025150159319,-577.058823529413,750.555390744528,-588.647323299301,879.5,-566.5
    930.5,-463.5,919.5,-519.5,943.206579759089,-398.811957590093
    891.5,-334.499999999999,905.013571387621,-365.06155081719,882.596970580946,-314.36539766485
    892.235294117647,-252.499999999999,884.970588235294,-285.499999999999,892.235294117647,-252.499999999999`;
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
    bez_points[i].handle_right = control_points[i][2]`);
})();