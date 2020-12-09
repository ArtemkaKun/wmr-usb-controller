using Microsoft.Win32;

namespace WMR_USB_Controller.YUART.Holographic
{
    /// <summary>
    /// Base class, that provides base data and behaviour for holographic managers.
    /// </summary>
    public class HolographicManager
    {
        private const string PathToHololensRegKeys = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Holographic";
        protected readonly RegistryKey HololensRegKey = Registry.CurrentUser.OpenSubKey(PathToHololensRegKeys, true);
    }
}