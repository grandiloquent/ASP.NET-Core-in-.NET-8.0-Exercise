(() => {
    const polylinePoints = `661.7,229 660.9,250.9 634.7,260.3 620.6,243.4 618.6,219.1 640.2,201.1 664.9,197.5 694.9,209.9 
	703.7,241.9 693.4,278.1 678.9,314.9 664.1,348.1 649.1,385.9 640.9,422.9 640.3,459.6 655.1,490.6 693.6,495.1 720.9,483.6 
	732,452.3 716.1,426.6 682.1,421.9 668.6,449.1 680.1,474.6 709.7,465.8`;
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