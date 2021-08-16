using System.Drawing;
using System.Threading.Tasks;

namespace PAD.Data
{
    public interface IColorService
    {
        public string BASE_IMAGE_URL { get; }
        public Task<string> ConvertRGB_ToHex(string rgbValuesStr);
        public Task<string> ConvertHexToRGB(string hexValue);
        public Task<Color> ConvertRBG_StrToColor(string rgbValuesStr);
        public Task<Color> ConvertHexStrToColor(string hexValue);
    }
}
