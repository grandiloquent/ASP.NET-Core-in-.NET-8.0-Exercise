function saveAsTextFile(filePath, content) {
    var saveFile = new File(filePath);

    saveFile.encoding = "UTF8";
    saveFile.open("w");
    if (saveFile.error != "")
        return saveFile.error;

    saveFile.write(content);
    if (saveFile.error != "")
        return saveFile.error;

    saveFile.close();
    if (saveFile.error != "")
        return saveFile.error;

    return "";
}
var pointSamples = app.activeDocument.colorSamplers
var buffer = [];
var buffer1 = [];
for (var i = 0; i < Math.min(pointSamples.length, 5); i++) {
    var rgb = pointSamples[i].color.rgb;
    //buffer.push(parseFloat(pointSamples[i].position[0]|0)+","+parseFloat(pointSamples[i].position[1]|0)+","+((0xFF << 24) | (rgb.red << 16) | (rgb.green << 8) | rgb.blue)+",");
    buffer.push("checkIfColorIsRange(bitmap, " + parseFloat(pointSamples[i].position[0] | 0) + ", " +
        parseFloat(pointSamples[i].position[1] | 0)
        + ", (r) -> {\n" +
        "                        return (r > " + (rgb.red | 0) + " - 20)&& (r<" + (rgb.red | 0) + "+20);\n" +
        "                    }, (g) -> {\n" +
        "                        return (g > " + (rgb.green | 0) + "-20)&&(g<" + (rgb.green | 0) + "+20);\n" +
        "                    }, (b) -> {\n" +
        "                        return (b > " + (rgb.blue | 0) + "-20)&&(b<" + (rgb.blue | 0) + "+20);\n" +
        "                    }) &&");
    buffer1.push(parseFloat(pointSamples[i].position[0] | 0) + ","
        + parseFloat(pointSamples[i].position[1] | 0) + ","
        + (rgb.red | 0) + ","
        + (rgb.green | 0) + ","
        + (rgb.blue | 0));
}
var s = "if (compareColor(20, decoded,\n" +
    buffer.join('\n') + ")"
    + "&& Utils.checkColorTotal(bitmap, +" + parseFloat(pointSamples[4].position[0] | 0) + "+," + parseFloat(pointSamples[4].position[1] | 1) + " 84, 750)"
"                    ) {\n" +
    "                        Log.e(\"B5aOx2\", String.format(\"screenShoot2, %s\", \"3\"));\n" +
    "                        click(accessibilityService, getRandomNumber(300, 780), getRandomNumber(1320, 1380));\n" +
    "\nThread.sleep(1000);\n" +
    "                    }"
s = "// \nelse if (" + buffer.join('\n') + ")\n{\n}";
s = "Utils.checkIfColorIsRange(20,bitmap,new int[]{" + buffer1.join(',\n') + "})\n"
s += "&& Utils.checkColorTotal(bitmap, " + (parseFloat(pointSamples[4].position[0] | 0)) + "," + (parseFloat(pointSamples[4].position[1] | 1)) + ", 750)\n"

//s="//\nelse if (Utils.checkIfColorIsRange(20,bitmap,new int[]{"+buffer1.join(',\n')+"})){\n Toast.makeText(mAccessibilityService, \"\", Toast.LENGTH_SHORT).show();\n}"
//$.writeln(s);

if (pointSamples.length > 8) {


    var x3 = parseFloat(pointSamples[5].position[0] | 0) + "," + parseFloat(pointSamples[6].position[0] | 0);
    var y3 = parseFloat(pointSamples[7].position[1] | 0) + "," + parseFloat(pointSamples[8].position[1] | 0);

    var s3 = "click(accessibilityService, getRandomNumber(" + x3 + "), getRandomNumber(" + y3 + "));";
    //s=x+"\n"+y;
    //$.writeln(s3);
}

var x1 = buffer1.join(',\n');
var x2 = pointSamples.length > 8 && parseFloat(pointSamples[5].position[0] | 0);
var x3 = pointSamples.length > 8 && parseFloat(pointSamples[6].position[0] | 0);
var x4 = pointSamples.length > 8 && parseFloat(pointSamples[7].position[1] | 0);
var x5 = pointSamples.length > 8 && parseFloat(pointSamples[8].position[1] | 0);
var str = "if (Utils.checkIfColorIsRange(20,bitmap,new int[]{" + x1 + "})" +
    "        ) {" +
    "            Toast.makeText(accessibilityService, \"\", Toast.LENGTH_SHORT).show();" +
    "            click(accessibilityService, getRandomNumber(" + x2 + "," + x3 + "), getRandomNumber(" + x4 + "," + x5 + "));" +
    "            return true;" +
    "        }";

$.writeln(str);

function saveAsTextFile(filePath, content) {
    var saveFile = new File(filePath);

    saveFile.encoding = "UTF8";
    saveFile.open("w");
    if (saveFile.error != "")
        return saveFile.error;

    saveFile.write(content);
    if (saveFile.error != "")
        return saveFile.error;

    saveFile.close();
    if (saveFile.error != "")
        return saveFile.error;

    return "";
}

var Path = app.activeDocument.path;
var saveFile = File(Path + "/3.txt");
saveAsTextFile(saveFile,str);