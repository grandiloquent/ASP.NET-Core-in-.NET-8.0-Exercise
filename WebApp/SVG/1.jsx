// Create a new gradient
// A new gradient always has 2 stops
var newGradient = app.activeDocument.gradients.add();
// newGradient.name = "NewGradient";
newGradient.type = GradientType.LINEAR;

var data = [
    [
        0, 10, 10, 10
    ],
    [
        10, 30, 30, 30
    ],
    [
        50, 0, 0, 0
    ],
    [
        90, 30, 30, 30
    ],
    [
        100, 10, 10, 10
    ]
]
for (var i = 0; i < data.length; i++) {
    if (i > 1) 
        newGradient.gradientStops.add();
    
    var newRGBColor = new RGBColor();
    newRGBColor.red = data[i][1];
    newRGBColor.green = data[i][2];
    newRGBColor.blue = data[i][3];

    newGradient.gradientStops[i].rampPoint = data[i][0];
    newGradient.gradientStops[i].color = newRGBColor;
}

// construct an Illustrator.GradientColor object referring to the newly created gradient
var colorOfGradient = new GradientColor();

colorOfGradient.angle = 90
colorOfGradient.gradient = newGradient;

// get first path item, apply new gradient as its fill
var topPath = app.activeDocument.selection[0];
topPath.filled = true;
topPath.fillColor = colorOfGradient;
