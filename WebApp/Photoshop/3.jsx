var pointSamples = app.activeDocument.colorSamplers
var buffer=[];
var buffer1=[];
for(var i = 0; i < pointSamples.length; i++) {
    var rgb=pointSamples[i].color.rgb;
//buffer.push(parseFloat(pointSamples[i].position[0]|0)+","+parseFloat(pointSamples[i].position[1]|0)+","+((0xFF << 24) | (rgb.red << 16) | (rgb.green << 8) | rgb.blue)+",");
buffer.push("checkIfColorIsRange(bitmap, "+parseFloat(pointSamples[i].position[0]|0)+", "+
parseFloat(pointSamples[i].position[1]|0)
+", (r) -> {\n" +
                            "                        return (r > "+(rgb.red|0)+" - 20)&& (r<"+(rgb.red|0)+"+20);\n" +
                            "                    }, (g) -> {\n" +
                            "                        return (g > "+(rgb.green|0)+"-20)&&(g<"+(rgb.green|0)+"+20);\n" +
                            "                    }, (b) -> {\n" +
                            "                        return (b > "+(rgb.blue|0)+"-20)&&(b<"+(rgb.blue|0)+"+20);\n" +
                            "                    }) &&");
                            buffer1.push(parseFloat(pointSamples[i].position[0]|0)+","
                        +parseFloat(pointSamples[i].position[1]|0)+","
                        +(rgb.red|0)+","
                    +(rgb.green|0)+","
                +(rgb.blue|0));
}
var s="if (compareColor(20, decoded,\n" +
                            buffer.join('\n')+
                            "                    )) {\n" +
                            "                        Log.e(\"B5aOx2\", String.format(\"screenShoot2, %s\", \"3\"));\n" +
                            "                        click(accessibilityService, getRandomNumber(300, 780), getRandomNumber(1320, 1380));\n" +
                            "\nThread.sleep(1000);\n"+
                            "                    }" 
                            s="// \nelse if ("+buffer.join('\n')+")\n{\n}";
                            s="//\nelse if (Utils.checkIfColorIsRange(20,bitmap,new int[]{"+buffer1.join(',\n')+"})){\n// swipe(mAccessibilityService, getRandomNumber(300, 340), getRandomNumber(1380, 1580), getRandomNumber(340, 380), getRandomNumber(380, 680));\n}"
                            $.writeln(s);