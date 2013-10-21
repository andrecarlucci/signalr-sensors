using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Sensors.App.Signalr;

namespace Sensors.App.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Multitouch : Page {
        public Multitouch() {
            this.InitializeComponent();
        }

        private int _fingers;

        private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e) {
            _fingers++;
            FingersChanged();
        }

        private void UIElement_OnPointerReleased(object sender, PointerRoutedEventArgs e) {
            _fingers--;
            FingersChanged();
        }

        private async void FingersChanged() {
            Number.Text = _fingers.ToString();
            if (GameClient.Current.IsConnected()) {
                await GameClient.Current.ChangeBossSpeed(0.02 * (_fingers + 1));
            }
        }

        private void BackClicked(object sender, RoutedEventArgs e) {
            Frame.GoBack();
            _fingers = 0;
        }
    }
}
