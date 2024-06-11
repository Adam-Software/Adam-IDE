using System;

namespace AdamStudio.Core.Extensions
{
    public static class ColorExtensions
    {
        public static string HexToRbgColor(this string hexColorString)
        {
            if (string.IsNullOrEmpty(hexColorString))
            {
                return "";
            }

            byte R = Convert.ToByte(hexColorString.Substring(3, 2), 16);
            byte G = Convert.ToByte(hexColorString.Substring(5, 2), 16);
            byte B = Convert.ToByte(hexColorString.Substring(7, 2), 16);

            return $"rgb({R}, {G}, {B})";
        }

        //TODO To HUE COLOR
    }
}
