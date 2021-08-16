using System.Threading.Tasks;

namespace PAD.Data
{
    public interface IWebpConverter
    {
        public string BASE_IMAGE_URL { get; }

        public Task<bool> ImgFileCheck(string imgFileName);
        public Task<string> FileTypeFinder(string imgFileName);
        public Task<byte[]> WebpConvert(byte[] inputImgFileByteArray);
    }
}
