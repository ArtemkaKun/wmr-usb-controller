using System.Collections.Generic;
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
        private const string UsbDeviceIdParameterName = "DeviceID";
        private const string UsbPnpDeviceIdParameterName = "PNPDeviceID";
        private const string UsbDescriptionParameterName = "Name";

        private readonly Stack<UsbDevice> _usbDevices = new Stack<UsbDevice>();
        private readonly string[] _wmrNames = {
            "Windows Mixed Reality"
        };

        /// <summary>
        /// Turns off WMR device.
        /// </summary>
        public void TurnOffWmrDevice()
        {
            var devices = GetUsbDevices();

            while (devices.Count > 0)
            {
                var device = devices.Pop();

                if (!_wmrNames.Any(device.Description.Contains)) continue;

                var devManViewProc = new Process
                {
                    StartInfo =
                    {
                        FileName = @"C:\DevManView.exe",
                        Arguments = $"/disable \"{device.Description}\""
                    }
                };
                
                devManViewProc.Start();
                devManViewProc.WaitForExit();
                
                //enable
                // devManViewProc.StartInfo.Arguments = "/enable \"<name of the device>\"";
                // devManViewProc.Start();
                // devManViewProc.WaitForExit();
                
                break;
            }
        }
        
        private Stack<UsbDevice> GetUsbDevices()
        {
            _usbDevices.Clear();
            
            using var usbSearcher = new ManagementObjectSearcher(UsbSeekerQueryString);
            
            using var collectionUsbObjects = usbSearcher.Get();

            foreach (var deviceObject in collectionUsbObjects)
            {
                _usbDevices.Push(new UsbDevice(
                    (string)deviceObject.GetPropertyValue(UsbDeviceIdParameterName),
                    (string)deviceObject.GetPropertyValue(UsbPnpDeviceIdParameterName),
                    (string)deviceObject.GetPropertyValue(UsbDescriptionParameterName)
                ));
            }
            
            return _usbDevices;
        }
    }
}