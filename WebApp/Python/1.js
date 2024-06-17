(() => {
    const polylinePoints = `664.7,378.9 634.4,401.1 595,384.1 576.7,328 629.3,273.7 702.4,261.7 769.3,278.3 799.6,326.3 
	807.6,381.1 779,420.6 736.7,438.9 701.3,415.4 694.4,384 `;
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