using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls;
using Microsoft.AspNet.SignalR.Client;

namespace Sensors.App.Signalr {
    public class GameClient {
        private IHubProxy _hub;
        private static GameClient _current;

        public event Action<ConnectionState> StateChanged;
        public event Action<Exception> OnError;

        public ConnectionState ConnectionState { get; set; }

        public bool IsConnected() {
            return ConnectionState == ConnectionState.Connected;
        }

        protected virtual void OnOnError(Exception obj) {
            Action<Exception> handler = OnError;
            if (handler != null) {
                handler(obj);
            }
        }

        protected virtual void OnStateChange(ConnectionState obj) {
            ConnectionState = obj;
            Action<ConnectionState> handler = StateChanged;
            if (handler != null) {
                handler(obj);
            }
        }

        public static GameClient Current {
            get {
                if (_current == null) {
                    _current = new GameClient();
                }
                return _current;
            }
        }

        private GameClient() {
            ConnectionState = ConnectionState.Disconnected;
        }

        public HubConnection HubConnection { get; private set; }

        public async Task ConnectAsync(string url) {
            if (HubConnection != null) {
                HubConnection.Dispose();
            }
            HubConnection = new HubConnection(url);
            _hub = HubConnection.CreateHubProxy("GameHub");
            HubConnection.StateChanged += args => OnStateChange(args.NewState);
            await HubConnection.Start().ContinueWith(x => {
                
            });
        }

        public async Task ChangeWind(Direction direction) {
            await _hub.Invoke("changeWind", (int) direction);
        }

        public async Task ChangeIsDay(bool isDay) {
            await _hub.Invoke("ChangeIsDay", isDay);
        }
        public async Task ChangeCompass(double angle) {
            await _hub.Invoke("ChangeCompass", angle);
        }

        public async Task ChangeBossSpeed(double speed) {
            await _hub.Invoke("ChangeBossSpeed", speed);
        }
    }
}
