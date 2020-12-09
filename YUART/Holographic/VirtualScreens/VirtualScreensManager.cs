using System.Windows.Controls;
using WMR_USB_Controller.YUART.Utilities;

namespace WMR_USB_Controller.YUART.Holographic.VirtualScreens
{
    /// <summary>
    /// Class, that provide methods to manage Virtual Screens Allocation option.
    /// </summary>
    public sealed class VirtualScreensManager : HolographicManager
    {
        private const string VirtualScreensRegkey = "PreallocateVirtualMonitors";

        private readonly CheckBox _virtualScreensCheckbox;

        public VirtualScreensManager(CheckBox virtualScreensCheckbox)
        {
            _virtualScreensCheckbox = virtualScreensCheckbox;
        }
        
        /// <summary>
        /// Initialize class and set startup UI values
        /// </summary>
        public void Initialize()
        {
            SetVirtualMonitorsCheckboxValue();
        }

        private void SetVirtualMonitorsCheckboxValue()
        {
            if (_virtualScreensCheckbox.IsChecked == null) return;

            _virtualScreensCheckbox.IsChecked = !GetCurrentVirtualScreensStatusFromRegistry();
        }

        private bool GetCurrentVirtualScreensStatusFromRegistry()
        {
            return HololensRegKey.IsExists(VirtualScreensRegkey) && ((int) HololensRegKey.GetValue(VirtualScreensRegkey)).ConvertIntToBool();
        }

        /// <summary>
        /// Set new status for Virtual Screens option.
        /// </summary>
        /// <param name="newStatus">Turn on/off virtual screens</param>
        public void SetVirtualScreensStatus(bool newStatus)
        {
            HololensRegKey.SetValue(VirtualScreensRegkey, newStatus.ConvertBoolToInt());
        }
    }
}