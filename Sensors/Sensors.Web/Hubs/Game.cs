using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sensors.Web.Hubs {
    public class Game : Hub {

        private static readonly ConcurrentDictionary<string, object> _players =
           new ConcurrentDictionary<string, object>();

        public override Task OnConnected() {
            _players.TryAdd(Context.ConnectionId, null);
            return Clients.All.clientCountChanged(_players.Count);
        }


    }
}