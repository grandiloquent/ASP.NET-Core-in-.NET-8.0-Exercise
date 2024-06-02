(() => {
    const numbers = `588.142857142857,352.285714285714,588.142857142857,352.285714285714,588.142857142857,352.285714285714
    649.285714285714,357.428571428572,635.571428571428,355.714285714288,663,359.142857142857
    704.142857142859,373.142857142859,687.285714285716,364.857142857143,721.000000000002,381.428571428574
    731.571428571431,401.285714285716,727.857142857145,391.571428571431,735.285714285717,411.000000000002
    725.857142857145,426.285714285716,732.142857142861,418.57142857143,719.571428571429,434.000000000003
    691.857142857145,428.857142857145,700.714285714288,436.000000000002,683.000000000002,421.714285714288
    666.714285714287,401.14285714286,671.857142857144,408.285714285717,661.57142857143,394.000000000002
    644.714285714287,376.285714285717,653.285714285716,383.428571428576,636.142857142859,369.142857142859
    622.428571428573,363.428571428574,638.14285714286,370.000000000004,606.714285714287,356.857142857145
    588.142857142857,356.571428571431,594.142857142855,356.571428571431,582.142857142859,356.571428571431
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