using System.Collections.Generic;

namespace Sensors.Web.Hubs {
    public class Turn {
        public Player Boss { get; set; }
        public int Speed { get; set; }
        public List<Player> Players { get; set; } 
    }
}