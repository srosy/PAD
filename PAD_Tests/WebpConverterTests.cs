using Microsoft.Extensions.Configuration;
using Moq;
using PAD.Data;
using Shouldly;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PAD_Tests
{
    public class WebpConverterTests
    {
        private readonly IConfiguration _config;
        public WebpConverterTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
            {"AcceptableFileTypes", "jpg, jpeg, png, webp"}
            };

             _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

        }
        [Fact]
        public async Task ImgFileCheck_Success_VALIDINPUT()
        {
            // assemble
            var fileName = Path.GetFileName("Y:/PixelArt2/PAD/PAD_Tests/Images/charmander.webp");
            var expectedValue = true;

            // act
            var value = await new WebpConverter(_config).ImgFileCheck(fileName);

            // assert
            value.ShouldBeOfType<bool>();
            value.ShouldBe(expectedValue);
        }

        [Fact]
        public async Task ImgFileCheck_Success_INVALIDINPUT()
        {
            // assemble
            var fileName = Path.GetFileName("Y:/PixelArt2/PAD/PAD_Tests/TestFiles/blazor_input.txt");
            var expectedValue = false;

            // act
            var value = await new WebpConverter(_config).ImgFileCheck(fileName);

            // assert
            value.ShouldBeOfType<bool>();
            value.ShouldBe(expectedValue);
        }

        [Fact]
        public async Task FileTypeFinder_WEBP()
        {
            // assemble
            var fileName = Path.GetFileName("Y:/PixelArt2/PAD/PAD_Tests/Images/charmander.webp");
            var expectedValue = ".webp";

            // act
            var value = await new WebpConverter(_config).FileTypeFinder(fileName);

            // assert
            value.ShouldBeOfType<string>();
            value.ShouldBe(expectedValue);
        }

        [Fact]
        public async Task FileTypeFinder_PNG()
        {
            // assemble
            var fileName = Path.GetFileName("Y:/PixelArt2/PAD/PAD_Tests/Images/landscape.png");
            var expectedValue = ".png";

            // act
            var value = await new WebpConverter(_config).FileTypeFinder(fileName);

            // assert
            value.ShouldBeOfType<string>();
            value.ShouldBe(expectedValue);
        }

        [Fact]
        public async Task FileTypeFinder_INVALDINPUT()
        {
            // assemble
            var fileName = Path.GetFileName("Y:/PixelArt2/PAD/PAD_Tests/Images/blazor_input.txt");

            // act
            var value = await new WebpConverter(_config).FileTypeFinder(fileName);

            // assert
            value.ShouldBe(".txt");
        }

        [Fact]
        public async Task WebpConvert_SUCCESS_ValidInput()
        {
            // assemble
            var fileByteArray = File.ReadAllBytes("Y:/PixelArt2/PAD/PAD_Tests/Images/landscape.png");
            var expectedValue = false;
            var value = false;

            // act
            var newByteArray = await new WebpConverter(_config).WebpConvert(fileByteArray);
            if (newByteArray == fileByteArray)
            {
                value = true;
            }

            // assert
            value.ShouldBeOfType<bool>();
            value.ShouldBe(expectedValue);
        }


    }

}
