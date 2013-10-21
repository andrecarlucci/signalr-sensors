using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Sensors.App.Controls {
    public class BasePage : Page {

        public void ShowMessage(string message, string title = "") {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => new MessageDialog("Você precisa conectar no servidor primeiro", title).ShowAsync());
        }
    }
}
