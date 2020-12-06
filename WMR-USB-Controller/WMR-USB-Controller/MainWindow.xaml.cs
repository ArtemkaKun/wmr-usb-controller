using System;
using System.Windows;
using Microsoft.Win32;
using WMR_USB_Controller.YUART.Tray_Icon;
using WMR_USB_Controller.YUART.USB;

namespace WMR_USB_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string PathToAutostartRegKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        
        private readonly RegistryKey _autostartRegKey = Registry.CurrentUser.OpenSubKey(PathToAutostartRegKey, true);
        private readonly UsbDevicesManager _usbDevicesManager = new UsbDevicesManager();
        private readonly string _appExecutionPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        private readonly string _appName;
        private TrayIconManager _trayIconManager;
        
        public MainWindow()
        {
            InitializeComponent();

            _appName = Application.Current.MainWindow?.Title;

            SetAutostartCheckboxValue();
            
            SetupTrayIconManager();

            _usbDevicesManager.Initialize();
        }

        private void SetupTrayIconManager()
        {
            _trayIconManager = new TrayIconManager(this);
            _trayIconManager.Initialize();
        }

        private void SetAutostartCheckboxValue()
        {
            if (AutostartCheckbox.IsChecked == null) return;

            var startupAutostartValue = _autostartRegKey.GetValue(_appName);
            
            AutostartCheckbox.IsChecked = startupAutostartValue != null;
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

        private void SwitchAutostartStatus(object sender, RoutedEventArgs e)
        {
            if (AutostartCheckbox.IsChecked == null || _autostartRegKey == null) return;
            
            SetToAutostart(AutostartCheckbox.IsChecked.Value);
        }

        private void SetToAutostart(bool autostartStatus)
        {
            if (autostartStatus)
            {
                _autostartRegKey.SetValue(_appName, _appExecutionPath);
            }
            else
            {
                _autostartRegKey.DeleteValue(_appName, false);
            }
        }
    }
}