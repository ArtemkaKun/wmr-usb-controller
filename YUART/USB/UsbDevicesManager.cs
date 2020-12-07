using System.Diagnostics;
using System.Management;
using System.Linq;

namespace WMR_USB_Controller.YUART.USB
{
    /// <summary>
    /// Class, that handles data about connected USB devices.
    /// </summary>
    public sealed class UsbDevicesManager
    {
        private const string UsbSeekerQueryString = @"SELECT * FROM Win32_PnPEntity where DeviceID Like ""USB%""";
        private const string UsbDescriptionParameterName = "Name";
        private const string PathToDevManViewTool = @"DevManView.exe";
        private const string EnableWmrCommand = "/enable";
        private const string DisableWmrCommand = "/disable";

        private readonly string[] _wmrNames = {
            "Windows Mixed Reality",
            "WMR"
        };

        private Process _wmrOnProcess;
        private Process _wmrOffProcess;
        private string _wmrDeviceName;

        /// <summary>
        /// Initialize UsbDevicesManager class.
        /// </summary>
        public void Initialize()
        {
            GetWmrDevice();
            PrepareControlProcesses();
        }

        private void GetWmrDevice()
        {
            using var usbSearcher = new ManagementObjectSearcher(UsbSeekerQueryString);

            using var collectionUsbObjects = usbSearcher.Get();

            foreach (var deviceObject in collectionUsbObjects)
            {
                var deviceDescription = (string)deviceObject.GetPropertyValue(UsbDescriptionParameterName);

                if (!_wmrNames.Any(deviceDescription.Contains)) continue;

                _wmrDeviceName = deviceDescription;

                break;
            }
        }

        private void PrepareControlProcesses()
        {
            _wmrOnProcess = new Process
            {
                StartInfo =
                {
                    FileName = PathToDevManViewTool,
                    Arguments = $"{EnableWmrCommand} \"{_wmrDeviceName}\""
                }
            };

            _wmrOffProcess = new Process
            {
                StartInfo =
                {
                    FileName = PathToDevManViewTool,
                    Arguments = $"{DisableWmrCommand} \"{_wmrDeviceName}\""
                }
            };
        }

        /// <summary>
        /// Activate or disable WMR device.
        /// </summary>
        /// <param name="newStatus">Activate WMR device?</param>
        public void ActivateWmrDevice(bool newStatus)
        {
            var action = newStatus ? _wmrOnProcess : _wmrOffProcess;

            action.Start();
            action.WaitForExit();
        }
    }
}