(() => {
    const numbers = `632.678571428571,210.982142857143,630.438776025886,214.580142588831,635.554788555066,206.361797428715
    635,189.428573608398,636.745963645725,202.926961184681,633.210108481775,175.590571029332
    613.571411132812,139.428573608398,620.428588867188,146.571426391602,606.714294433594,132.285720825195
    587.857116699219,125.428573608398,598.714294433594,128,577,122.857139587402
    533.571411132812,122.857139587402,543.285705566406,122.285713195801,523.857116699219,123.428573608398
    488.142852783203,136.285720825195,499,124.285713195801,482.081115722656,142.985549926758
    475,175.142852783203,476.714294433594,152.857147216797,473.285705566406,197.428573608398
    484.345762261951,218.743567585965,473.909874891665,209.772499382911,501.42857142857,233.428571428556
    551.857116699219,231.714279174805,551.857116699219,231.714279174805,551.857116699219,231.714279174805
    613.571411132812,224,597.441135152158,232.343234958696,626.517857142857,217.303571428572
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