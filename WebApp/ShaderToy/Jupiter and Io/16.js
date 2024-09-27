

function ShaderToy(parentElement) {
    if (parentElement === null) return;

    var me = this;


    this.mNeedsSave = false;
    this.mAreThereAnyErrors = false;
    this.mAudioContext = null;
    this.mCreated = false;
    this.mHttpReq = null;
    this.mEffect = null;
    this.mTo = null;
    this.mTOffset = 0;
    this.mCanvas = null;
    this.mFPS = piCreateFPSCounter();
    this.mIsPaused = false;
    this.mForceFrame = false;
    this.mInfo = null;
    this.mCharCounter = document.getElementById("numCharacters");
    this.mCharCounterTotal = document.getElementById("numCharactersTotal");
    this.mEleCompilerTime = document.getElementById("compilationTime");
    this.mPass = [];
    this.mActiveDoc = 0;
    this.mIsEditorFullScreen = false;
    this.mFontSize = 0;
    this.mVR = null;
    this.mState = 0;

    //buildInputsUI( this );

    this.mCanvas = document.getElementById("demogl");
    this.mCanvas.tabIndex = "0"; // make it react to keyboard
    this.mCanvas.width =512 //this.mCanvas.offsetWidth;
    this.mCanvas.height =288 //this.mCanvas.offsetHeight;

    // ---------------------------------------

    this.mHttpReq = new XMLHttpRequest();
    this.mTo = getRealTime();
    this.mTf = 0;
    this.mRestarted = true;
    this.mFPS.Reset(this.mTo);
    this.mMouseIsDown = false;
    this.mMouseSignalDown = false;
    this.mMouseOriX = 0;
    this.mMouseOriY = 0;
    this.mMousePosX = 0;
    this.mMousePosY = 0;
    this.mIsRendering = false;

    // --- audio context ---------------------

    this.mAudioContext = piCreateAudioContext();

    if (this.mAudioContext === null) {
        //alert( "no audio!" );
    }

    // --- vr susbsystem ---------------------
    /*
        this.mVR = new WebVR( function(b) 
                              {
                                   var ele = document.getElementById("myVR");
                                   if( b )
                                       ele.style.background="url('/img/themes/" + gThemeName + "/vrOn.png')";
                                   else
                                       ele.style.background="url('/img/themes/" + gThemeName + "/vrOff.png')";
                              },
                              this.mCanvas );
    */
    // --- soundcloud context ---------------------
    this.mSoundcloudImage = new Image();

    window.onfocus = function () {
        if (!this.mIsPaused) {
            me.mTOffset = me.mTf;
            me.mTo = getRealTime();
            me.mRestarted = true;
        }
    };


    // ---------------

    this.mErrors = new Array();



    //refreshCharsAndFlags(this);
    //this.setCompilationTime();
    //this.mEleCompilerTime.textContent = "";




    this.mCanvas.onmousedown = function (ev) {
        var rect = me.mCanvas.getBoundingClientRect();
        me.mMouseOriX = Math.floor((ev.clientX - rect.left) / (rect.right - rect.left) * me.mCanvas.width);
        me.mMouseOriY = Math.floor(me.mCanvas.height - (ev.clientY - rect.top) / (rect.bottom - rect.top) * me.mCanvas.height);
        me.mMousePosX = me.mMouseOriX;
        me.mMousePosY = me.mMouseOriY;
        me.mMouseIsDown = true;
        me.mMouseSignalDown = true;
        if (me.mIsPaused) me.mForceFrame = true;
        //        return false; // prevent mouse pointer change
    }
    this.mCanvas.onmousemove = function (ev) {
        if (me.mMouseIsDown) {
            var rect = me.mCanvas.getBoundingClientRect();
            me.mMousePosX = Math.floor((ev.clientX - rect.left) / (rect.right - rect.left) * me.mCanvas.width);
            me.mMousePosY = Math.floor(me.mCanvas.height - (ev.clientY - rect.top) / (rect.bottom - rect.top) * me.mCanvas.height);
            if (me.mIsPaused) me.mForceFrame = true;
        }
    }
    this.mCanvas.onmouseup = function (ev) {
        me.mMouseIsDown = false;
        if (me.mIsPaused) me.mForceFrame = true;
    }

    this.mCanvas.addEventListener("keydown", function (ev) {
        me.mEffect.SetKeyDown(me.mActiveDoc, ev.keyCode);
        if (me.mIsPaused) me.mForceFrame = true;
        ev.preventDefault();
    }, false);

    this.mCanvas.addEventListener("keyup", function (ev) {
        if ((ev.keyCode == 82) && ev.altKey) {
            let r = document.getElementById("myRecord");
            r.click();
        }

        me.mEffect.SetKeyUp(me.mActiveDoc, ev.keyCode);
        if (me.mIsPaused) me.mForceFrame = true;
        ev.preventDefault();
    }, false);



    this.mEffect = new Effect(this.mVR, this.mAudioContext, this.mCanvas, null, this, false, false, null, null);
    if (!this.mEffect.mCreated) {
        console.log("Effect created")
        return;
    }

    // --- mediaRecorder ---------------------



    this.mCreated = true;
}


ShaderToy.prototype.startRendering = function () {
    this.mIsRendering = true;
    var me = this;

    function renderLoop2() {
        me.mEffect.RequestAnimationFrame(renderLoop2);

        if (me.mIsPaused && !me.mForceFrame) {
            me.mEffect.UpdateInputs(me.mActiveDoc, false);
            return;
        }
        me.mForceFrame = false;

        var time = getRealTime();

        var ltime = 0.0;
        var dtime = 0.0;
        if (me.mIsPaused) {
            ltime = me.mTf;
            dtime = 1000.0 / 60.0;
        }
        else {
            ltime = me.mTOffset + time - me.mTo;
            if (me.mRestarted)
                dtime = 1000.0 / 60.0;
            else
                dtime = ltime - me.mTf;
            me.mTf = ltime;
        }
        me.mRestarted = false;

        var newFPS = me.mFPS.Count(time);

        let mouseOriX = Math.abs(me.mMouseOriX);
        let mouseOriY = Math.abs(me.mMouseOriY);
        if (!me.mMouseIsDown) mouseOriX = -mouseOriX;
        if (!me.mMouseSignalDown) mouseOriY = -mouseOriY;
        me.mMouseSignalDown = false;

        me.mEffect.Paint(ltime / 1000.0, dtime / 1000.0, me.mFPS.GetFPS(), mouseOriX, mouseOriY, me.mMousePosX, me.mMousePosY, me.mIsPaused);


    }

    renderLoop2();
}



ShaderToy.prototype.Load = function (jsn, preventCache, doResolve) {


    let me = this;

    if (!this.mEffect.Load(jsn)) return;


    this.mEffect.Compile(true /*true due to crash reporting*/, function (worked) {
        console.log("Load");
        me.startRendering();
    });

    this.mInfo = jsn.info;

    return {
        mDownloaded: true,
        mDate: jsn.info.date,
        mViewed: jsn.info.viewed,
        mName: jsn.info.name,
        mUserName: jsn.info.username,
        mDescription: jsn.info.description,
        mLikes: jsn.info.likes,
        mPublished: jsn.info.published,
        mHasLiked: jsn.info.hasliked,
        mTags: jsn.info.tags,
        mParentId: jsn.info.parentid,
        mParentName: jsn.info.parentname
    };

}

//----------------------------------------------------------------------------

var gShaderToy = null;
var gCode = null;
var gIsLiked = 0;
var gRes = null;

function iLoadShader(jsnShader) {

    gRes = gShaderToy.Load(jsnShader[0], gIsMyShader);

    if (gRes.mDownloaded === false)
        return;

}

async function loadShader() {
    gShaderToy.mState = 1;
    iLoadShader(await (await fetch('XXjSRc.json')).json());
}

async function watchInit() {
    var viewerParent = document.getElementById("player");

    gShaderToy = new ShaderToy(viewerParent);
    if (!gShaderToy.mCreated)
        return;


    await loadShader(gShaderID);
}



