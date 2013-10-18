﻿/// <reference path="jquery.signalR-1.1.3.js" />
/// <reference path="jquery-1.6.4.js" />
(function (game, starfield, $, undefined) {
    
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
        },
        removePlayer: function(playerId) {
            var div;
            if (div = document.getElementById(playerId)) {
                div.parentNode.removeChild(div);
            }
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
            //console.log("Moved: " + $shape.attr('id') + " left: " + x + " right: " + y);
        },
        clientCountChanged: function(count) {
            $clientCount.text(count);
        },
        clientDeathChanged: function (count) {
            $deathCount.text(count);
        },
        windChanged: function(angle) {
            starfield.changeDirection(angle);
        }
    });

    function changeWind(angle) {
        hub.server.changeWind(angle);
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
                var $this = $(this),
                x = this.offsetLeft / (body.clientWidth - $this.width()),
                y = this.offsetTop / (body.clientHeight - $this.height());
                hub.server.movePlayer(x, y);
            }
        });
        $playerDiv.css('-ms-touch-action', 'none');
    }

    function moveShape(id, x, y) {
        var $shape = $("#"+id);
        $shape.css({
            left: (body.clientWidth - $shape.width()) * x,
            top: (body.clientHeight - $shape.height()) * y
        });
        //console.log("id: "+ id +" -> x: " + x + " y: " + y);
    }

    game.start = function(done) {
        $.connection.hub.start().done(done || {});
    };
    game.changeWind = changeWind;

}(window.game = window.game || {}, starfield, jQuery));