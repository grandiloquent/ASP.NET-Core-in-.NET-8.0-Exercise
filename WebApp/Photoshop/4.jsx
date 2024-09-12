var pointSamples = app.activeDocument.colorSamplers
var buffer=[];

var x="convertX("+parseFloat(pointSamples[0].position[0]|0)+"),convertX("+parseFloat(pointSamples[1].position[0]|0+")");
var y="convertY("+parseFloat(pointSamples[2].position[1]|0)+"),convertY("+parseFloat(pointSamples[3].position[1]|0+")");

var s="click(accessibilityService, getRandomNumber("+x+")), getRandomNumber("+y+")));";
//s=x+"\n"+y;
$.writeln(s);