using System.Diagnostics;
using System.Threading;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sensors.Web.Hubs {
    public class Game {
        private static object _sync = new object();
        private static Game _game;
        public static Game Instance {
            get {
                lock(_sync) {
                    return _game ?? (_game = new Game());
                }
            }
        }

        public ConcurrentDictionary<string, Player> PlayersDic = new ConcurrentDictionary<string, Player>();
        public Player Boss { get; set; }

        private Timer _timer;

        private Game() {
            Boss = new Player("boss");
            Boss.Speed = 0.05;
            _timer = new Timer(Tick, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        private void Tick(object state) {
            var players = PlayersDic.Values.ToList();
            MoveBoss(players);
            var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            context.Clients.All.shapeMoved("boss", Boss.Px, Boss.Py);
            Debug.WriteLine("tick: x:" + Boss.Px +" y: "+ Boss.Py);
        }

        public void MoveBoss(List<Player> players) {
            if (players.Count == 0) return;
            var b = Boss;
            var target = players.OrderBy(p => (Math.Pow(b.Px - p.Px, 2) + Math.Pow(b.Py - p.Py, 2))).First();

            var y = target.Py - b.Py;
            var x = target.Px - b.Px;
            var z = Math.Sqrt(y*y + x*x);

            var yl = y*b.Speed/z;
            var xl = x*b.Speed/z;

            b.Px += xl;
            b.Py += yl;
        }

        public void AddPlayer(Player player) {
            PlayersDic.TryAdd(player.Id, player);
        }

        public void RemovePlayer(string id) {
            Player player;
            PlayersDic.TryRemove(id, out player);
        }

        public void MovePlayer(string id, double x, double y) {
            Player player;
            if (PlayersDic.TryGetValue(id, out player)) {
                player.Px = x;
                player.Py = y;
            }
        }
    }
}