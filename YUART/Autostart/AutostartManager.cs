using System;
using System.Windows.Controls;
using IWshRuntimeLibrary;
using Application = System.Windows.Application;
using File = System.IO.File;

namespace WMR_USB_Controller.YUART.Autostart
{
    /// <summary>
    /// Class, that controls app's autostart.
    /// </summary>
    public sealed class AutostartManager
    {
        private const string PathToTrayIcon = "WMR USB Controller Main Icon.ico";
        
        private readonly WshShell _wshShell = new WshShell();
        private readonly string _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private readonly CheckBox _autostartCheckbox;

        private string _appName;
        private string _autostartShortcutPath;
        private string _iconLocation;

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

            SetAutostartShortcutPath();
            
            SetIconLocation();
            
            SetAutostartCheckboxValue();
        }

        private void SetApplicationName()
        {
            _appName = Application.Current.MainWindow?.Title;
        }

        private void SetAutostartShortcutPath()
        {
            _autostartShortcutPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Startup)}\{_appName}.lnk";
        }

        private void SetIconLocation()
        {
            _iconLocation = $"{_workingDirectory}{PathToTrayIcon}";
        }

        private void SetAutostartCheckboxValue()
        {
            if (_autostartCheckbox.IsChecked == null) return;

            _autostartCheckbox.IsChecked = File.Exists(_autostartShortcutPath);
        }

        /// <summary>
        /// Sets/removes app from autostart (depends from the value of autostart checkbox).
        /// </summary>
        public void SetToAutostart()
        {
            if (_autostartCheckbox.IsChecked == null) return;

            if (_autostartCheckbox.IsChecked.Value)
            {
                CreateNewAutostartShortcut();
            }
            else
            {
                File.Delete(_autostartShortcutPath);
            }
        }

        private void CreateNewAutostartShortcut()
        {
            var shortcut = (IWshShortcut) _wshShell.CreateShortcut(_autostartShortcutPath);
            shortcut.Description = _appName;
            shortcut.WorkingDirectory = _workingDirectory;
            shortcut.TargetPath = _assemblyLocation;
            shortcut.IconLocation = _iconLocation;
            shortcut.Save();
        }
    }
}