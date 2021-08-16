using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PAD.Data
{
    public class ColorService : IColorService
    {
        public string BASE_IMAGE_URL { get; }
        private readonly IConfiguration _config;
        public ColorService(IConfiguration configuration)
        {
            _config = configuration;
            BASE_IMAGE_URL = _config?.GetValue("BASE_IMAGE_URL", "whabalubadubdub!!!");
        }

        public ColorService()
        {
        }

        /// <summary>
        /// Converts hex string to a color object.
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns>Color equivalent.</returns
        public async Task<Color> ConvertHexStrToColor(string hexValue)
        {
            if (string.IsNullOrEmpty(hexValue)) return Color.White;
            if (hexValue.Count(c => c == '#') > 1) return Color.White;

            var color = (Color)new ColorConverter().ConvertFromString(hexValue);
            return color;
        }

        /// <summary>
        /// Converts the hexValue as string to the rgb string equivalent.
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns>String</returns>
        public async Task<string> ConvertHexToRGB(string hexValue)
        {
            if (string.IsNullOrEmpty(hexValue)) return null;
            if (hexValue.Count(c => c == '#') > 1) return null;

            if (hexValue.Contains("#")) hexValue = hexValue.Replace("#", ""); //Remove # if present

            int red = 0, green = 0, blue = 0;

            if (hexValue.Length == 6)
            {
                //#RRGGBB
                red = int.Parse(hexValue.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexValue.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexValue.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }

            if (hexValue.Length == 3)
            {
                //#RGB
                red = int.Parse(hexValue[0].ToString() + hexValue[0].ToString(), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexValue[1].ToString() + hexValue[1].ToString(), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexValue[2].ToString() + hexValue[2].ToString(), NumberStyles.AllowHexSpecifier);
            }

            return $"rgb({red}, {green}, {blue})";
        }

        /// <summary>
        /// Converts rgb string to a color object.
        /// </summary>
        /// <param name="rgbValuesStr"></param>
        /// <returns>Color equivalent.</returns>
        public async Task<Color> ConvertRBG_StrToColor(string rgbValuesStr)
        {
            var rgbValues = await GetRGB_ValuesFromRGB_Str(rgbValuesStr);
            if (rgbValues == null) return Color.White; // default is white

            return Color.FromArgb(rgbValues[0], rgbValues[1], rgbValues[2]);
        }

        /// <summary>
        /// Converts the rbg values [rgb(nnn, nnn, nnn)] into a hexcode [#nnnnn].
        /// Expected input format: 'rgb(nnn, nnn, nnn)'.
        /// </summary>
        /// <param name="rgbValuesStr"></param>
        /// <returns>Hex equivalent as string.</returns>
        public async Task<string> ConvertRGB_ToHex(string rgbValuesStr)
        {
            var rgbValues = await GetRGB_ValuesFromRGB_Str(rgbValuesStr);
            if (rgbValues == null) return null;

            var color = Color.FromArgb(rgbValues[0], rgbValues[1], rgbValues[2]);
            var hex = $"{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";
            return hex;
        }

        /// <summary>
        /// Returns rgb values parsed from passed rgb string as an int[]. 
        /// </summary>
        /// <param name="rgbValuesStr"></param>
        /// <returns>int[3] - R,G,B</returns>
        private async Task<int[]> GetRGB_ValuesFromRGB_Str(string rgbValuesStr)
        {
            if (string.IsNullOrEmpty(rgbValuesStr)) return null;
            if (!string.Join(string.Empty, rgbValuesStr.ToLower().Take(4)).Equals("rgb(", StringComparison.OrdinalIgnoreCase)) return null;
            if (rgbValuesStr.TakeLast(1).ToArray()[0] != ')') return null;
            if (rgbValuesStr.Count(c => c == ',') != 2) return null;

            var rgbValues = rgbValuesStr.ToLower().Replace("rgb(", string.Empty).Replace(")", string.Empty).Trim().Split(',').Select(val => int.Parse(val)).ToArray();
            return rgbValues;
        }
    }
}
