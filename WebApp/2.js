(() => {
    const polylinePoints = `691,521.7 701.9,521.7 715.6,521.7 729,520.6 743.9,518.6 754.7,516.9 761,514.9 764.4,513.4 
	766.1,510.6 766.1,496.3 757.9,490.9 762.7,485.4 764.4,481.1 766.1,474.3 763.9,464 751.6,450.6 747,444.9 745,438.6 745.9,432.6 
	748.4,426.6 743.3,421.7 737.9,419.4 733.6,410.3 728.6,398.9 723.3,381.4 719.3,363.4 718.1,348.9 718.4,333.7 719.9,322.6 
	721.6,312.9 722.7,306.3 733.9,306.3 739.6,306.3 743.9,301.4 745,296 741.6,289.4 735,286 731.9,284.6 735.3,282.9 736.7,278.6 
	730.7,274.9 723.6,272.9 723.9,266.9 726.7,264.6 727.3,262.3 723.6,259.1 724.4,244 726.7,233.4 732.7,223.1 738.7,215.1 
	745.9,210 741.9,203.1 738.1,198.9 723.3,198.9 718.4,189.7 709.3,181.1 696.7,175.4 701.3,172 704.1,166.9 704.7,159.4 
	702.7,153.1 696.4,148.9 689.6,147.1 `;
    const points = [...polylinePoints.matchAll(/[\d.]+/g)]
        .map(point => parseFloat(point[0]));
    const start = [points[0], points[1]];
    const end = [];
    const endPoints = points;
    for (let i = 0; i < endPoints.length; i += 2) {
        if (i < endPoints.length-2 ) {
            end.push(`(${parseFloat(((endPoints[i] - start[0]) / 100).toFixed(2))},0,${parseFloat(((endPoints[i + 1] - start[1]) / 100).toFixed(2))})`
                 
            );
        }
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