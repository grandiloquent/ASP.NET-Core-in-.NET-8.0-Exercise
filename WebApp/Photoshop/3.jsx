var pointSamples = app.activeDocument.colorSamplers
var buffer=[];
for(var i = 0; i < pointSamples.length; i++) {
    var rgb=pointSamples[i].color.rgb;
buffer.push(parseFloat(pointSamples[i].position[0]|0)+","+parseFloat(pointSamples[i].position[1]|0)+","+((0xFF << 24) | (rgb.red << 16) | (rgb.green << 8) | rgb.blue)+",");
}
var s="if (compareColor(20, decoded,\n" +
                            buffer.join('\n')+
                            "                    )) {\n" +
                            "                        Log.e(\"B5aOx2\", String.format(\"screenShoot2, %s\", \"3\"));\n" +
                            "                        click(accessibilityService, getRandomNumber(300, 780), getRandomNumber(1320, 1380));\n" +
                            "\nThread.sleep(1000);\n"+
                            "                    }"
                            $.writeln(s);