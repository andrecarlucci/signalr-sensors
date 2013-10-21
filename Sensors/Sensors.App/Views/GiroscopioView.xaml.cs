using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using Windows.Devices.Sensors;
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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Sensors.App.Common;
using Sensors.App.Signalr;

namespace Sensors.App {
    public sealed partial class GiroscopioView : Page {

        private Gyrometer _sensor;
        private int _upperLimit = 20;
        private int _lowerLimit = -20;

        public GiroscopioView() {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_sensor == null) {
                CreateGyrometer();
            }
        }

        private void CreateGyrometer() {
            _sensor = Gyrometer.GetDefault();
            if (_sensor == null) {
                new MessageDialog("Gyrometer não suportado!", "Desculpe").ShowAsync();
                return;
            }
            _sensor.ReportInterval = Config.SensorRefreshInterval;
            _sensor.ReadingChanged += NewRead;
        }

        private async void NewRead(Gyrometer sender, GyrometerReadingChangedEventArgs args) {
            var x = args.Reading.AngularVelocityX;
            var y = args.Reading.AngularVelocityY;
            var z = args.Reading.AngularVelocityZ;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                Tx.Text = String.Format("{0,5:0.00}", x);
                Ty.Text = String.Format("{0,5:0.00}", y);
                Tz.Text = String.Format("{0,5:0.00}", z);

                Direction direction;
                if (x > _upperLimit) {
                    direction = Direction.South;
                    Tx.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (x < _lowerLimit) {
                    direction = Direction.North;
                    Tx.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (y > _upperLimit) {
                    direction = Direction.East;
                    Ty.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (y < _lowerLimit) {
                    direction = Direction.West;
                    Ty.Foreground = new SolidColorBrush(Colors.Green);
                }
                else {
                    Tx.Foreground = new SolidColorBrush(Colors.White);
                    Ty.Foreground = new SolidColorBrush(Colors.White);
                    return;
                }

                if (GameClient.Current.IsConnected()) {
                    await GameClient.Current.ChangeWind(direction);
                }
            });
        }

        private void BackClicked(object sender, RoutedEventArgs e) {
            if (_sensor != null) {
                _sensor.ReportInterval = 0;
                _sensor.ReadingChanged -= NewRead;
            }
            Frame.GoBack();
        }
    }
}
