(() => {
    const numbers = `854.5,-269.5,854.5,-93.4592360012502,854.5,-410.332611198999
    599.5,-524.5,740.332611198999,-524.5,486.8339110408,-524.5
    395.5,-320.5,395.5,-433.1660889592,395.5,-230.36712883264
    558.7,-157.3,468.56712883264,-157.3,630.806296933888,-157.3
    689.26,-287.86,689.26,-215.753703066111,689.26,-345.54503754711
    584.812,-392.308000000001,642.49703754711,-392.308000000001,538.663969962312,-392.308000000001
    501.2536,-308.7496,501.2536,-354.897630037688,501.2536,-271.831175969849
    568.10032,-241.90288,531.181895969849,-241.90288,597.635059224121,-241.90288
    621.577696,-295.380256,621.577696,-265.845516775879,621.577696,-319.008047379297
    854.5,-269.5,854.5,-93.4592360012502,854.5,-410.332611198999
    599.5,-524.5,740.332611198999,-524.5,486.8339110408,-524.5
    395.5,-320.5,395.5,-433.1660889592,395.5,-230.36712883264
    558.7,-157.3,468.56712883264,-157.3,630.806296933888,-157.3
    689.26,-287.86,689.26,-215.753703066111,689.26,-345.54503754711
    584.812,-392.308000000001,642.49703754711,-392.308000000001,538.663969962312,-392.308000000001
    501.2536,-308.7496,501.2536,-354.897630037688,501.2536,-271.831175969849
    568.10032,-241.90288,531.181895969849,-241.90288,597.635059224121,-241.90288
    621.577696,-295.380256,621.577696,-265.845516775879,621.577696,-319.008047379297
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