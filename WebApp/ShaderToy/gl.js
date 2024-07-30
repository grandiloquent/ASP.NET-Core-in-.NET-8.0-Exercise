'use strict';
window.getShaderSource = function (id) {
    return document.getElementById(id).textContent.replace(/^\s+|\s+$/g, '');
};
function createShader(gl, source, type) {
    var shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    return shader;
}
function createFragmentShaderSource(fragmentShaderSource) {
    const s = (document.getElementById('share') && document.getElementById('share').textContent) || '';
    if (!s) {
        return fragmentShaderSource;
    } else {
        return s + fragmentShaderSource;
        // const index = fragmentShaderSource.lastIndexOf('uniform');
        // if (index === -1) {
        //     return s + fragmentShaderSource;
        // } else {
        //     let start = index;
        //     while (start  < fragmentShaderSource.length) {
        //         start++;
        //         if (fragmentShaderSource[start] === '\n') {
        //             start++;
        //             break;
        //         }
        //     }
        //     return fragmentShaderSource.substring(0, start) + s + fragmentShaderSource.substring(start);
        // }
    }
}
window.createProgram = function (gl, vertexShaderSource, fragmentShaderSource) {
    var program = gl.createProgram();
    var vshader = createShader(gl, vertexShaderSource, gl.VERTEX_SHADER);
    var fshader = createShader(gl, createFragmentShaderSource(fragmentShaderSource), gl.FRAGMENT_SHADER);
    gl.attachShader(program, vshader);
    gl.deleteShader(vshader);
    gl.attachShader(program, fshader);
    gl.deleteShader(fshader);
    gl.linkProgram(program);
    var log = gl.getProgramInfoLog(program);
    if (log) {
        console.log(log);
    }
    log = gl.getShaderInfoLog(vshader);
    if (log) {
        console.log(log);
    }
    log = gl.getShaderInfoLog(fshader);
    if (log) {
        const div = document.createElement("div");
        div.textContent = log;
        document.body.appendChild(div);
    }
    return program;
};

window.onerror = function (errMsg, url, line, column, error) {
    var result = !column ? '' : 'column: ' + column;
    result += !error;

    const div = document.createElement("div");
    div.textContent = "\nError= " + errMsg + "\nurl= " + url + "\nline= " + line + result;
    document.body.appendChild(div);
    var suppressErrorAlert = true;
    return suppressErrorAlert;
};
document.addEventListener('visibilitychange', async evt => {
    if (document.visibilityState === "visible") {
        location.reload();
    }
})
var canvas = document.createElement('canvas');
let canvasWidth_ = 512;
let canvasHeight = 288;
let isDate = false;
const dataset = document.querySelector("script#fs") && document.querySelector("script#fs").dataset;
if (dataset && dataset.size) {
    const array = dataset.size.split(' ');
    canvasWidth_ = parseInt(array[0], 10);
    canvasHeight = parseInt(array[1], 10);
}
if (dataset && dataset.isdate) {
    isDate = true;
}
canvas.height = canvasHeight;
canvas.width = canvasWidth_;
canvas.style.width = '100%';
// canvas.style.height = canvasHeight+'px';
if (window.innerWidth > 512) {
    canvas.style.width = '512px';
}
//canvas.style.height = canvasHeight + 'px';
document.body.appendChild(canvas);
var gl = canvas.getContext('webgl2', {
    antialias: false
});
var program = createProgram(gl, getShaderSource('vs'), getShaderSource('fs'));
const positionAttributeLocation = gl.getAttribLocation(program, "a_position");
const resolutionLocation = gl.getUniformLocation(program, "iResolution");
const mouseLocation = gl.getUniformLocation(program, "iMouse");
const timeLocation = gl.getUniformLocation(program, "iTime");
const frameLocation = gl.getUniformLocation(program, "iFrame");
const dateLocation = gl.getUniformLocation(program, "iDate");
const timeDeltaLocation = gl.getUniformLocation(program, "iTimeDelta");
const frameRateLocation = gl.getUniformLocation(program, "iFrameRate");

const vao = gl.createVertexArray();
gl.bindVertexArray(vao);
const positionBuffer = gl.createBuffer();
gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
    -1, -1,
    1, -1,
    -1, 1,
    -1, 1,
    1, -1,
    1, 1,
]), gl.STATIC_DRAW);
gl.enableVertexAttribArray(positionAttributeLocation);
gl.vertexAttribPointer(
    positionAttributeLocation,
    2,
    gl.FLOAT,
    false,
    0,
    0,
);
let mouseX = 0;
let mouseY = 0;
function setMousePosition(e) {
    const rect = canvas.getBoundingClientRect();
    mouseX = e.clientX - rect.left;
    mouseY = rect.height - (e.clientY - rect.top) - 1;
}
canvas.addEventListener('mousemove', setMousePosition);
canvas.addEventListener('touchstart', (e) => {
    e.preventDefault();
}, {
    passive: false
});
canvas.addEventListener('touchmove', (e) => {
    e.preventDefault();
    setMousePosition(e.touches[0]);
}, {
    passive: false
});

let frame = 0;
let then = 0;
gl.useProgram(program);
gl.uniform1i(gl.getUniformLocation(program, "iChannel0"), 0);
gl.uniform1i(gl.getUniformLocation(program, "iChannel1"), 1);
gl.bindVertexArray(vao);
function render(time) {
    time *= 0.001;
    frame++;
    gl.clearColor(0., 0., 0., 1.0);
    gl.clear(gl.COLOR_BUFFER_BIT);
    gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);
    gl.uniform3f(resolutionLocation, gl.canvas.width, gl.canvas.height, 1.0);
    gl.uniform4f(mouseLocation, mouseX, mouseY, mouseX, mouseY);
    gl.uniform1f(timeLocation, time);
    gl.uniform1f(timeDeltaLocation, then - time);
    gl.uniform1f(frameRateLocation, then - time / (1 / 60));
    then = time;

    gl.uniform1i(frameLocation, frame);
    if (isDate) {
        const d = new Date();
        let dates = [d.getFullYear(), // the year (four digits)
        d.getMonth(),	   // the month (from 0-11)
        d.getDate(),     // the day of the month (from 1-31)
        d.getHours() * 60.0 * 60 + d.getMinutes() * 60 + d.getSeconds() + d.getMilliseconds() / 1000.0];
        gl.uniform4fv(dateLocation, new Float32Array(dates));
    }
    gl.drawArrays(gl.TRIANGLES, 0, 6);
    requestAnimationFrame(render);
}
requestAnimationFrame(render);