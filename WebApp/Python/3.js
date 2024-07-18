(() => {
    const numbers = `656.714285714285,442.285714285715,645.642857142857,417.07142857143,673.517379827877,480.553406041182
    725.785714285714,444.928571428572,712.866543341977,481.174023242945,733.500000000002,423.285714285707
    786.214285714286,401.428571428565,775.928571428572,434.64285714285,794.120457407473,375.898225335981
    789.857142857138,341.857142857139,773.142857142859,367.142857142855,818.272136178721,298.870358088572
    741.304327261874,277.211351865284,785.700018497564,260.490896724548,708.30432726188,289.639923293869
    642.428571428566,281.857142857136,671.142857142852,250.571428571422,613.71428571428,313.14285714285
    630.428571428566,355.57142857142,663.642857142857,315.928571428578,614.839360695335,374.17790589817
    616.285714285714,409.142857142859,602.142857142857,391.57142857143,630.428571428571,426.714285714287
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