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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Sensors.App.Common;
using Sensors.App.Signalr;

namespace Sensors.App {
    public sealed partial class SensorDeLuzView : Page {

        private LightSensor _sensor;
        private int _limit = 20;

        private bool IsOn { get; set; }
        public SensorDeLuzView() {
            IsOn = true;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_sensor == null) {
                CreateGyrometer();
            }
        }

        private async void CreateGyrometer() {
            _sensor = LightSensor.GetDefault();
            if (_sensor == null) {
                await new MessageDialog("Sensor de luz não suportado!", "Desculpe").ShowAsync();
                return;
            }
            _sensor.ReportInterval = Config.SensorRefreshInterval < _sensor.MinimumReportInterval ? _sensor.MinimumReportInterval : Config.SensorRefreshInterval;
            _sensor.ReadingChanged += NewRead;
        }

        private async void NewRead(LightSensor sender, LightSensorReadingChangedEventArgs args) {
            var value = args.Reading.IlluminanceInLux;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                Ty.Text = String.Format("{0,5:0.00}", value);

                bool newIsOn = value > _limit;
                if(IsOn == newIsOn) return;
                
                IsOn = newIsOn;
                string image = IsOn ? "on" : "off";
                Light.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + image + ".png", UriKind.Absolute));

                if (GameClient.Current.IsConnected()) {
                    await GameClient.Current.ChangeIsDay(IsOn);
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
