using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PAD.Data
{
    public class WebpConverter : IWebpConverter
    {
        public string BASE_IMAGE_URL { get; }
        private readonly IConfiguration _config;

        public WebpConverter(IConfiguration configuration)
        {
            _config = configuration;
            BASE_IMAGE_URL = _config?.GetValue("BASE_IMAGE_URL", "whabalubadubdub!!!");
        }

        public WebpConverter() { }

        /// <summary>
        /// Accepts a byte array returns a new byte array in webp format. Be sure to use the ImgFileCheck() function to 
        /// verify that the file is in the correct img format before using this function. In order to get a byte array you can use the AzureStorage.ConvertFileToByteArray() function first
        /// </summary>
        /// <param name="inputImgFileByteArray"></param>
        /// <returns></returns>
        public async Task<byte[]> WebpConvert(byte[] inputImgFileByteArray)
        {
            if (inputImgFileByteArray == null) return null;

            using var inStream = new MemoryStream(inputImgFileByteArray);
            using var outStream = new MemoryStream();
            using (var imageFactory = new ImageFactory(preserveExifData: false))
            {
                imageFactory.Load(inStream)
                .Format(new WebPFormat())
                .Quality(50);

                imageFactory.Save(outStream);
            }
            var newByteArray = outStream.ToArray();
            return newByteArray;
        }

        /// <summary>
        /// This function will check to see if the file provided by the user is accepted by our system such as "png, jpg, jpeg, or webp" and then
        /// will return true if so or false if not. The function only needs the file name of the file
        /// </summary>
        /// <param name="imgFileName"></param>
        /// <returns></returns>
        public async Task<bool> ImgFileCheck(string imgFileName)
        {
            var acceptableFileTypes = _config.GetValue("AcceptableFileTypes", "")?.Replace(" ", string.Empty).ToLower().Split(",").Select(f => $".{f}").ToArray();
            for (int i = 0; i < acceptableFileTypes.Length; i++)
            {
                if (imgFileName.EndsWith(acceptableFileTypes[i])) return true;
            }

            return false;
        }


        /// <summary>
        /// This function returns the file type of the file as a string such as ".png" or ".webp" and returns null if its empty
        /// </summary>
        /// <param name="imgFileName"></param>
        /// <returns></returns>
        public async Task<string> FileTypeFinder(string imgFileName)
        {
            if (string.IsNullOrEmpty(imgFileName)) { return null; }
            return $".{imgFileName.ToLower().Split(".")[1]}";
        }
    }
}

