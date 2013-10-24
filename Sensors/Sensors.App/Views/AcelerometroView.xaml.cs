// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using System;
using System.Linq; 
using Windows.Devices.Sensors;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Sensors.App.Common;
using Sensors.App.Signalr;
using Windows.Foundation.Diagnostics;

namespace Sensors.App.Views {
    public sealed partial class AcelerometroView : Page {

        private Accelerometer _sensor;
        private double _upperLimit = 0.8;
        private double _lowerLimit = -0.8;
        private static LoggingChannel Logger = new LoggingChannel("sensors");

        public AcelerometroView() {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_sensor == null) {
                CreateAcelerometro();
            }
        }

        private async void CreateAcelerometro() {
            _sensor = Accelerometer.GetDefault();
            if (_sensor == null) {
                await new MessageDialog("Acelerômetro não suportado!", "Desculpe").ShowAsync();
                return;
            }
            _sensor.ReportInterval = Config.SensorRefreshInterval < _sensor.MinimumReportInterval ? 
                                            _sensor.MinimumReportInterval : 
                                            Config.SensorRefreshInterval;
            _sensor.ReadingChanged += NewRead;
            Logger.LogMessage("Accelerometer Start", LoggingLevel.Information);
        }

        private async void NewRead(Accelerometer sender, AccelerometerReadingChangedEventArgs args) {
            var x = args.Reading.AccelerationX;
            var y = args.Reading.AccelerationY;
            var z = args.Reading.AccelerationZ;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                Tx.Text = String.Format("{0,5:0.00}", x);
                Ty.Text = String.Format("{0,5:0.00}", y);
                Tz.Text = String.Format("{0,5:0.00}", z);

                var green = new SolidColorBrush(Colors.Green);

                Direction direction;
                if (x > _upperLimit) {
                    direction = Direction.West;
                    Ty.Foreground = green;
                }
                else if (x < _lowerLimit) {
                    direction = Direction.East;
                    Ty.Foreground = green;
                }
                else if (z > _lowerLimit) {
                    direction = Direction.South;
                    Tz.Foreground = green;
                }
                else if (y < _lowerLimit) {
                    direction = Direction.North;
                    Ty.Foreground = green;
                }
                else {
                    Tx.Foreground = new SolidColorBrush(Colors.White);
                    Ty.Foreground = new SolidColorBrush(Colors.White);
                    Tz.Foreground = new SolidColorBrush(Colors.White);
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
                Logger.LogMessage("Accelerometer End", LoggingLevel.Information);
            }
            Frame.GoBack();
        }
    }
}
