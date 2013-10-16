
(function(starfield, $, undefined) {
    'use strict';

    function moveStar($star, x, y) {
        $star.animate({
            left: '+=' + x,
            top: '+=' + y
        },100);
    }

    //$shape.css({
    //    left: (body.clientWidth - $shape.width()) * x,
    //    top: (body.clientHeight - $shape.height()) * y
    //});


}(window.starfield = window.starfield || {}, jQuery));

function MoveObjNN4(obj, xPos, yPos) {
    obj.top = yPos;
    obj.left = xPos;
}

function MoveObjOpera(obj, xPos, yPos) {
    obj.style.pixelTop = yPos - 25;
    obj.style.pixelLeft = xPos;
}

function MoveObjIE4(obj, xPos, yPos) {
    obj.style.pixelTop = yPos;
    obj.style.pixelLeft = xPos;
}

function MoveObjIE5NN6(obj, xPos, yPos) {
    obj.style.top = yPos + "px";
    obj.style.left = xPos + "px";
}

// horiz only functions for efficiency
function MoveObjHorizNN4(obj, xPos) {
    obj.left = xPos;
}

function MoveObjHorizOpera(obj, xPos) {
    obj.style.pixelLeft = xPos;

}

function MoveObjHorizIE4(obj, xPos) {
    obj.style.pixelLeft = xPos;
}

function MoveObjHorizIE5NN6(obj, xPos) {
    obj.style.left = xPos + "px";
}


var MoveObj = null;
var MoveObjHoriz = null;
function InitMoveObj() {
    if (document.layers) { //Netscape 4
        MoveObj = MoveObjNN4;
        MoveObjHoriz = MoveObjHorizNN4;
    }
    else if (navigator.userAgent.indexOf("Opera") != -1) { //Opera
        MoveObj = MoveObjOpera;
        MoveObjHoriz = MoveObjHorizOpera;
    }
    else if (document.all && !document.getElementById) { //IE 4
        MoveObj = MoveObjIE4;
        MoveObjHoriz = MoveObjHorizIE4;
    }
    else if (document.getElementById) { //Netscape 6 & IE 5
        MoveObj = MoveObjIE5NN6;
        MoveObjHoriz = MoveObjHorizIE5NN6;
    }
}
InitMoveObj();

function GetRandomSpeed() {
    speed = Math.random();
    speed = (-1.0 / 1) * Math.log(1.0 - speed); // uniform distribution becomes exponential, more slower stars
    speed = Math.round(speed * 10) + 1;
    return speed;
}

function Speed2Color() {
    color = 255 - 10 * (20 - speed); // the faster the brighter
    if (color > 255) color = 255;
    return color;
}

function getObjbyID(objId) {
    var myObj;
    if (document.layers) { //Netscape 4
        myObj = eval('document.' + objId);
    }
    else if (document.all && !document.getElementById) { //IE 4
        myObj = eval('document.all.' + objId);
    }
    else if (document.getElementById) { //Netscape 6 & IE 5
        myObj = document.getElementById(objId);
    }
    else {
        return null;
    }
    return myObj;
}

function SetStarColor(obj, color) {
    obj.style.color = 'rgb(' + color + "," + color + "," + color + ")";
}

var totalWidth;
var totalHeight;
var nStars;
var speeds;
var stars;
var star_tops;
var star_lefts;

function GetViewportWidth() {
    var myWidth = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
    } else if (document.documentElement &&
        (document.documentElement.clientWidth)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
    } else if (document.body && (document.body.clientWidth)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
    }
    return myWidth - 10;
}

function GetViewportHeight() {
    var myHeight = 0;
    if (typeof (window.innerHeight) == 'number') {
        //Non-IE
        myHeight = window.innerHeight;
    } else if (document.documentElement &&
        (document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientHeight)) {
        //IE 4 compatible
        myHeight = document.body.clientHeight;
    }
    return myHeight - 20;
}


function InitGlobals() {
    totalWidth = GetViewportWidth();
    totalHeight = GetViewportHeight();
    nStars = 20;
    speeds = new Array(nStars);
    stars = new Array(nStars);
    star_tops = new Array(nStars);
    star_lefts = new Array(nStars);
}

function StartStars() {
    InitStars();
    doStarAnim();
}

function InitStars() {
    InitGlobals();
    for (i = 0; i < nStars; i++) {
        var x = Math.round(Math.random() * totalWidth);
        var y = Math.round(Math.random() * totalHeight);
        star_tops[i] = y;
        star_lefts[i] = x;

        var star = document.createElement('span');
        star.innerHTML = '&middot;';
        star.className = 'star';
        star.id = 'star_' + i;
        stars[i] = star;
        document.body.appendChild(star);

        var speed = GetRandomSpeed();
        speeds[i] = speed;
        var color = Speed2Color(speed);
        SetStarColor(stars[i], color);
        MoveObj(stars[i], x, y);
    }
}

function doStarAnim() {
    for (i = 0; i < nStars; i++) {
        var newStarLeft = star_lefts[i] + speeds[i];
        var newStarTop = star_tops[i];

        if (newStarLeft >= totalWidth) {
            newStarLeft = star_lefts[i] = 0;
            speed = GetRandomSpeed();
            speeds[i] = speed;
            color = Speed2Color(speed);
            SetStarColor(stars[i], color);
        }

        star_lefts[i] = newStarLeft;
        MoveObjHoriz(stars[i], newStarLeft);

    }
    timerID = setTimeout("doStarAnim()", 10);
}

function RepositionStars() {
    totalWidth = GetViewportWidth();
    totalHeight = GetViewportHeight();

    for (i = 0; i < nStars; i++) {
        star_lefts[i] = Math.round(Math.random() * totalWidth);
        star_tops[i] = Math.round(Math.random() * totalHeight);
        MoveObj(stars[i], star_lefts[i], star_tops[i]);
    }
}

window.onresize = RepositionStars;
