(() => {
    const polylinePoints = `702.9,327.1 689.6,327.1 672.9,324.6 653.6,321.1 630.9,316.4 606.9,313.9 577.3,311.7 557.6,311.7 
	537,311.7 511.7,314.7 483,317.3 450,322.4 450,357.6 459,372.1 467.1,399.1 477,427 486.9,443.7 503.6,453.1 525.4,457.9 
	555.4,461.3 587.6,461.3 604.3,461.3 622.3,457.4 641.6,450.1 652.3,441.6 659.6,431.7 667.7,414.6 675,395.7 682.8,375.6 
	686.1,364.9 668.6,364.9 667.3,378.6 664.3,391 659.6,404.3 653.6,417.1 646.3,430.4 637.3,439.4 626.1,445.4 615,448.4 597,451.9 
	579,453.6 561.9,452.7 544.3,450.6 518.6,447.6 501.9,441.1 493.7,433.4 484.7,417.6 480,386.5 481.3,359.3 486.9,344.7 498.9,334 
	523.7,328.4 552.9,324.6 570,324.6 599.1,325.9 623.1,328.9 650.6,336.6 661.7,345.1 669,355 702.9,355 `;
    const points = [...polylinePoints.matchAll(/[\d.]+/g)]
        .map(point => parseFloat(point[0]));
    const start = [points[0], points[1]];
    const end = [];
    const endPoints = points;
    for (let i = 0; i < endPoints.length; i += 2) {
        if (i < endPoints.length-2 ) {
            end.push([
                [
                    parseFloat(((endPoints[i] - start[0]) / 100).toFixed(2)),
                    0,
                   parseFloat(((endPoints[i + 1] - start[1]) / 100).toFixed(2))

                ],
                [parseFloat(((endPoints[i + 2] - start[0]) / 100).toFixed(2))
                    ,
                    0,
                parseFloat(((endPoints[i + 3] - start[1]) / 100).toFixed(2))


                ]
            ]);
        }
    }
    console.log(`import bpy
import bmesh
data = bpy.context.object.data
bm = bmesh.from_edit_mesh(data)
points= ${JSON.stringify(end)}
for p in points:
    v1 = bm.verts.new(p[0])
    v2 = bm.verts.new(p[1])
    bm.edges.new((v1,v2))
bmesh.update_edit_mesh(data)`);

})();