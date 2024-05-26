(() => {
    const polylinePoints = `776.7,528 734.1,528 681.9,531.1 626.1,520.9 602.7,485.4 599.6,429.1 613.6,376.6 619.9,338.3 
	610.7,302.9 595.6,257.7 590.7,238.3 `;
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