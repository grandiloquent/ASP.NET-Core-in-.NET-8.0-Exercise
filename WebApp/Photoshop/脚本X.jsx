var pointSamples = app.activeDocument.colorSamplers

function toInt(value) {
    return value | 0;
}

var rgb = pointSamples[0].color.rgb;
var x = toInt(pointSamples[0].position[0]);
var y = toInt(pointSamples[0].position[1]);

var buffer = [];
for (var i = 1; i < pointSamples.length; i++) {
    var xx=toInt(pointSamples[i].position[0]);
    var yy=toInt(pointSamples[i].position[1]);
    var gg = pointSamples[i].color.rgb;
    buffer.push("i + ("+xx+" - "+x+"),"+yy+", "+toInt(gg.red)+", "+toInt(gg.green)+", "+toInt(gg.blue));
}


var s='if (checkIfColorIsRange(20, bitmap, new int[]{' +
buffer.join(",\n")+
    '})) {' +
    'click(accessibilityService, getRandomNumber(convertX(i),convertX(i) + 100),getRandomNumber(convertY('+y+'), convertY('+y+') + 20));'+
    '                                           return true;' +
    '                                        }'

s='for (int i = '+toInt(pointSamples[pointSamples.length-2].position[0])+'; i < '+toInt(pointSamples[pointSamples.length-1].position[0])+'; i++) {'+
'\n                                    if (checkIfColorIsRange(20, bitmap, new int[]{'+"i, "+y+","+toInt(rgb.red)+", "+toInt(rgb.green)+","+toInt(rgb.blue)+'})) {'+
'\n'+s+

'                                    }'+
'                                }'

    $.writeln(s);

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
    saveAsTextFile(saveFile, s);