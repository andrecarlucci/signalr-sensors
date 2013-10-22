/// <reference path="jquery.signalR-1.1.3.js" />
/// <reference path="jquery-1.6.4.js" />
(function (game, starfield, daynight, compass, $, undefined) {
    
    var hub = $.connection.gameHub,
        $boss = $("#boss"),
        $clientCount = $("#clientCountSpan"),
        $deathCount = $("#deathCountSpan"),
        body = window.document.body,
        $me;

    $.extend(hub.client, {
        addOtherPlayer: function(player) {
            addPlayer(player);
        },
        addPlayer: function(player) {
            addPlayer(player);
            $me = $("#" + player.Id);
            $me.addClass("me");
            addDraggable($me);
            talk($me, 'Arraste-me para longe do monstro!');
        },
        removePlayer: function(playerId) {
            var div;
            if (div = document.getElementById(playerId)) {
                div.parentNode.removeChild(div);
            }
            talk($boss, 'Hmmmm!');
        },
        shapeMoved: function(id, x, y) {
            var $shape = $("#" + id);
            if (!$shape) {
                return;
            }
            $shape.css({
                left: (body.clientWidth - $shape.width()) * x,
                top: (body.clientHeight - $shape.height()) * y,
                visibility: 'visible'
            });
        },
        clientCountChanged: function(count) {
            $clientCount.text(count);
        },
        clientDeathChanged: function(count) {
            $deathCount.text(count);
        },
        windChanged: function(angle) {
            starfield.changeDirection(angle);
        },
        isDayChanged: function(isDay) {
            if (isDay) {
                daynight.letThereBeDay();
            } else {
                daynight.letThereBeNight();
            }
        },
        compassChanged: function(angle) {
            compass.setAngle(angle);
        }
    });

    function changeWind(angle) {
        hub.server.changeWind(angle);
    }
    
    function changeIsDay(isDay) {
        hub.server.changeIsDay(isDay);
    }

    function changeCompass(angle) {
        game.compassAngle = angle;
        hub.server.changeCompass(angle);
    }
    
    function addPlayer(player) {
        if (document.getElementById(player.Id)) {
            return;
        }
        $(body).append("<div class='player' id='" + player.Id + "'></div>");
        moveShape(player.Id, player.Px, player.Py);
    }

    function addDraggable($playerDiv) {
        $playerDiv.draggable({
            containment: "parent",
            drag: function () {
                var x = this.offsetLeft / (body.clientWidth - this.offsetWidth);
                var y = this.offsetTop / (body.clientHeight - this.offsetHeight);
                hub.server.movePlayer(x, y);
            }
        });
    }

    function moveShape(id, x, y) {
        var $shape = $("#"+id);
        $shape.css({
            left: (body.clientWidth - $shape.width()) * x,
            top: (body.clientHeight - $shape.height()) * y
        });
    }

    function talk($shape, text) {
        $shape.showBalloon({ contents: text });
        setTimeout(function () {
            $shape.hideBalloon();
        }, 2000);
    }

    game.start = function(done) {
        $.connection.hub.start().done(done || {});
    };
    game.changeWind = changeWind;
    game.changeIsDay = changeIsDay;
    game.changeCompass = changeCompass;
    game.compassAngle = 0;

}(window.game = window.game || {}, starfield, daynight, compass, jQuery));