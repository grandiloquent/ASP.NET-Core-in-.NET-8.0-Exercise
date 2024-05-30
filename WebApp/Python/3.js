(() => {
    const numbers = `-2462.5,-2442.5,-2462.5,-2442.5,-2462.5,-2442.5
    -2020.5,-2087.5,-2287.5,-2087.5,-1753.5,-2087.5
    -1598.85714285714,-2442.5,-1604.21428571429,-2282.5,-1593.5,-2602.5
    -2030.67857142857,-2885,-1788.85714285714,-2885,-2272.5,-2885
    -2462.5,-3234.71428571428,-2451.5,-2972.92857142857,-2473.5,-3496.5
    -2030.62476761081,-3687.28571428571,-2318.74953522163,-3687.28571428571,-1742.5,-3687.28571428571
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
    bez_points[i].handle_right = control_points[i][2]`);
})();