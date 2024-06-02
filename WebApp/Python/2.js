(() => {
    const polylinePoints = `672.4,478 705.4,461.7 735.9,435.1 758.6,391.4 761.1,359.3 750.4,333.6 750.9,312.6 753.4,299.3 
	736.7,291.1 705,282.1 672.4,279.6`;
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