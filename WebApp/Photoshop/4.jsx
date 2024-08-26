var pointSamples = app.activeDocument.colorSamplers
var buffer=[];

var x=parseFloat(pointSamples[0].position[0]|0)+","+parseFloat(pointSamples[1].position[0]|0);
var y=parseFloat(pointSamples[2].position[1]|0)+","+parseFloat(pointSamples[3].position[1]|0);

var s="click(accessibilityService, getRandomNumber("+x+"), getRandomNumber("+y+"));";
$.writeln(s);