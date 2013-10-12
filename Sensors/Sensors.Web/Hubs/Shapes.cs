using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sensors.Web.Hubs {
    public class Shapes : Hub  {
        private static readonly ConcurrentDictionary<string, object> _connections =
           new ConcurrentDictionary<string, object>();

        public void MoveShape(double x, double y) {
            Clients.Others.shapeMoved(x, y);
        }

        public override Task OnConnected() {
            _connections.TryAdd(Context.ConnectionId, null);
            return Clients.All.clientCountChanged(_connections.Count);
        }

        public override Task OnReconnected() {
            _connections.TryAdd(Context.ConnectionId, null);
            return Clients.All.clientCountChanged(_connections.Count);
        }

        public override Task OnDisconnected() {
            object value;
            _connections.TryRemove(Context.ConnectionId, out value);
            return Clients.All.clientCountChanged(_connections.Count);
        }
    }
}