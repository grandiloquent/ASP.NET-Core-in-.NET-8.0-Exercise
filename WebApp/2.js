(() => {
    const polylinePoints = `1016.1,521.7 1047,521.7 1069.9,519.4 1083.6,516 1083.6,501.7 1078.4,496 1083.6,488.6 1083.6,477.7 
	1072.7,468.6 1063.6,460.6 1061.9,450.3 1072.7,439.4 1059,432 1083.6,422.9 1049.9,378.9 1019,338.3 1021.3,333.1 1061.9,330.3 
	1060.1,334.9 1069.9,338.9 1078.4,330.3 1072.1,326.3 1080.1,326.3 1083.6,323.4 1086.4,307.4 1070.4,297.7 1056.1,281.7 
	1044.1,252.6 1032.1,252.6 1016.1,252.6 1019,229.1 1002.4,237.7 983,256.6 958.4,283.4 946.4,316.6 946.4,351.4 946.4,384 
	956.7,424 971.6,431.4`;
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