using System;
using System.Management;
using System.Windows;

namespace WMR_USB_Controller.YUART.USB
{
    /// <summary>
    /// Class, that handles data about connected USB devices.
    /// </summary>
    public sealed class UsbDevicesManager
    {
        private const string UsbSeekerQueryString = @"SELECT * FROM Win32_PnPEntity WHERE PNPClass LIKE 'Holographic'";
        private const string PnpRootPath = "root\\CIMV2";
        private const string CantFoundWmrDeviceMessage = "Can't find WMR device!";

        private ManagementObject _wmrDevice;

        /// <summary>
        /// Initialize UsbDevicesManager class.
        /// </summary>
        public void Initialize()
        {
            GetWmrDevice();
        }

        private void GetWmrDevice()
        {
            using var usbSearcher = new ManagementObjectSearcher(PnpRootPath, UsbSeekerQueryString);

            using var collectionUsbObjects = usbSearcher.Get();

            if (collectionUsbObjects.Count == 0)
            {
                MessageBox.Show(CantFoundWmrDeviceMessage);
                return;
            }

            GetFirstFoundedDevice(collectionUsbObjects);
        }

        private void GetFirstFoundedDevice(ManagementObjectCollection collectionUsbObjects)
        {
            foreach (ManagementObject device in collectionUsbObjects)
            {
                _wmrDevice = device;
                break;
            }
        }

        /// <summary>
        /// Activate or disable WMR device.
        /// </summary>
        /// <param name="newStatus">Activate WMR device?</param>
        public void ActivateWmrDevice(bool newStatus)
        {
            if (newStatus)
            {
                EnableWmrDevice();
            }
            else
            {
                DisableWmrDevice();
            }
        }

        private void EnableWmrDevice()
        {
            _wmrDevice.InvokeMethod("Enable", null, null);
        }

        private void DisableWmrDevice()
        {
            _wmrDevice.InvokeMethod("Disable", null, null);
        }
    }
}