using System.Text;
using System.Windows;
using WMR_USB_Controller.YUART.USB;

namespace WMR_USB_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly UsbDevicesManager _usbDevicesManager = new UsbDevicesManager();

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void HandleCheck(object sender, RoutedEventArgs e) { 
            _usbDevicesManager.TurnOffWmrDevice(); 
        }
    }
}