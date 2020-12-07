using System.Windows.Controls;
using Microsoft.Win32;
using Application = System.Windows.Application;

namespace WMR_USB_Controller.YUART.Autostart
{
    /// <summary>
    /// Class, that controls app's autostart.
    /// </summary>
    public sealed class AutostartManager
    {
        private const string PathToAutostartRegKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        private readonly RegistryKey _autostartRegKey = Registry.CurrentUser.OpenSubKey(PathToAutostartRegKey, true);
        private readonly string _appExecutionPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private readonly CheckBox _autostartCheckbox;

        private string _appName;

        public AutostartManager(CheckBox autostartCheckbox)
        {
            _autostartCheckbox = autostartCheckbox;
        }

        /// <summary>
        /// Initialize manager data.
        /// </summary>
        public void Initialize()
        {
            SetApplicationName();

            SetAutostartCheckboxValue();
        }

        private void SetApplicationName()
        {
            _appName = Application.Current.MainWindow?.Title;
        }

        private void SetAutostartCheckboxValue()
        {
            if (_autostartCheckbox.IsChecked == null) return;

            var startupAutostartValue = _autostartRegKey.GetValue(_appName);

            _autostartCheckbox.IsChecked = startupAutostartValue != null;
        }

        /// <summary>
        /// Sets/removes app from autostart (depends from the value of autostart checkbox).
        /// </summary>
        public void SetToAutostart()
        {
            if (_autostartCheckbox.IsChecked == null || _autostartRegKey == null) return;

            if (_autostartCheckbox.IsChecked.Value)
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