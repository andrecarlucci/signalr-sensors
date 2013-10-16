﻿function $r(parent, child) { (document.getElementById(parent)).removeChild(document.getElementById(child)); }

function $t(name) { return document.getElementsByTagName(name); }

function $c(code) { return String.fromCharCode(code); }

function $h(value) { return ('0' + Math.max(0, Math.min(255, Math.round(value))).toString(16)).slice(-2); }

function _i(id, value) { $t('div')[id].innerHTML += value; }

function _h(value) { return !hires ? value : Math.round(value / 2); }

function get_screen_size() {
    var w = document.documentElement.clientWidth;
    var h = document.documentElement.clientHeight;
    return Array(w, h);
}

var url = document.location.href;

var flag = true;
var test = true;
var n = 20;
var w = 0;
var h = 0;
var x = 0;
var y = 0;
var z = 0;
var star_color_ratio = 0;
var star_x_save, star_y_save;
var star_ratio = 256;
var star_speed = 4;
var star_speed_save = 0;
var star = new Array(n);
var color;
var opacity = 0.1;

var cursor_x = 0;
var cursor_y = 0;
var mouse_x = 0;
var mouse_y = 0;

var canvas_x = 0;
var canvas_y = 0;
var canvas_w = 0;
var canvas_h = 0;
var context;

var key;
var ctrl;

var timeout;
var fps = 5;

function init() {
    var a = 0;
    for (var i = 0; i < n; i++) {
        star[i] = new Array(5);
        star[i][0] = Math.random() * w * 2 - x * 2;
        star[i][1] = Math.random() * h * 2 - y * 2;
        star[i][2] = Math.round(Math.random() * z);
        star[i][3] = 0;
        star[i][4] = 0;
    }
    var starfield = document.getElementById('starfield');
    starfield.style.position = 'absolute';
    starfield.width = w;
    starfield.height = h;
    context = starfield.getContext('2d');
    //context.lineCap='round';
    //context.fillStyle = 'rgb(0,0,0)';
    context.strokeStyle = 'rgb(255,255,255)';
}

function anim() {
    mouse_x = cursor_x - x;
    mouse_y = cursor_y - y;
    context.fillRect(0, 0, w, h);
    for (var i = 0; i < n; i++) {
        test = true;
        star_x_save = star[i][3];
        star_y_save = star[i][4];
        star[i][0] += mouse_x >> 4;
        if (star[i][0] > x << 1) {
            star[i][0] -= w << 1;
            test = false;
        }
        if (star[i][0] < -x << 1) {
            star[i][0] += w << 1;
            test = false;
        }
        star[i][1] += mouse_y >> 4;
        if (star[i][1] > y << 1) {
            star[i][1] -= h << 1;
            test = false;
        }
        if (star[i][1] < -y << 1) {
            star[i][1] += h << 1;
            test = false;
        }
        star[i][2] -= star_speed;
        if (star[i][2] > z) {
            star[i][2] -= z;
            test = false;
        }
        if (star[i][2] < 0) {
            star[i][2] += z;
            test = false;
        }
        star[i][3] = x + (star[i][0] / star[i][2]) * star_ratio;
        star[i][4] = y + (star[i][1] / star[i][2]) * star_ratio;
        if (star_x_save > 0 && star_x_save < w && star_y_save > 0 && star_y_save < h && test) {
            context.lineWidth = (1 - star_color_ratio * star[i][2]) * 2;
            context.beginPath();
            context.moveTo(star_x_save, star_y_save);
            context.lineTo(star[i][3], star[i][4]);
            context.stroke();
            context.closePath();
        }
    }
    timeout = setTimeout('anim()', fps);
}

function move(evt) {
    evt = evt || event;
    cursor_x = evt.pageX - canvas_x;
    cursor_y = evt.pageY - canvas_y;
    console.log(cursor_x);
    console.log(cursor_y);
}

function key_manager(evt) {
    evt = evt || event;
    key = evt.which || evt.keyCode;
    //ctrl=evt.ctrlKey;
    switch (key) {
    case 27:
        flag = flag ? false : true;
        if (flag) {
            timeout = setTimeout('anim()', fps);
        } else {
            clearTimeout(timeout);
        }
        break;
    case 32:
        star_speed_save = (star_speed != 0) ? star_speed : star_speed_save;
        star_speed = (star_speed != 0) ? 0 : star_speed_save;
        break;
    case 13:
        context.fillStyle = 'rgba(0,0,0,' + opacity + ')';
        break;
    }
    top.status = 'key=' + ((key < 100) ? '0' : '') + ((key < 10) ? '0' : '') + key;
}

function release() {
    switch (key) {
    case 13:
        context.fillStyle = 'rgb(0,0,0)';
        break;
    }
}

function mouse_wheel(evt) {
    evt = evt || event;
    var delta = 0;
    if (evt.wheelDelta) {
        delta = evt.wheelDelta / 120;
    } else if (evt.detail) {
        delta = -evt.detail / 3;
    }
    star_speed += (delta >= 0) ? -0.2 : 0.2;
    if (evt.preventDefault) evt.preventDefault();
}

function start() {
    resize();
    anim();
}

function resize() {
    w = parseInt(get_screen_size()[0]);
    h = parseInt(get_screen_size()[1]);
    x = Math.round(w / 2);
    y = Math.round(h / 2);
    z = (w + h) / 2;
    star_color_ratio = 1 / z;
    cursor_x = x;
    cursor_y = y;
    init();
}

function startsDirection(x, y) {
    cursor_x = x;
    cursor_y = y;
}

function starsUp() {
    cursor_x = document.documentElement.clientWidth/2;
    cursor_y = 0;
}
function starsDown() {
    cursor_x = document.documentElement.clientWidth / 2;
    cursor_y = document.documentElement.clientHeight;
}
function starsLeft() {
    cursor_x = 0;
    cursor_y = document.documentElement.clientHeight / 2;
}
function starsRight() {
    cursor_x = document.documentElement.clientWidth;
    cursor_y = document.documentElement.clientHeight / 2;;
}

//document.onmousemove = move;
//document.onkeypress = key_manager;
//document.onkeyup = release;