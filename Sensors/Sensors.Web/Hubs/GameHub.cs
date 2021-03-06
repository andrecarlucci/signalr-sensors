﻿using System.Diagnostics;
using System.Threading;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Sensors.Web.Hubs {
    public class GameHub : Hub {

        private Game _game;
        public GameHub() {
            _game = Game.Instance;
        }

        public void ChangeWind(double angle) {
            _game.WindAngle = angle;
            Clients.All.windChanged(angle);
        }

        public void ChangeIsDay(bool isDay) {
            _game.IsDay = isDay;
            Clients.All.isDayChanged(isDay);
        }

        public void ChangeCompass(double angle) {
            _game.CompassAngle = angle;
            Clients.All.compassChanged(angle);
        }

        public void ChangeBossSpeed(double speed) {
            _game.Boss.Speed = speed;
        }

        public void MovePlayer(double x, double y) {
            _game.MovePlayer(Context.ConnectionId, x, y);
            Clients.Others.shapeMoved(Context.ConnectionId, x, y);
        }

        public override Task OnConnected() {
            foreach (var p in _game.PlayersDic) {
                Clients.Caller.addOtherPlayer(p.Value);
            }

            var player = new Player(Context.ConnectionId);
            _game.AddPlayer(player);

            Clients.Others.addOtherPlayer(player);
            Clients.Caller.addPlayer(player);
            Clients.Caller.windChanged(_game.WindAngle);
            Clients.Caller.isDayChanged(_game.IsDay);
            Clients.Caller.clientDeathChanged(_game.DeathCount);
            Clients.Caller.compassChanged(_game.CompassAngle);

            return Clients.All.clientCountChanged(_game.PlayersDic.Count);
        }

        public override Task OnReconnected() {
            return Clients.All.clientCountChanged(_game.PlayersDic.Count);
        }

        public override Task OnDisconnected() {
            _game.RemovePlayer(Context.ConnectionId);
            Clients.All.removePlayer(Context.ConnectionId);
            return Clients.All.clientCountChanged(_game.PlayersDic.Count);
        }
    }
}