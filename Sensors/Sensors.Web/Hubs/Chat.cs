﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Sensors.Web.Hubs {
    public class ChatHub : Hub {
        public void Send(string name, string message) {
            Clients.All.broadcastMessage(name, message);
        }
    }
}