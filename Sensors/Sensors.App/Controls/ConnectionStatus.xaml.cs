using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Microsoft.AspNet.SignalR.Client;
using Sensors.App.Common;
using Sensors.App.Signalr;

namespace Sensors.App.Controls {
    public sealed partial class ConnectionStatus : UserControl {
        public ConnectionStatus() {
            InitializeComponent();
            Loaded += OnLoad;
            Unloaded += UnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e) {
            GameClient.Current.StateChanged += ConnectionStateChanged;
        }

        private void UnLoad(object sender, RoutedEventArgs e) {
            GameClient.Current.StateChanged -= ConnectionStateChanged;
        }

        private void ConnectionStateChanged(ConnectionState state) {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                switch (state) {
                    case ConnectionState.Connecting:
                        Status.Fill = Colors.Yellow.ToBrush();
                        break;
                    case ConnectionState.Connected:
                        Status.Fill = Colors.Green.ToBrush();
                        break;
                    case ConnectionState.Reconnecting:
                        Status.Fill = Colors.Yellow.ToBrush();
                        break;
                    case ConnectionState.Disconnected:
                        Status.Fill = Colors.Red.ToBrush();
                        break;
                }
            });
        }
    }
}
