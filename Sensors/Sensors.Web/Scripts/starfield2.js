(function (starfield, $, undefined) {
    'use strict';
    
    function getScreenSize() {
        var wid = document.documentElement.clientWidth;
        var hei = document.documentElement.clientHeight;
        return Array(wid, hei);
    }

    var angle = -1;
    var test = true;
    var n = 30;
    var w = 0;
    var h = 0;
    var x = 0;
    var y = 0;
    var z = 0;
    var starColorRatio = 0;
    var starXSave, starYSave;
    var starRatio = 256;
    var starSpeed = 4;
    var star = new Array(n);

    var cursorX = 0;
    var cursorY = 0;
    var mouseX = 0;
    var mouseY = 0;
    var context;

    var timeout;
    var fps = 5;

    function init() {
        for (var i = 0; i < n; i++) {
            star[i] = new Array(5);
            star[i][0] = Math.random() * w * 2 - x * 2;
            star[i][1] = Math.random() * h * 2 - y * 2;
            star[i][2] = Math.round(Math.random() * z);
            star[i][3] = 0;
            star[i][4] = 0;
        }
        var stars = document.getElementById('starfield');
        stars.style.position = 'absolute';
        stars.width = w;
        stars.height = h;
        context = stars.getContext('2d');
        context.lineCap='round';
        context.fillStyle = 'rgb(0,60,171)';
        context.strokeStyle = 'rgb(255,255,255)';
    }

    function anim() {
        mouseX = cursorX - x;
        mouseY = cursorY - y;
        context.fillRect(0, 0, w, h);
        for (var i = 0; i < n; i++) {
            test = true;
            starXSave = star[i][3];
            starYSave = star[i][4];
            star[i][0] += mouseX >> 4;
            if (star[i][0] > x << 1) {
                star[i][0] -= w << 1;
                test = false;
            }
            if (star[i][0] < -x << 1) {
                star[i][0] += w << 1;
                test = false;
            }
            star[i][1] += mouseY >> 4;
            if (star[i][1] > y << 1) {
                star[i][1] -= h << 1;
                test = false;
            }
            if (star[i][1] < -y << 1) {
                star[i][1] += h << 1;
                test = false;
            }
            star[i][2] -= starSpeed;
            if (star[i][2] > z) {
                star[i][2] -= z;
                test = false;
            }
            if (star[i][2] < 0) {
                star[i][2] += z;
                test = false;
            }
            star[i][3] = x + (star[i][0] / star[i][2]) * starRatio;
            star[i][4] = y + (star[i][1] / star[i][2]) * starRatio;
            if (starXSave > 0 && starXSave < w && starYSave > 0 && starYSave < h && test) {
                context.lineWidth = (1 - starColorRatio * star[i][2]) * 4;
                context.beginPath();
                context.moveTo(starXSave, starYSave);
                context.lineTo(star[i][3], star[i][4]);
                context.stroke();
                context.closePath();
            }
        }
        timeout = setTimeout('starfield.anim()', fps);
    }

    starfield.start = function() {
        resize();
        anim();
    };

    function resize() {
        w = parseInt(getScreenSize()[0]);
        h = parseInt(getScreenSize()[1]);
        x = Math.round(w / 2);
        y = Math.round(h / 2);
        z = (w + h) / 2;
        starColorRatio = 1 / z;
        cursorX = x;
        cursorY = y;
        init();
    }

    function startsDirection(x, y) {
        cursorX = x;
        cursorY = y;
    }

    function starsUp() {
        cursorX = document.documentElement.clientWidth / 2;
        cursorY = 0;
    }
    function starsDown() {
        cursorX = document.documentElement.clientWidth / 2;
        cursorY = document.documentElement.clientHeight;
    }
    function starsLeft() {
        cursorX = 0;
        cursorY = document.documentElement.clientHeight / 2;
    }
    function starsRight() {
        cursorX = document.documentElement.clientWidth;
        cursorY = document.documentElement.clientHeight / 2;;
    }
    function starsCenter() {
        cursorX = document.documentElement.clientWidth / 2;
        cursorY = document.documentElement.clientHeight / 2;
    }

    function changeDirection(newAngle) {
        if (!newAngle) return;
        if (newAngle < 0) {
            starsCenter();
        }
        else if (newAngle > 315 || newAngle <= 45) {
            starsRight();
        }
        else if (newAngle > 45 && newAngle <= 135) {
            starsUp();
        }
        else if (newAngle > 135 && newAngle <= 225) {
            starsLeft();
        }
        else if (newAngle > 225 && newAngle <= 315) {
            starsDown();
        }
        this.angle = newAngle;
    }

    document.body.onresize = document.body.onorientationchange = function() {
        starfield.resize();
        changeDirection(this.angle);
    };

    starfield.anim = anim;
    starfield.resize = resize;
    starfield.changeDirection = changeDirection;

}(window.starfield = window.starfield || {}, jQuery));

//document.onmousemove = move;
//document.onkeypress = key_manager;
//document.onkeyup = release;