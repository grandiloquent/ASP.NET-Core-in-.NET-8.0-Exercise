function playVideo(video, path) {
    document.title = substringAfterLast(path, "/");
    toast.setAttribute('message', document.title);
    video.load();
    video.src = path;
    video.play();
}

function substringAfterLast(string, delimiter, missingDelimiterValue) {
    const index = string.lastIndexOf(delimiter);
    if (index === -1) {
        return missingDelimiterValue || string;
    } else {
        return string.substring(index + delimiter.length);
    }
}

function appendSubtitle(video) {
    document.querySelectorAll('track').forEach(x => x.remove())
    const track = document.createElement('track');
    var tracks = video.textTracks;
    var numTracks = tracks.length;
    for (var i = numTracks - 1; i >= 0; i--)
        video.textTracks[i].mode = "disabled";
    track.src = substringBeforeLast(video.src, ".") + ".vtt";
    track.default = true;
    video.appendChild(track);
}

function substringBeforeLast(string, delimiter, missingDelimiterValue) {
    const index = string.lastIndexOf(delimiter);
    if (index === -1) {
        return missingDelimiterValue || string;
    } else {
        return string.substring(0, index);
    }
}

function jumpToSpecificTime(video) {
    const searchParams = new URL(window.location).searchParams;
    let t = searchParams.get('t');
    if (t) {
        const m = /(\d+)m(\d+)s/.exec(t);
        if (m) {
            video.currentTime = parseFloat(m[1]) * 60 + parseFloat(m[2]);
        }
    }
}

function saveBookmark(video, path) {
    const obj = JSON.parse(localStorage.getItem('bookmark') || '{}');
    obj[path] = video.currentTime;
    localStorage.setItem('bookmark', JSON.stringify(obj));
}

function formatDuration(ms) {
    if (isNaN(ms)) return '0:00';
    if (ms < 0) ms = -ms;
    const time = {
        hour: Math.floor(ms / 3600) % 24,
        minute: Math.floor(ms / 60) % 60,
        second: Math.floor(ms) % 60,
    };
    return Object.entries(time)
        .filter((val, index) => index || val[1])
        .map(val => (val[1] + '').padStart(2, '0'))
        .join(':');
}

function adjustSize(video) {
    if (video.videoWidth > 0) {
        const w = Math.min(window.outerWidth, window.innerWidth);
        const h = Math.min(window.outerHeight, window.innerHeight);
        let ratio = Math.min(w / video.videoWidth, h / video.videoHeight);
        let height = video.videoHeight * ratio
        let width = video.videoWidth * ratio;
        video.style.width = `${width}px`;
        video.style.height = `${height}px`;
        video.style.left = `${(w - width) / 2}px`;
        video.style.top = `${(h - height) / 2}px`
    }
}

function zoomIn(video, evt) {
    const b = video.getBoundingClientRect();
    const x = evt.clientX;
    const y = evt.clientY;

    const width = (x - b.left) / b.width * video.videoWidth;
    const height = (y - b.top) / b.height * video.videoHeight;


    video.style.width = video.videoWidth + 'px';
    video.style.height = video.videoHeight + 'px';
    video.style.left = (window.innerWidth / 2 - width) + 'px';
    video.style.top = (window.innerHeight / 2 - height) + 'px';
}

function originalSize(video) {
    let height = video.videoHeight * 1
    let width = video.videoWidth * 1
    video.style.width = `${width}px`;
    video.style.height = `${height}px`;
}

// ==============================================================================

const file = "How to Make a padlock.mp4";
const searchParams = new URL(window.location).searchParams;
const path = searchParams.get('path') || file;


let timer;
const topWrapper = document.querySelector('#top-wrapper');
const middleWrapper = document.querySelector('#middle-wrapper');
const bottomWrapper = document.querySelector('#bottom-wrapper');
const timeFirst = document.querySelector('#time-first');
const timeSecond = document.querySelector('#time-second');
const video = document.querySelector('#video');
const progressBarPlayed = document.querySelector('#progress-bar-played');
const toast = document.getElementById('toast');


const fullscreen = document.querySelector('#fullscreen');
fullscreen.addEventListener('click', async evt => {
    if (fullscreen.dataset.state === '1') {
        adjustSize(video);
        fullscreen.dataset.state = '0'
    } else {
        originalSize(video);
        fullscreen.dataset.state = '1'
    }
});
playVideo(video, path);
appendSubtitle(video);
jumpToSpecificTime(video)

video.addEventListener('durationchange', evt => {
    if (video.duration) {
        timeSecond.textContent = formatDuration(video.duration);
    }
    adjustSize(video);
});

video.addEventListener('timeupdate', evt => {
    if (video.currentTime) {
        timeFirst.textContent = formatDuration(video.currentTime);
        customSeekbar.value = (100 / video.duration) * video.currentTime;
    }
});


const customSeekbar = document.querySelector('#custom-seekbar');
customSeekbar.addEventListener("seekbarClicked", function () {
    scheduleHide();
    video.pause();
});
customSeekbar.addEventListener("seekbarInput", evt => {
    console.log(evt.detail);
    scheduleHide();
    var time = video.duration * evt.detail;
    video.currentTime = time;
});
video.muted = true;
video.addEventListener('play', evt => {
    scheduleHide();
    playPause.querySelector('path').setAttribute('d', 'M9 19H7V5h2Zm8-14h-2v14h2Z');
});
video.addEventListener('pause', evt => {
    playPause.querySelector('path').setAttribute('d', 'm7 4 12 8-12 8V4z');
});
////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
let seeking = false;
video.addEventListener('seeked', evt => {
    seeking = false;
});
video.addEventListener('seeking', evt => {
    seeking = true;
});
////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
video.addEventListener('ended', evt => {
    const url = new URL(video.src);
    const path = url.searchParams.get('path');
    let next = 0;
    for (let i = 0; i < videos.length; i++) {
        if (videos[i].path === path) {
            next = i;
        }
    }
    if (next + 1 < videos.length) {
        next = next + 1;
    } else {
        next = 0;
    }
    playVideo(video, videos[next].path);
});
const playPause = document.querySelector('#play-pause');
playPause.addEventListener('click', evt => {
    if (video.paused) {
        video.play();

    } else {
        video.pause();

    }
});

function scheduleHide() {
    if (timer) clearTimeout(timer);
    timer = setTimeout(() => {
        topWrapper.style.display = 'none';
        middleWrapper.style.display = 'none';
        bottomWrapper.style.display = 'none';
    }, 5000);
}

video.addEventListener('click', evt => {
    evt.stopPropagation();
    /*
    if (video.getBoundingClientRect().left === 0) {
        zoomIn(video, evt);
    } else {
        adjustSize(video);
    }*/
    zoomIn(video, evt);
});
document.querySelector('.wrapper').addEventListener('click', evt => {
    topWrapper.style.display = 'flex';
    middleWrapper.style.display = 'flex';
    bottomWrapper.style.display = 'block';
    scheduleHide();
})

const videoList = document.querySelector('#video-list');
videoList.addEventListener('click', evt => {
    showVideoList(baseUri, path, video);
});

window.addEventListener("resize", evt => {
    const w = Math.min(window.outerWidth, window.innerWidth);
    const h = Math.min(window.outerHeight, window.innerHeight);
    toast.setAttribute('message', `${w}x${h}`);
    adjustSize(video);
});
document.addEventListener('visibilitychange', evt => {
    saveBookmark(video, path);
})

////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
var last_media_time, last_frame_num, fps;
var fps_rounder = [];
var frame_not_seeked = true;

function ticker(useless, metadata) {
    var media_time_diff = Math.abs(metadata.mediaTime - last_media_time);
    var frame_num_diff = Math.abs(metadata.presentedFrames - last_frame_num);
    var diff = media_time_diff / frame_num_diff;
    if (
        diff &&
        diff < 1 &&
        frame_not_seeked &&
        fps_rounder.length < 50 &&
        video.playbackRate === 1 &&
        document.hasFocus()
    ) {
        fps_rounder.push(diff);
        fps = Math.round(1 / get_fps_average());
    }
    frame_not_seeked = true;
    last_media_time = metadata.mediaTime;
    last_frame_num = metadata.presentedFrames;
    if (fps_rounder.length < 50)
        video.requestVideoFrameCallback(ticker);
}

function get_fps_average() {
    return fps_rounder.reduce((a, b) => a + b) / fps_rounder.length;
}

video.requestVideoFrameCallback(ticker);
////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////

video.addEventListener("seeked", function () {
    fps_rounder.pop();
    frame_not_seeked = false;
});

const next = document.querySelector('#next');
next.addEventListener('click', evt => {
    scheduleHide();
    video.currentTime += 1 / fps;
});
const previous = document.querySelector('#previous');
previous.addEventListener('click', evt => {
    scheduleHide();
    video.currentTime -= 1 / fps;
});
////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////

window.addEventListener('keydown', async evt => {

    if (evt.key === 'ArrowLeft') {
        evt.preventDefault();
        video.currentTime -= 1;
    } else if (evt.key === 'ArrowRight') {
        evt.preventDefault();
        video.currentTime += .5;
    } else if (evt.key === 'ArrowUp') {
        evt.preventDefault();
        if (!seeking) {
            video.currentTime -= 1 / fps;
        }
    } else if (evt.key === 'ArrowDown') {
        evt.preventDefault();
        if (!seeking) {
            video.currentTime += 1 / fps;
        }
    } else if (evt.key === '0') {
        evt.preventDefault();
        if (fullscreen.dataset.state === '1') {
            adjustSize(video);
            fullscreen.dataset.state = '0'
        } else {
            originalSize(video);
            fullscreen.dataset.state = '1'
        }
    } else if (evt.key === ' ') {
        evt.preventDefault();
        if (video.paused) {
            video.play();
        } else {
            video.pause();
        }
    }
});


////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
var isDown = false;
video.addEventListener('mousedown', function (e) {
    isDown = true;
}, true);

video.addEventListener('mouseup', function () {
    isDown = false;
}, true);

video.addEventListener('mousemove', function (event) {
    event.preventDefault();
    if (isDown) {
        var deltaX = event.movementX;
        var deltaY = event.movementY;
        var rect = video.getBoundingClientRect();
        video.style.left = rect.x + deltaX + 'px';
        video.style.top = rect.y + deltaY + 'px';
    }
}, true);
