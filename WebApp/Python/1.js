(() => {
    const polylinePoints = `545.3,275.7 565.9,225.7 589.9,210 638.1,207.7 720.7,207.7 719.6,269.7 735.6,294 760.1,302.3 
	783,290 807.9,292 807.9,310.9 795.3,336 769,367.1 807.9,434.9 788.4,504.3 738.1,554.9 669,573.4 598.7,554.6 548.7,504.6 
	529.3,434.6 575,360.9 579.9,316`;
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