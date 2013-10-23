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
    public sealed partial class BussolaView : Page {

        private Compass _sensor;

        private bool IsOn { get; set; }
        public BussolaView() {
            IsOn = true;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_sensor == null) {
                CreateGyrometer();
            }
        }

        private async void CreateGyrometer() {
            _sensor = Compass.GetDefault();
            if (_sensor == null) {
                await new MessageDialog("Bússola não suportada!", "Desculpe").ShowAsync();
                return;
            }
            _sensor.ReportInterval = Config.SensorRefreshInterval < _sensor.MinimumReportInterval ? _sensor.MinimumReportInterval : Config.SensorRefreshInterval;
            _sensor.ReadingChanged += NewRead;
        }

        private async void NewRead(Compass sender, CompassReadingChangedEventArgs args) {
            var value = args.Reading.HeadingMagneticNorth;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                Tangle.Text = String.Format("{0,5:0.00}", value);

                NeedleRotation.RotationZ = value;
                
                if (GameClient.Current.IsConnected()) {
                    await GameClient.Current.ChangeCompass(value);
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
