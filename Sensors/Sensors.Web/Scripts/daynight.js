(function (daynight, $, undefined) {
    'use strict';

    var isDay = true;
    var time = 3000;

    daynight.letThereBeDay = function () {
        if (isDay) return;
        isDay = true;

        $('#sun_yellow').animate({ 'top': '20%', 'opacity': 1 }, time * 0.6);
        $('#stars').animate({ 'opacity': 0 }, time);
        $('#moon').animate({ 'top': '60%', 'opacity': 0 }, time * 0.25);
        $('#sstar').animate({ 'opacity': 0 }, time * 0.015);
        $('#sstar').animate({ 'backgroundPosition': '0px 0px', 'top': '15%', 'opacity': 1 }, time * 0.05);
        $('#night').animate({ 'opacity': 0 }, time);
    };
    
    daynight.letThereBeNight = function () {
        if (!isDay) return;
        isDay = false;
        $('#sun_yellow').animate({ 'top': '96%', 'opacity': 0.4 }, time * 0.6);
        $('#stars').animate({ 'opacity': 1 }, 5000);
        $('#moon').animate({ 'top': '30%', 'opacity': 1 }, time * 0.25);
        $('#sstar').animate({ 'opacity': 1 }, time * 0.015);
        $('#sstar').animate({ 'backgroundPosition': '0px 0px', 'top': '15%', 'opacity': 0 }, time * 0.05);
        $('#night').animate({ 'opacity': 0.7 }, time);
    };

}(window.daynight = window.daynight || {}, jQuery));