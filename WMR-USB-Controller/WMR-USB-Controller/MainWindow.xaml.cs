using System;
using System.Windows;
using WMR_USB_Controller.YUART.Tray_Icon;
using WMR_USB_Controller.YUART.USB;

namespace WMR_USB_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly UsbDevicesManager _usbDevicesManager = new UsbDevicesManager();
        private TrayIconManager _trayIconManager;
        
        public MainWindow()
        {
            InitializeComponent();

            SetupTrayIconManager();

            _usbDevicesManager.Initialize();
        }

        private void SetupTrayIconManager()
        {
            _trayIconManager = new TrayIconManager(this);
            _trayIconManager.Initialize();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                _trayIconManager.ShowIcon();
            }

            base.OnStateChanged(e);
        }
        
        private void DisableWmrDeviceAction(object sender, RoutedEventArgs e)
        {
            ChangeWmrDeviceState(false);
        }

        private void EnableWmrDeviceAction(object sender, RoutedEventArgs e)
        {
            ChangeWmrDeviceState(true);
        }

        public void ChangeWmrDeviceState(bool newState)
        {
            _usbDevicesManager.ActivateWmrDevice(newState);
        }
    }
}