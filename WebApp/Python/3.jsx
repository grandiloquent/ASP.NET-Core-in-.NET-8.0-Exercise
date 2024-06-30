// var pathItem = app.activeDocument.pathItems[0];
// var length = pathItem.pathPoints.length;
// for (var i = 0; i < length; i++) {
//     var pathPoint = pathItem.pathPoints[i];
//     if (pathPoint.selected == PathPointSelection.ANCHORPOINT) {
//         var left = pathPoint.anchor[0] - pathPoint.leftDirection[0];
//         var right = pathPoint.rightDirection[0] - pathPoint.anchor[0];
//         $.writeln(left);
//         $.writeln(right);
//         //pathPoint.rightDirection=[pathPoint.rightDirection[0]+right,pathPoint.rightDirection[1]];
//         $.writeln(pathPoint.anchor+","+pathPoint.leftDirection+","+pathPoint.rightDirection);
//     }
//     // $.writeln(pathPoint.anchor+","+pathPoint.leftDirection+","+pathPoint.rightDirection);
// }
// // 27.6142374900001/10


var pathItem = app.activeDocument.pathItems[1];
var length = pathItem.pathPoints.length;
for (var i = 0; i < length; i++) {
    var pathPoint = pathItem.pathPoints[i];
    $.writeln(pathPoint.anchor + "," + pathPoint.leftDirection + "," + pathPoint.rightDirection);
}
// 27.6142374900001/10
