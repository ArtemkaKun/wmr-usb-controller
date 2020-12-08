namespace WMR_USB_Controller.YUART.Utilities
{
    /// <summary>
    /// Class, that provides tools for int-bool conversion.
    /// </summary>
    public static class IntBoolConverter
    {
        public static bool ConvertIntToBool(this int value)
        {
            return value == 1;
        }

        public static int ConvertBoolToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}