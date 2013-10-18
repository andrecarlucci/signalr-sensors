//adapted from
//http://homepage.ntlworld.com/ray.hammond/compass/compass.js

(function(compass, $, undefined) {
    'use strict';

    compass.setAngle = function (rotationrange) {
        var angle = parseInt(rotationrange, 10);
        $("#needle").rotate(angle);
    };
    
}(window.compass = window.compass || {}, jQuery));
