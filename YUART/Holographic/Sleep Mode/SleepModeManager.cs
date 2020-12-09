using System.Windows.Controls;
using WMR_USB_Controller.YUART.Utilities;

namespace WMR_USB_Controller.YUART.Holographic.Sleep_Mode
{
    /// <summary>
    /// Class, that provide controls for sleep mode of WMR.
    /// </summary>
    public sealed class SleepModeManager : HolographicManager
    {
        private const string SleepDelayRegkeyName = "IdleTimerDuration";
        private const string ScreensaverModeRegkeyName = "ScreensaverModeEnabled";
        private const int DefaultSleepDelay = 15;
        
        private readonly Label _sleepDelayValueLabel;
        private readonly Label _screensaverModeStatusLabel;
        private readonly CheckBox _screensaverModeCheckbox;

        public SleepModeManager(Label sleepDelayValueLabel, Label screensaverModeStatusLabel, CheckBox screensaverModeCheckbox)
        {
            _sleepDelayValueLabel = sleepDelayValueLabel;
            _screensaverModeStatusLabel = screensaverModeStatusLabel;
            _screensaverModeCheckbox = screensaverModeCheckbox;
        }

        /// <summary>
        /// Initialize class and set startup UI values
        /// </summary>
        public void Initialize()
        {
            UpdateUiValues();
        }

        private void UpdateUiValues()
        {
            UpdateLabelsValues();
            
            SetScreensaverModeCheckboxValue();
        }
        
        private void UpdateLabelsValues()
        {
            SetCurrentSleepDelayValue();
            SetCurrentScreensaverModeStatus();
        }

        private void SetCurrentSleepDelayValue()
        {
            _sleepDelayValueLabel.Content = $"{(HololensRegKey.IsExists(SleepDelayRegkeyName) ? TimeConverter.ConvertMillisecondsIntoMinutes((int) HololensRegKey.GetValue(SleepDelayRegkeyName)) : DefaultSleepDelay).ToString()} minutes";
        }

        private void SetCurrentScreensaverModeStatus()
        {
            _screensaverModeStatusLabel.Content = GetCurrentScreensaverModeStatusFromRegistry();
        }

        private void SetScreensaverModeCheckboxValue()
        {
            if (_screensaverModeCheckbox.IsChecked == null) return;

            _screensaverModeCheckbox.IsChecked = GetCurrentScreensaverModeStatusFromRegistry();
        }

        private bool GetCurrentScreensaverModeStatusFromRegistry()
        {
            return HololensRegKey.IsExists(ScreensaverModeRegkeyName) && ((int) HololensRegKey.GetValue(ScreensaverModeRegkeyName)).ConvertIntToBool();
        }

        /// <summary>
        /// Set new value to regkey of sleep delay for WMR.
        /// </summary>
        /// <param name="newDelayInMinutes">New value of the sleep delay in minutes.</param>
        public void SetNewSleepDelay(int newDelayInMinutes)
        {
            HololensRegKey.SetValue(SleepDelayRegkeyName, TimeConverter.ConvertMinutesIntoMilliseconds(newDelayInMinutes));
            
            SetCurrentSleepDelayValue();
        }

        /// <summary>
        /// Set new value for screensaver regkey.
        /// </summary>
        /// <param name="newStatus">New status (on/off)</param>
        public void SetScreensaverModeStatus(bool newStatus)
        {
            HololensRegKey.SetValue(ScreensaverModeRegkeyName, newStatus.ConvertBoolToInt());
            
            SetCurrentScreensaverModeStatus();
        }

        /// <summary>
        /// Reset sleep delay value and screensaver mode value (this function will delete keys from the registry)
        /// </summary>
        public void ResetSleepModeValues()
        {
            HololensRegKey.DeleteValue(SleepDelayRegkeyName);
            HololensRegKey.DeleteValue(ScreensaverModeRegkeyName);

            UpdateUiValues();
        }
    }
}