(() => {
    const polylinePoints = `776.7,528 734.1,528 681.9,531.1 626.1,520.9 602.7,485.4 599.6,429.1 613.6,376.6 619.9,338.3 
	610.7,302.9 595.6,257.7 590.7,238.3`;
    const points = [...polylinePoints.matchAll(/[\d.]+/g)]
        .map(point => parseFloat(point[0]));
    const start = [points[0], points[1]];
    const end = [];
    const endPoints = points;
    for (let i = 0; i < endPoints.length; i += 2) {
        
            end.push(`(${parseFloat(((endPoints[i] - start[0]) / 100).toFixed(3))},0,${parseFloat(((endPoints[i + 1] - start[1]) / 100).toFixed(3))})`
                 
            );
    }
    console.log(`import bpy

mesh = bpy.data.meshes.new("myBeautifulMesh")  # add the new mesh
obj = bpy.data.objects.new(mesh.name, mesh)
col = bpy.data.collections["Collection"]
col.objects.link(obj)
bpy.context.view_layer.objects.active = obj
    
verts = [${end.join(", ")}]
edges = []
faces = [[${[...new Array(end.length).keys()].map(x=>x+'').join(", ")}]]
    
mesh.from_pydata(verts, edges, faces)`);

})();