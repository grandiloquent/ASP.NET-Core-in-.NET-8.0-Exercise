var pointSamples = app.activeDocument.colorSamplers

function toInt(value) {
    return value| 0;
}

var rgb = pointSamples[0].color.rgb;
var x = toInt(pointSamples[0].position[0]);
var y = toInt(pointSamples[0].position[1]);

var buffer = [];
for (var i = 1; i < pointSamples.length; i++) {
    var xx=toInt(pointSamples[i].position[0]);
    var yy=toInt(pointSamples[i].position[1]);
    var gg = pointSamples[i].color.rgb;
    buffer.push(xx+", i + ("+yy+" - "+y+"), "+toInt(gg.red)+", "+toInt(gg.green)+", "+toInt(gg.blue));
}


var s='if (checkIfColorIsRange(20, bitmap, new int[]{' +
buffer.join(",\n")+
    '})) {' +
    'click(accessibilityService, getRandomNumber('+x+', '+x+' + 100),getRandomNumber(i,i+40));'+
    '                                            break;' +
    '                                        }'

s='for (int i = '+toInt(pointSamples[pointSamples.length-2].position[1])+'; i < '+toInt(pointSamples[pointSamples.length-1].position[1])+'; i++) {'+
'\n                                    if (checkIfColorIsRange(20, bitmap, new int[]{'+x+", i, "+toInt(rgb.red)+", "+toInt(rgb.green)+","+toInt(rgb.blue)+'})) {'+
'\n'+s+
'\n                                        i += 964 - 882;'+
'                                    }'+
'                                }'

    $.writeln(s);
