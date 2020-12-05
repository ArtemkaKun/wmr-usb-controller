namespace WMR_USB_Controller.YUART.USB
{
    /// <summary>
    /// Struct, that handles USB device data.
    /// </summary>
    public readonly struct UsbDevice
    {
        public string DeviceId => _deviceId;

        public string PnpDeviceId => _pnpDeviceId;

        public string Description => _description;
        
        private readonly string _deviceId;
        private readonly string _pnpDeviceId;
        private readonly string _description;

        public UsbDevice(string deviceId, string pnpDeviceId, string description)
        {
            _deviceId = deviceId;
            _pnpDeviceId = pnpDeviceId;
            _description = description;
        }
    }
}