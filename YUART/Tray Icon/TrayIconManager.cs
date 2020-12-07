using System;
using System.Windows;
using System.Windows.Forms;

namespace WMR_USB_Controller.YUART.Tray_Icon
{
    /// <summary>
    /// Class, that manages behaviour of the application in tray.
    /// </summary>
    public sealed class TrayIconManager
    {
        private const string PathToTrayIcon = "WMR USB Controller Main Icon.ico";
        private const string ToolTipText = "WMR USB Controller";
        private const string DisableWmrMenuItemText = "Disable WMR Device";
        private const string EnableWmrMenuItemText = "Enable WMR Device";

        private readonly NotifyIcon _trayIcon = new NotifyIcon
        {
            Icon = new System.Drawing.Icon(PathToTrayIcon),
            Visible = false,
            Text = ToolTipText
        };

        private readonly MainWindow _mainWindow;

        private ContextMenu _disableWmrMenu;
        private ContextMenu _enableWmrMenu;

        public TrayIconManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Initialize behaviour of tray icon.
        /// </summary>
        public void Initialize()
        {
            InitializeTrayOptions();

            _trayIcon.ContextMenu = _enableWmrMenu;

            _trayIcon.DoubleClick += ShowMainWindow;
        }

        private void InitializeTrayOptions()
        {
            _disableWmrMenu = new ContextMenu(new[]
            {
                new MenuItem(DisableWmrMenuItemText, DisableWmr)
            });

            _enableWmrMenu = new ContextMenu(new[]
            {
                new MenuItem(EnableWmrMenuItemText, ActivateWmr)
            });
        }

        private void DisableWmr(object _, EventArgs eventArgs)
        {
            _trayIcon.ContextMenu = _enableWmrMenu;
            _mainWindow.ChangeWmrDeviceState(false);
        }

        private void ActivateWmr(object _, EventArgs eventArgs)
        {
            _trayIcon.ContextMenu = _disableWmrMenu;
            _mainWindow.ChangeWmrDeviceState(true);
        }

        private void ShowMainWindow(object _, EventArgs eventArgs)
        {
            HideIcon();

            _mainWindow.Show();

            _mainWindow.WindowState = WindowState.Normal;
        }

        private void HideIcon()
        {
            _trayIcon.Visible = false;
        }

        /// <summary>
        /// Makes icon visible in the tray.
        /// </summary>
        public void ShowIcon()
        {
            _trayIcon.Visible = true;
        }
    }
}