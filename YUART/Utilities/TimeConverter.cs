using System;

namespace WMR_USB_Controller.YUART.Utilities
{
    /// <summary>
    /// Class, that handles methods for time conversion.
    /// </summary>
    public static class TimeConverter
    {
        private const double MillisecondsInMinute = 60000d;
    
        public static int ConvertMillisecondsIntoMinutes(int milliseconds)
        {
            return Convert.ToInt32(Math.Round(milliseconds / MillisecondsInMinute));
        }

        public static int ConvertMinutesIntoMilliseconds(int minutes)
        {
            return Convert.ToInt32(minutes * MillisecondsInMinute);
        }
    }
}