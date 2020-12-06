using System;
using System.Windows;
using WMR_USB_Controller.YUART.Autostart;
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

        private AutostartManager _autostartManager;
        private TrayIconManager _trayIconManager;
        
        public MainWindow()
        {
            InitializeComponent();

            SetupTrayIconManager();
            SetupAutostartManager();

            _usbDevicesManager.Initialize();

            DisableWmrDeviceOnStartup();
        }

        private void SetupTrayIconManager()
        {
            _trayIconManager = new TrayIconManager(this);
            _trayIconManager.Initialize();
        }

        private void SetupAutostartManager()
        {
            _autostartManager = new AutostartManager(AutostartCheckbox);
            _autostartManager.Initialize();
        }

        private void DisableWmrDeviceOnStartup()
        {
            ChangeWmrDeviceState(false);
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

        /// <summary>
        /// Switch WMR device status (active/disabled).
        /// </summary>
        /// <param name="newState">New active status of WMR device.</param>
        public void ChangeWmrDeviceState(bool newState)
        {
            _usbDevicesManager.ActivateWmrDevice(newState);
            WmrStatusToggle.IsChecked = !newState;
        }

        private void SwitchAutostartStatus(object sender, RoutedEventArgs e)
        {
            _autostartManager.SetToAutostart();
        }
    }
}