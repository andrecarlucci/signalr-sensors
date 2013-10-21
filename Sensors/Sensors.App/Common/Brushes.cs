using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Sensors.App.Common {
    public static class Brushes {
        public static SolidColorBrush ToBrush(this Color color) {
            return new SolidColorBrush(color);
        }
    }
}
