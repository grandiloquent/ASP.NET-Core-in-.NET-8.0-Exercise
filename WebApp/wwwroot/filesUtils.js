const pathSeperator = "\\";
const baseUri = window.location.host === "127.0.0.1:5500" ? "http://192.168.8.55:8500" : "";
const searchParams = new URL(window.location).searchParams;

////////////////////////////////////////////////////////////////

async function loadData(path, size) {
    const res = await fetch(`${baseUri}/files?path=${encodeURIComponent(path || '')}`);
    return res.json();
}
function normalizePath(path){
    return  path || searchParams.get("path") || 'C:\\Users\\Administrator\\Desktop';
}
function sortFileList(res){
    let isSize="";
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
        }
            else {
                return x.name.localeCompare(y.name)
            }
    })
}
async function render(path) {
    setDocumentTitle(path);
    path = normalizePath(path);
    const res = await loadData(path);
    this.wrapper.innerHTML = sortFileList(res)
        .map(x => {
            return `<div class="item" data-path="${x.parent+pathSeperator+x.name}" data-isdirectory=${x.isDir}>
            <div class="item-icon ${x.isDir ? 'item-directory' : 'item-file'}" 
            ${imageRe.test(x.name) ? `style="background-repeat:no-repeat;background-size:contain;background-position:50% 50%;background-image:url(${baseUri}/file?path=${encodeURIComponent(x.parent+pathSeperator+x.name)})"` : ''}
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
////////////////////////////////////////////////////////////////
render();
