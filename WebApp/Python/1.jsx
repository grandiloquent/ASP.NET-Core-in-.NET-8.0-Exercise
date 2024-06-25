var myDoc = app.activeDocument;
var myLine = myDoc.pathItems.add();

//set stroked to true so we can see the path
myLine.stroked = true;

var newPoint = myLine.pathPoints.add();
newPoint.anchor = [0, 0];
//giving the direction points the same value as the
//anchor point creates a straight line segment
newPoint.leftDirection = newPoint.anchor;
newPoint.rightDirection = newPoint.anchor;
newPoint.pointType = PointType.CORNER;

var newPoint1 = myLine.pathPoints.add();
newPoint1.anchor = [100, -100];
newPoint1.leftDirection = newPoint1.anchor;
newPoint1.rightDirection = newPoint1.anchor;
newPoint1.pointType = PointType.CORNER;

var newPoint2 = myLine.pathPoints.add();
newPoint2.anchor = [200, -100];
//giving the direction points different values
//than the anchor point creates a curve
newPoint2.leftDirection =[200, -100];
newPoint2.rightDirection = [200, -100];
newPoint2.pointType = PointType.CORNER;