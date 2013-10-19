using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

namespace Sensors.App {
    public sealed partial class GiroscopioView : Page {

        private Gyrometer _gyrometer;

        public GiroscopioView() {
            InitializeComponent();

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_gyrometer == null) {
                CreateGyrometer();
            }
            
        }

        private void CreateGyrometer() {
            _gyrometer = Gyrometer.GetDefault();
            if (_gyrometer == null) {
                new MessageDialog("Gyrometer não suportado!", "Desculpe").ShowAsync();
                return;
            }
            _gyrometer.ReportInterval = _gyrometer.MinimumReportInterval > 16 ? _gyrometer.MinimumReportInterval : 16;
            _gyrometer.ReadingChanged += NewRead;
        }

        private async void NewRead(Gyrometer sender, GyrometerReadingChangedEventArgs args) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                GyrometerReading reading = args.Reading;
                Tx.Text = String.Format("{0,5:0.00}", reading.AngularVelocityX);
                Ty.Text = String.Format("{0,5:0.00}", reading.AngularVelocityY);
                Tz.Text = String.Format("{0,5:0.00}", reading.AngularVelocityZ);
            });
        }
    }
}
