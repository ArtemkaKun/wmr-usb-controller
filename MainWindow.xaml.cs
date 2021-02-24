using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Management.Automation;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WMR_USB_Controller.YUART.Autostart;
using WMR_USB_Controller.YUART.Holographic.Sleep_Mode;
using WMR_USB_Controller.YUART.Holographic.VirtualScreens;
using WMR_USB_Controller.YUART.Tray_Icon;
using WMR_USB_Controller.YUART.USB;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace WMR_USB_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string DisableWMRToggleText = "Disable WMR device";
        private const string EnableWMRToggleText = "Enable WMR device";
        private const double WindowToScreenSizeRatio = 4;
        
        private readonly UsbDevicesManager _usbDevicesManager = new UsbDevicesManager();
        private readonly PowerShell ps = PowerShell.Create();
        
        private AutostartManager _autostartManager;
        private TrayIconManager _trayIconManager;
        private SleepModeManager _sleepModeManager;
        private VirtualScreensManager _virtualScreensManager;

        public MainWindow()
        {
            InitializeComponent();

            SetProgramWindowSizeWithRatio();
            
            SetupTrayIconManager();
            SetupAutostartManager();
            SetupSleepModeManager();
            SetupVirtualScreensManager();
            
            _usbDevicesManager.Initialize();

            DisableWmrDeviceOnStartup();
        }
        
        private void SetProgramWindowSizeWithRatio()
        {
            var primaryScreenBounds = Screen.PrimaryScreen.Bounds;

            var heightScreenSizeWithRatio = primaryScreenBounds.Height / WindowToScreenSizeRatio;
            
            Width = heightScreenSizeWithRatio;
            
            Height = heightScreenSizeWithRatio;
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

        private void SetupSleepModeManager()
        {
            _sleepModeManager = new SleepModeManager(SleepDelayValue, ScreensaverModeStatus, ScreensaverModeStatusCheckbox);
            _sleepModeManager.Initialize();
        }

        private void SetupVirtualScreensManager()
        {
            _virtualScreensManager = new VirtualScreensManager(VirtualScreensCheckbox);
            _virtualScreensManager.Initialize();
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
            WmrStatusToggle.Content = newState ? DisableWMRToggleText : EnableWMRToggleText;
            WmrStatusToggle.IsChecked = !newState;
        }

        private void StartWMR(object sender, RoutedEventArgs e) {
            ps.Commands.Clear();
            EnableWmrDeviceAction(null, null);
            ps.AddScript(File.ReadAllText(@"resolution_script.ps1")).Invoke();
        }

        private void SwitchAutostartStatus(object sender, RoutedEventArgs e)
        {
            _autostartManager.SetToAutostart();
        }
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TrySetNewSleepDelayValue(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            
            _sleepModeManager.SetNewSleepDelay(Int32.Parse(SleepDelayInputField.Text));
            
            SleepDelayInputField.Text = String.Empty;
        }

        private void EnableScreensaverMode(object sender, RoutedEventArgs e)
        {
            _sleepModeManager.SetScreensaverModeStatus(true);
        }
        
        private void DisableScreensaverMode(object sender, RoutedEventArgs e)
        {
            _sleepModeManager.SetScreensaverModeStatus(false);
        }

        private void ResetSleepModeValues(object sender, RoutedEventArgs e)
        {
            _sleepModeManager.ResetSleepModeValues();
        }

        private void EnableVirtualScreens(object sender, RoutedEventArgs e)
        {
            _virtualScreensManager.SetVirtualScreensStatus(true);
        }
        
        private void DisableVirtualScreens(object sender, RoutedEventArgs e)
        {
            _virtualScreensManager.SetVirtualScreensStatus(false);
        }
    }
}
