using System;
using System.Windows.Controls;
using Microsoft.Win32;
using WMR_USB_Controller.YUART.Utilities;

namespace WMR_USB_Controller.YUART.Sleep_Mode
{
    /// <summary>
    /// Class, that provide controls for sleep mode of WMR.
    /// </summary>
    public sealed class SleepModeManager
    {
        private const string PathToHololensRegKeys = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Holographic";
        private const string SleepDelayRegkeyName = "IdleTimerDuration";
        private const string ScreensaverModeRegkeyName = "ScreensaverModeEnabled";

        private readonly RegistryKey _hololensRegKey = Registry.CurrentUser.OpenSubKey(PathToHololensRegKeys, true);
        private readonly Label _sleepDelayValueLabel;
        private readonly Label _screensaverModeStatusLabel;

        public SleepModeManager(Label sleepDelayValueLabel, Label screensaverModeStatusLabel)
        {
            _sleepDelayValueLabel = sleepDelayValueLabel;
            _screensaverModeStatusLabel = screensaverModeStatusLabel;
        }

        /// <summary>
        /// Initialize class and set startup UI values
        /// </summary>
        public void Initialize()
        {
            SetCurrentSleepDelayValue();
            SetCurrentScreensaverModeStatus();
        }

        private void SetCurrentSleepDelayValue()
        {
            _sleepDelayValueLabel.Content = $"{(_hololensRegKey.IsExists(SleepDelayRegkeyName) ? TimeConverter.ConvertMillisecondsIntoMinutes((int)_hololensRegKey.GetValue(SleepDelayRegkeyName)) : 15).ToString()} minutes";
        }

        private void SetCurrentScreensaverModeStatus()
        {
            _screensaverModeStatusLabel.Content = _hololensRegKey.IsExists(SleepDelayRegkeyName) ? ConvertIntIntoBool((int) _hololensRegKey.GetValue(ScreensaverModeRegkeyName)) : false;
        }

        private bool ConvertIntIntoBool(int value)
        {
            return value == 1;
        }

        /// <summary>
        /// Set new value to regkey of sleep delay for WMR.
        /// </summary>
        /// <param name="newDelayInMinutes">New value of the sleep delay in minutes.</param>
        public void SetNewSleepDelay(int newDelayInMinutes)
        {
            _hololensRegKey.SetValue(SleepDelayRegkeyName, TimeConverter.ConvertMinutesIntoMilliseconds(newDelayInMinutes));
            
            SetCurrentSleepDelayValue();
        }
    }
}