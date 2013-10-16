/// <reference path="jquery.signalR-1.1.3.js" />
/// <reference path="jquery-1.6.4.js" />
$(function () {
    var hub = $.connection.gameHub,
        $boss = $("#boss"),
        $clientCount = $("#clientCount"),
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
                top: (body.clientHeight - $shape.height()) * y
            });
            //console.log("Moved: " + $shape.attr('id') + " left: " + x + " right: " + y);
        },
        clientCountChanged: function(count) {
            $clientCount.text(count);
        },
        windChanged: function(angle) {
            if (angle < 0) {
                starsCenter();
            }
            else if (angle > 315 && angle <= 45) {
                startsRight();
            }
            else if (angle > 45 && angle <= 135) {
                startsUp();
            }
            else if (angle > 135 && angle <= 225) {
                startsLeft();
            }
            else if (angle > 225 && angle <= 315) {
                startsDown();
            }
        }
    });

    $.connection.hub.start().done(function () {
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
});