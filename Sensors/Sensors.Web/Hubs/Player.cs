    using System;

namespace Sensors.Web.Hubs {
    public class Player {
        public string Id { get; set; }
        public double Px { get; set; }
        public double Py { get; set; }
        public double Speed { get; set; }

        public Player(string id) {
            Id = id;
            var random = new Random();
            Px = random.NextDouble();
            Py = random.NextDouble();
        }
    }
}