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

namespace Sensors.App.Views {
    public sealed partial class AcelerometroView : Page {

        private Accelerometer _sensor;
        private double _upperLimit = 0.5;
        private double _lowerLimit = -0.5;

        public AcelerometroView() {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_sensor == null) {
                CreateGyrometer();
            }
        }

        private void CreateGyrometer() {
            _sensor = Accelerometer.GetDefault();
            if (_sensor == null) {
                new MessageDialog("Acelerômetro não suportado!", "Desculpe").ShowAsync();
                return;
            }
            _sensor.ReportInterval = Config.SensorRefreshInterval; //_sensor.MinimumReportInterval > 16 ? _sensor.MinimumReportInterval : 16;
            _sensor.ReadingChanged += NewRead;
        }

        private async void NewRead(Accelerometer sender, AccelerometerReadingChangedEventArgs args) {
            var x = args.Reading.AccelerationX;
            var y = args.Reading.AccelerationY;
            var z = args.Reading.AccelerationZ;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                Tx.Text = String.Format("{0,5:0.00}", x);
                Ty.Text = String.Format("{0,5:0.00}", y);
                Tz.Text = String.Format("{0,5:0.00}", z);

                Direction direction;
                if (x > _upperLimit) {
                    direction = Direction.East;
                    Ty.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (x < _lowerLimit) {
                    direction = Direction.West;
                    Ty.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (z > _lowerLimit) {
                    direction = Direction.South;
                    Tz.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (y < _lowerLimit) {
                    direction = Direction.North;
                    Ty.Foreground = new SolidColorBrush(Colors.Green);
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
            }
            Frame.GoBack();
        }
    }
}
