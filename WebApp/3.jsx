var pathItem = app.activeDocument.pathItems[0];
var length = pathItem.pathPoints.length;
for (var i = 0; i < length; i++) {
    var pathPoint = pathItem.pathPoints[i];
    $.writeln(pathPoint.anchor+","+pathPoint.leftDirection+","+pathPoint.rightDirection);
}
