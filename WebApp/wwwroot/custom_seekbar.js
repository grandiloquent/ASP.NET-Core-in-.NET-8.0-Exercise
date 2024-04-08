(() => {

  class CustomSeekbar extends HTMLElement {

    constructor() {
      super();
      this.attachShadow({
        mode: 'open'
      });
      const wrapper = document.createElement("div");
      wrapper.setAttribute("class", "wrapper");
      const style = document.createElement('style');
      style.textContent = `
      .wrapper{
        padding: 24px 0;
        display: flex;
        align-items: center;
        position: relative;
    }
      input[type=range] {
        
        cursor: pointer;
        position: relative;
        background-color: transparent;
        border: none;
        outline: none;
        width: 100%;
        padding: 0;
      }
      
      input[type=range]:focus {
        outline: none;
      }
      
      
      input[type=range] {
        -webkit-appearance: none;
      }
      
      input[type=range]::-webkit-slider-runnable-track {
        height: 2px;
        background: rgba(194, 192, 194, 0.8);
        border: none;
      }
      
      input[type=range]::-webkit-slider-thumb {
        -webkit-appearance: none;
        border: none;
        height: 14px;
        width: 14px;
        border-radius: 50%;
        background-color: #f00;
        -webkit-transform-origin: 50% 50%;
                transform-origin: 50% 50%;
        margin: -6px 0 0 0;
        transition: all .3s ease-in-out;
      }
      input[type=range]::-webkit-slider-thumb:hover {
         -ms-transform: scale(1.2); /* IE 9 */
          -webkit-transform: scale(1.2); /* Safari */
          transform: scale(1.2);
      }
      
      input[type=range]:focus::-webkit-slider-runnable-track {
        background: #ccc;
      }
      input[type=range]::-moz-range-track {
        height: 3px;
        background: #ddd;
        border: none;
      }
      
      input[type=range]::-moz-range-thumb {
        border: none;
        height: 14px;
        width: 14px;
        border-radius: 50%;
        background: #26a69a;
        margin-top: -5px;
      }
      
      input[type=range]:-moz-focusring {
        outline: 1px solid #fff;
        outline-offset: -1px;
      }
      
      input[type=range]:focus::-moz-range-track {
        background: #ccc;
      }
      
      input[type=range]::-ms-track {
        height: 3px;
        background: transparent;
        border-color: transparent;
        border-width: 6px 0;
        /*remove default tick marks*/
        color: transparent;
        
      }
      
      input[type=range]::-ms-fill-lower {
        background: #777;
      }
      
      input[type=range]::-ms-fill-upper {
        background: #ddd;
      }
      
      input[type=range]::-ms-thumb {
        border: none;
        height: 14px;
        width: 14px;
        border-radius: 50%;
        background: #26a69a;
      }
      
      input[type=range]:focus::-ms-fill-lower {
        background: #888;
      }
      
      input[type=range]:focus::-ms-fill-upper {
        background: #ccc;
      }`;
      this.wrapper = wrapper;
      this.shadowRoot.append(style, wrapper);
    }

    static get observedAttributes() {
      return ['title'];
    }


    onSeekbarClicked(evt) {
      evt.stopPropagation();
      this.dispatchEvent(new CustomEvent('seekbarClicked'));
    }
    onSeekbarInput(evt) {
      evt.stopPropagation();
      this.dispatchEvent(new CustomEvent('seekbarInput', {
        detail: evt.currentTarget.value / 100
      }));
    }
    connectedCallback() {
      this.wrapper.innerHTML = `<input type="range" id="progress-bar-played" value="0">`;
      const input = this.wrapper.querySelector('input');
      input.addEventListener("mousedown", evt => {
        this.onSeekbarClicked(evt)
      });
      input.addEventListener("input", evt => {
        this.onSeekbarInput(evt)
      });
      this.input = input;
    }
    set value(value) {
      this.input.value = value;
    }

    attributeChangedCallback(name, oldValue, newValue) {
    }
  }

  customElements.define('custom-seekbar', CustomSeekbar);


})();
        /*
const seekbar = document.createElement('custom-seekbar');
seekbar.setAttribute('title', "询问");
const div = document.createElement('div');
div.textContent = "确定删除吗?";
seekbar.appendChild(div);
document.body.appendChild(seekbar);
seekbar.addEventListener('submit', evt => {

})

<custom-seekbar id="custom-seekbar"></custom-seekbar>

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

https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/range

*/