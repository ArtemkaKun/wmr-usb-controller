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
        private const double MillisecondsInMinute = 60000d;
        
        private readonly RegistryKey _hololensRegKey = Registry.CurrentUser.OpenSubKey(PathToHololensRegKeys, true);
        private readonly Label _sleepDelayValueLabel;
        private readonly Label _screensaverModeStatusLabel;

        public SleepModeManager(Label sleepDelayValueLabel)
        {
            _sleepDelayValueLabel = sleepDelayValueLabel;
        }

        /// <summary>
        /// Initialize class and set startup UI values
        /// </summary>
        public void Initialize()
        {
            SetCurrentSleepDelayValue();
        }

        private void SetCurrentSleepDelayValue()
        {
            _sleepDelayValueLabel.Content = $"{(_hololensRegKey.IsExists(SleepDelayRegkeyName) ? ConvertMillisecondsIntoMinutes((int)_hololensRegKey.GetValue(SleepDelayRegkeyName)) : 15).ToString()} minutes";
        }

        private int ConvertMillisecondsIntoMinutes(int milliseconds)
        {
            return Convert.ToInt32(Math.Round(milliseconds / MillisecondsInMinute));
        }
        
        
    }
}