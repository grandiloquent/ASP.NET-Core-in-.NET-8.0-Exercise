(() => {
    const polylinePoints = `858.4,521.7 884.1,521.7 908.1,519.4 919.6,519.4 923.6,513.1 923.6,501.7 914.4,501.7 920.7,493.1 
	923.6,484 919.6,473.7 908.1,464.6 903,456.6 904.1,449.1 908.1,444 903.6,438.9 898.4,438.9 885.9,419.4 879,394.9 878.4,372.6 
	896.7,372.6 904.7,363.4 900.1,355.4 891,353.1 891,347.4 880.7,342.3 881.9,334.4 887.6,334.4 885.3,326.3 896.7,316.6 
	900.1,298.9 896.7,278.9 884.1,262.3 865.9,238.9 874.4,235.4 874.4,226.3 861.3,220 854.4,220`;
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