var pathItem = app.activeDocument.pathItems[0];
var length = pathItem.pathPoints.length;
for (var i = 0; i < length; i++) {
    var pathPoint = pathItem.pathPoints[i];
    if (pathPoint.selected == PathPointSelection.ANCHORPOINT) {
        $.writeln(pathPoint.anchor[0] - pathPoint.leftDirection[0]);
        $.writeln(pathPoint.anchor + "," + pathPoint.leftDirection + "," + pathPoint.rightDirection);
        pathPoint.leftDirection = [
            pathPoint.anchor[0], pathPoint.anchor[1]-92.2315532165994
        ]
        // - 107.4193838361 -92.2315532165994

    }
}
