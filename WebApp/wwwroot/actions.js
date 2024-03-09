

function addContextMenuItem(bottomSheet, title, handler) {
    const item = document.createElement('div');
    item.className = 'menu-item';
    item.textContent = title;
    bottomSheet.appendChild(item);
    item.addEventListener('click', handler);
}

async function downloadDirectory(path) {
    window.open(`${baseUri}/zip?path=${encodeURIComponent(path)}`, '_blank');
}
function getExtension(path) {
    const index = path.lastIndexOf('.');
    if (index !== -1) {
        return path.substr(index + 1);
    }
    return null;
}
function initializeDropZone() {
    document.addEventListener("DOMContentLoaded", evt => {
        var dropZone = document.querySelector('body');
        dropZone.addEventListener('dragover', function (e) {
            e.stopPropagation();
            e.preventDefault();
            e.dataTransfer.dropEffect = 'copy'
        });
        dropZone.addEventListener('drop', function (e) {
            e.stopPropagation();
            e.preventDefault();
            uploadFiles(e.dataTransfer.files)
        });
        async function uploadFiles(files) {
            document.querySelector('.dialog').className = 'dialog dialog-show';
            const dialogContext = document.querySelector('.dialog-content span');
            const length = files.length;
            let i = 1;
            for (let file of files) {
                dialogContext.textContent = `正在上传 (${i++}/${length}) ${file.name} ...`;
                const formData = new FormData;
                let path = new URL(location.href).searchParams.get('path') || "/storage/emulated/0";
                formData.append('path', path + "/" + file.name);
                formData.append('file', file, path + "/" + file.name);
                try {
                    await fetch(`${baseUri}/upload`, {
                        method: "POST",
                        body: formData
                    }).then(res => console.log(res))
                } catch (e) {
                }
            }
            document.querySelector('.dialog').className = 'dialog'
        }
    });
}

function newFile() {
    const dialog = document.createElement('custom-dialog');
    dialog.setAttribute('title', "新建文件")
    const input = document.createElement('input');
    input.type = 'text';
    dialog.appendChild(input);
    dialog.addEventListener('submit', async () => {
        let path = new URL(window.location).searchParams.get("path")
            || '/storage/emulated/0';
        const res = await fetch(`${baseUri}/file/new_file?path=${encodeURIComponent(path + "/" + input.value.trim())}`);
        window.location.reload();
    });
    document.body.appendChild(dialog);
}

function onDelete() {
    const dialog = document.createElement('custom-dialog');
    dialog.setAttribute('title', '删除文件');
    const div = document.createElement('div');
    div.className = "list-wrapper";
    const obj = JSON.parse(localStorage.getItem('paths') || "[]");
    const buf = [];
    for (let index = 0; index < obj.length; index++) {
        const element = obj[index];
        buf.push(`<div class="list-item" data-path="${element}"><div class="list-item-text">${element}</div>
        <div class="list-item-action">删除</div>
        </div>`);
    }
    div.innerHTML = buf.join('');
    dialog.appendChild(div);
    div.querySelectorAll('.list-item').forEach(listItem => {
        listItem.addEventListener('click', evt => {
            let index = obj.indexOf(listItem.dataset.path);
            if (index !== -1) {
                obj.splice(index, 1);
            }
            listItem.remove();
        });
    });
    dialog.addEventListener('submit', async () => {
        const res = await fetch(`${baseUri}/file/delete`, {
            method: 'POST',
            body: JSON.stringify(obj)
        });
        localStorage.setItem('paths', '');
        location.reload();
    });
    document.body.appendChild(dialog);
}

function selectSameType(path, isDirectory) {
    const extension = getExtension(path);
    const buf = [];
    document.querySelectorAll('.item').forEach(item => {
        const isdirectory = item.dataset.isdirectory === 'true';
        if (isDirectory) {
            if (isdirectory) {
                buf.push(item.dataset.path);
            }
        } else {
            if (!isdirectory) {
                if (extension === getExtension(item.dataset.path)) {
                    buf.push(item.dataset.path);
                }
            }
        }
    });
    localStorage.setItem("paths", JSON.stringify(buf));
    toast.setAttribute('message', '已成功写入剪切板');
}
function setDocumentTitle(path) {
    if (!path) return;
    document.title = substringAfterLast(decodeURIComponent(path), "/")
}
function showContextMenu(evt) {
    evt.stopPropagation();
    const dataset = evt.currentTarget.parentNode.dataset;
    const path = dataset.path;
    const isDirectory = dataset.isdirectory === 'true';
    const bottomSheet = document.createElement('custom-bottom-sheet');
    addContextMenuItem(bottomSheet, '复制路径', () => {
        bottomSheet.remove();
        //writeText(`data-image="/file?path=${path}"`);
        writeText(`${path}`);

    });
    addContextMenuItem(bottomSheet, '选择', () => {
        bottomSheet.remove();
        localStorage.setItem("paths", JSON.stringify([path]));
        toast.setAttribute('message', '已成功写入剪切板');
    });
    addContextMenuItem(bottomSheet, '选择相同类型', () => {
        bottomSheet.remove();
        selectSameType(path, isDirectory);
    });
    addContextMenuItem(bottomSheet, '重命名', () => {
        bottomSheet.remove();
        renameFile(path);
    });
    addContextMenuItem(bottomSheet, '删除', () => {
        bottomSheet.remove();
        deleteFile(path);
    });
    if (isDirectory) {
        addContextMenuItem(bottomSheet, '加入收藏夹', () => {
            bottomSheet.remove();
            addFavorite(path);
        });
        addContextMenuItem(bottomSheet, '压缩', () => {
            bottomSheet.remove();
            downloadDirectory(path);
        });
    } else {
        if (zipRe.test(path)) {
            addContextMenuItem(bottomSheet, '解压', () => {
                bottomSheet.remove();
                unCompressFile(path);
            });
        } else if (videoRe.test(path)) {
            addContextMenuItem(bottomSheet, '显示视频信息', () => {
                bottomSheet.remove();
                showVideoInformation(path);
            });
            addContextMenuItem(bottomSheet, '预览', () => {
                bottomSheet.remove();
                let imgPath = `${substringBeforeLast(path, "/")}/.images/${substringAfterLast(path, "/")}`;
                showImage(imgPath)
            });
        } else if (path.endsWith('.srt')) {
            addContextMenuItem(bottomSheet, '播放视频', () => {
                bottomSheet.remove();
                window.open(`/srt.html?path=${encodeURIComponent(path)}`);
            });
        }
        addContextMenuItem(bottomSheet, '分享', () => {
            bottomSheet.remove();
            if (typeof NativeAndroid !== 'undefined') {
                NativeAndroid.share(path);
            } else {
                let mimetype = "application/*"
                if (imageRe.test(path)) {
                    mimetype = "image/png";
                } else if (videoRe.test(path)) {
                    mimetype = "video/*";
                }
                fetch(`/su?cmd="${`am start -a android.intent.action.SEND -t ${mimetype} --eu android.intent.extra.STREAM 'file://${encodeURI(path)}' --grant-read-uri-permission"`}`)
            }
        });
        addContextMenuItem(bottomSheet, '扫描', () => {
            bottomSheet.remove();
            if (typeof NativeAndroid !== 'undefined') {
                NativeAndroid.scanFile(path);
            }
        });
    }
    document.body.appendChild(bottomSheet);
}
function onMenu(evt) {
    evt.stopPropagation();
    const bottomSheet = document.createElement('custom-bottom-sheet');
    addContextMenuItem(bottomSheet, '大小', () => {
        bottomSheet.remove();
        const url = new URL(window.location);
        url.searchParams.set('size', true);
        window.location = url;
    });
    // createPdfFromImages
    addContextMenuItem(bottomSheet, '时间', () => {
        bottomSheet.remove();
        const url = new URL(window.location);
        url.searchParams.set('size', "0");
        window.location = url;
    });
    addContextMenuItem(bottomSheet, '合并图片', () => {
        bottomSheet.remove();
        if (typeof NativeAndroid !== 'undefined') {
            const url = new URL(window.location);
            const path = url.searchParams.get('path');
            NativeAndroid.combineImages(path, 400, null)
        }
    });
    addContextMenuItem(bottomSheet, '创建PDF', () => {
        bottomSheet.remove();
        if (typeof NativeAndroid !== 'undefined') {
            const url = new URL(window.location);
            const path = url.searchParams.get('path');
            NativeAndroid.createPdfFromImages(path)
        }
    });
    document.body.appendChild(bottomSheet);
}

async function addFavorite(path) {
    const res = await fetch(`${baseUri}/fav/insert?path=${path}`);
    toast.setAttribute('message', '成功');
}
async function unCompressFile(path) {
    let res;
    try {
        res = await fetch(`${baseUri}/unzip?path=${path}`);
        toast.setAttribute('message', '成功');
    } catch (error) {
        toast.setAttribute('message', '错误');
    }
}
function showVideoInformation(path) {
    const dialog = document.createElement('custom-dialog');
    const div = document.createElement('div');
    dialog.title = "视频信息";
    if (typeof NativeAndroid !== 'undefined') {
        const lines = NativeAndroid.probe(path);
        writeText(lines);
        div.innerHTML = lines.split('\n').map(x => `<div style="white-space:pre">${x}</div>`).join('');
    }
    dialog.appendChild(div);
    dialog.addEventListener('submit', async () => {

    });
    document.body.appendChild(dialog);
}
function showImage(path) {
    const div = document.createElement('div');
    div.className = 'photo-viewer';
    const deleteButton = document.createElement('button');
    deleteButton.textContent = "X"
    deleteButton.style = `
    position: fixed;
    background: #fff;
    z-index: 10001;
    left: 32px;
    top: 32px;
    padding: 12px;
    border-radius: 50%;
    height: 32px;
    width: 32px;
    line-height: 32px;
    font-size: 24px;
    box-sizing: content-box;
`
    div.appendChild(deleteButton);
    deleteButton.addEventListener('click',async evt => {
        const res = await fetch(`${baseUri}/file/delete`, {
            method: 'POST',
            body: JSON.stringify([path])
        });
        queryElementByPath(path).remove();
    })
    const img = document.createElement('img');
    img.src = `${baseUri}/file?path=${encodeURIComponent(path)}`
    div.appendChild(img);
    document.body.appendChild(div);
    img.addEventListener('click', () => {
        div.remove();
    })
    div.addEventListener('click', () => {
        div.remove();
    })
}