using Microsoft.Win32;

namespace WMR_USB_Controller.YUART.Utilities
{
    /// <summary>
    /// Class, that provides method to check regkeys.
    /// </summary>
    public static class RegkeyChecker
    {
        public static bool IsExists(this RegistryKey regkey, string keyName)
        {
            return regkey.GetValue(keyName) != null;
        }
    }
}