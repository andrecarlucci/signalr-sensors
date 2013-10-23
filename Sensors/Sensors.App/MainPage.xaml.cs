using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.AspNet.SignalR.Client;
using Sensors.App.Common;
using Sensors.App.Controls;
using Sensors.App.Signalr;
using Sensors.App.Views;

namespace Sensors.App {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : BasePage {

        public MainPage() {
            InitializeComponent();
        }

        private void AcelerometroClick(object sender, PointerRoutedEventArgs e) {
            Frame.Navigate(typeof(AcelerometroView));
        }

        private void GiroscopioClick(object sender, PointerRoutedEventArgs e) {
            Frame.Navigate(typeof(GiroscopioView));
        }

        private void SensorDeLuzClick(object sender, PointerRoutedEventArgs e) {
            Frame.Navigate(typeof(SensorDeLuzView));
        }

        private void BussolaClick(object sender, PointerRoutedEventArgs e) {
            Frame.Navigate(typeof(BussolaView));
        }

        private async void BConectar_Click(object sender, RoutedEventArgs e) {
            BConectar.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
            await GameClient.Current.ConnectAsync("http://" + tUrl.Text + "/signalr");
            ProgressBar.Visibility = Visibility.Collapsed;
            BConectar.IsEnabled = true;
        }

        private void MultitouchClick(object sender, PointerRoutedEventArgs e) {
            Frame.Navigate(typeof(Multitouch));
        }

        private void ShowAddress(object sender, PointerRoutedEventArgs e) {
            if (tUrl.Visibility == Visibility.Visible) {
                tUrl.Visibility = Visibility.Collapsed;
                return;
            }
            tUrl.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            ConnectionStateChanged(GameClient.Current.ConnectionState);
            GameClient.Current.StateChanged += ConnectionStateChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);
            GameClient.Current.StateChanged -= ConnectionStateChanged;
        }

        private async void ConnectionStateChanged(ConnectionState state) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
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
