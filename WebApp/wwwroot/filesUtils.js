const pathSeperator = "\\";
const baseUri = window.location.host === "127.0.0.1:5500" ? "http://192.168.8.55:8500" : "";
const searchParams = new URL(window.location).searchParams;
const DEFAULT_PATH = 'C:\\Users\\Administrator\\Desktop';

////////////////////////////////////////////////////////////////

async function loadData(path, size) {
    const res = await fetch(`${baseUri}/files?path=${encodeURIComponent(path || '')}${(size && "&isSize=1") || ''}`);
    return res.json();
}

function normalizePath(path) {
    return path || searchParams.get("path") || DEFAULT_PATH;
}

function sortFileList(res) {
    let isSize = searchParams.get("isSize");
    return res.sort((x, y) => {
        if (x.isDir !== y.isDir) if (x.isDir) return -1; else return 1;
        if (isSize === "0") {
            const dif = y.lastWriteTime - x.lastWriteTime;
            if (dif > 0) {
                return 1;
            } else if (dif < 0) {
                return -1;
            } else {
                return 0;
            }
        } else if (isSize) {
            if (y.length && x.length) {
                const dif = y.length - x.length;
                if (dif > 0) {
                    return 1;
                } else if (dif < 0) {
                    return -1;
                } else {
                    return 0;
                }
            }
        } else {
            const dif = y.lastModified - x.lastModified;//y.length - x.length;
            if (dif > 0) {
                return 1;
            } else if (dif < 0) {
                return -1;
            } else {
                return 0;
            }
            //return x.name.localeCompare(y.name)
        }
    })
}

async function render(path) {
    setDocumentTitle(path);
    path = normalizePath(path);
    const res = await loadData(path, searchParams.get("isSize"));
    this.wrapper.innerHTML = sortFileList(res)
        .map(x => {
            return `<div class="item" data-path="${x.parent + pathSeperator + x.name}" data-isdirectory=${x.isDir}>
            <div class="item-icon ${x.isDir ? 'item-directory' : 'item-file'}" 
            ${imageRe.test(x.name) ? `style="background-repeat:no-repeat;background-size:contain;background-position:50% 50%;background-image:url(${baseUri}/file?path=${encodeURIComponent(x.parent + pathSeperator + x.name)})"` : ''}
            ></div>
          <div class="item-title">
          <div>${x.name}</div>
          <div class="item-subtitle" style="${x.length === 0 ? 'display:none' : ''}">${humanFileSize(x.length)}</div>
          </div>

          <div class="item-more">
            <svg viewBox="0 0 24 24">
              <path d="M12 15.984q0.797 0 1.406 0.609t0.609 1.406-0.609 1.406-1.406 0.609-1.406-0.609-0.609-1.406 0.609-1.406 1.406-0.609zM12 9.984q0.797 0 1.406 0.609t0.609 1.406-0.609 1.406-1.406 0.609-1.406-0.609-0.609-1.406 0.609-1.406 1.406-0.609zM12 8.016q-0.797 0-1.406-0.609t-0.609-1.406 0.609-1.406 1.406-0.609 1.406 0.609 0.609 1.406-0.609 1.406-1.406 0.609z"></path>
            </svg>
          </div>
          </div>`
        }).join('');
    document.querySelectorAll('.item').forEach(item => {
        item.addEventListener('click', onItemClick);
    })
    document.querySelectorAll('.item-more').forEach(item => {
        item.addEventListener('click', showContextMenu);
    })
    document.querySelectorAll('.item-icon').forEach(item => {

        item.addEventListener('click', async evt => {
            evt.stopPropagation();
            // if (buf.indexOf(item.parentNode.dataset.path) === -1)
            //     buf.push(item.parentNode.dataset.path);
            // localStorage.setItem("paths", JSON.stringify(buf));
            if (new URL(window.location).searchParams.get("y") === "true") {

                const res = await fetch(`${baseUri}/file/delete`, {
                    method: 'POST',
                    body: JSON.stringify([item.parentNode.dataset.path])
                });

                queryElementByPath(item.parentNode.dataset.path).remove();
            } else {
                deleteFile(item.parentNode.dataset.path);
                const buf = (localStorage.getItem("paths") && JSON.parse(localStorage.getItem("paths"))) || [];
            }
        });
    })


}

function queryElementByPath(path) {
    return [...document.querySelectorAll(".item")].filter(x => x.dataset.path === path)
        ;
}

function deleteFile(path) {
    const dialog = document.createElement('custom-dialog');
    const div = document.createElement('div');
    div.textContent = `您确定要删除 ${substringAfterLast(path, "/")} 吗？`;
    dialog.appendChild(div);
    dialog.addEventListener('submit', async () => {
        if (/\.(?:blend)$/.test(path)) {
            fetch(`/movevideo?path=${encodeURIComponent(path)}`);
        } else {
            const res = await fetch(`${baseUri}/file/delete`, {
                method: 'POST',
                body: JSON.stringify([path])
            });
        }
        queryElementByPath(path).forEach(x => x.remove())
    });
    document.body.appendChild(dialog);
}

function renameFile(path) {

    const dialog = document.createElement('custom-dialog');
    dialog.setAttribute('title', "重命名")
    const input = document.createElement('textarea');
    input.value = substringAfterLast(path, pathSeperator);
    const re = /[(（]/;
    if (re.test(input.value)
    ) {
        let filename = `${input.value.split(re)[0].trim()}.${substringAfterLast(input.value, ".")}`;
        if (filename.indexOf("《") !== -1 && filename.indexOf("》") !== -1) {
            filename = substringAfterLast(filename, "《")
            filename = substringBeforeLast(filename, "》") + "." + substringAfterLast(filename, ".")
        }
        input.value = filename;

    }
    dialog.appendChild(input);
    dialog.addEventListener('submit', async () => {
        const filename = substringBeforeLast(path, pathSeperator) + pathSeperator + input.value.trim();
        const res = await fetch(`${baseUri}/file/rename?path=${encodeURIComponent(path)}&dst=${encodeURIComponent(filename)}`);
        let item = queryElementByPath(path)[0];
        item.querySelector('.item-title div').textContent = substringAfterLast(filename, pathSeperator);
        item.dataset.path = filename;
        //window.location.reload();
    });
    document.body.appendChild(dialog);
}

function onMove() {
    const dialog = document.createElement('custom-dialog');
    dialog.setAttribute('title', '移动文件');
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
    let path = new URL(window.location).searchParams.get("path")
        || DEFAULT_PATH;
    dialog.addEventListener('submit', async () => {
        const res = await fetch(`${baseUri}/file/move?dst=${encodeURIComponent(path)}`, {
            method: 'POST',
            body: JSON.stringify(obj)
        });
        localStorage.setItem('paths', '');
        location.reload();
    });
    document.body.appendChild(dialog);
}

function newDirectory() {
    const dialog = document.createElement('custom-dialog');
    dialog.setAttribute('title', "新建文件夹")
    const input = document.createElement('input');
    input.type = 'text';
    dialog.appendChild(input);
    dialog.addEventListener('submit', async () => {
        let path = new URL(window.location).searchParams.get("path")
            || DEFAULT_PATH;
        const res = await fetch(`${baseUri}/file/new_dir?path=${encodeURIComponent(path + pathSeperator + input.value.trim())}`);
        window.location.reload();
    });
    document.body.appendChild(dialog);
}

function addFavoriteItem(bottomSheet, path) {
    const item = document.createElement('div');
    item.className = 'menu-item';

    const div = document.createElement('div');
    div.style = `height: 48px;display: flex;align-items: center;justify-content: center`
    div.innerHTML = `<span class="material-symbols-outlined">
close
</span>`;
    div.addEventListener('click', async evt => {
        evt.stopPropagation();
        bottomSheet.remove();
        let res;
        try {
            res = await fetch(`${baseUri}/fav/delete?path=${encodeURIComponent(path)}`);
            if (res.status !== 200) {
                throw new Error();
            }
            toast.setAttribute('message', '成功');
        } catch (error) {
            toast.setAttribute('message', '错误');
        }
    });
    item.appendChild(div);

    const textElement = document.createElement('div');
    textElement.textContent = path;
    item.appendChild(textElement);

    bottomSheet.appendChild(item);
    item.addEventListener('click', () => {
        bottomSheet.remove();
        const url = new URL(window.location);
        url.searchParams.set('path', path);
        window.location = url;
    });
}

async function onShowFavorites() {
    const bottomSheet = document.createElement('custom-bottom-sheet');
    //const res = await fetch(`${baseUri}/fav/list`);
    [
        "C:\\Users\\Administrator\\Downloads",
        "C:\\Users\\Administrator\\Desktop\\文档",
        "C:\\Users\\Administrator\\Desktop\\视频",
        "C:\\Users\\Administrator\\Desktop\\视频\\文件",
        "D:\\文档",
        "D:\\",
    ].forEach(p => {
        addFavoriteItem(bottomSheet, p);
    })
    document.body.appendChild(bottomSheet);
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
            const length = files.length;
            let i = 1;
            for (let file of files) {
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
        }
    });
}
initializeDropZone();
////////////////////////////////////////////////////////////////
render();
