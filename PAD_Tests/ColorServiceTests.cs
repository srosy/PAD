using PAD.Data;
using Shouldly;
using System.Drawing;
using System.Threading.Tasks;
using Xunit;

namespace PAD_Tests
{
    public class ColorServiceTests
    {
        [Fact]
        public async Task ConvertHexStrToColor_SUCCESS_ValidInput()
        {
            // assemble
            var hex = "#FFFF0000";
            var expectedColor = Color.Red;

            // act
            var color = await new ColorService().ConvertHexStrToColor(hex);

            // assert
            color.ShouldBeOfType<Color>();
            color.ShouldBe(expectedColor);
        }

        [Fact]
        public async Task ConvertHexStrToColor_FAIL_InvalidInput()
        {
            // assemble
            var hex1 = "##FFFF0000";
            var hex2 = string.Empty;
            var expectedColor = Color.White;

            // act
            var color1 = await new ColorService().ConvertHexStrToColor(hex1);
            var color2 = await new ColorService().ConvertHexStrToColor(hex2);

            // assert
            color1.ShouldBeOfType<Color>();
            color2.ShouldBeOfType<Color>();
            color1.ShouldBe(expectedColor);
            color2.ShouldBe(expectedColor);
        }

        [Fact]
        public async Task ConvertRBG_StrToColor_SUCCESS_ValidInput()
        {
            // assemble
            var rgb = "rgb(128, 15, 15)".ToLower();
            var expectedColor = Color.FromArgb(128, 15, 15);

            // act
            var color = await new ColorService().ConvertRBG_StrToColor(rgb);

            // assert
            color.ShouldBeOfType<Color>();
            color.R.ShouldBeEquivalentTo(expectedColor.R);
            color.G.ShouldBeEquivalentTo(expectedColor.G);
            color.B.ShouldBeEquivalentTo(expectedColor.B);
            color.ShouldBeEquivalentTo(expectedColor);
        }

        [Fact]
        public async Task ConvertRBG_StrToColor_FAIL_InvalidInput()
        {
            // assemble
            var rgb1 = "rgb(128, 15, 15)]".ToLower();
            var rgb2 = string.Empty;
            var expectedColor = Color.White;

            // act
            var color1 = await new ColorService().ConvertRBG_StrToColor(rgb1);
            var color2 = await new ColorService().ConvertRBG_StrToColor(rgb2);

            // assert
            color1.ShouldBeOfType<Color>();
            color2.ShouldBeOfType<Color>();
            color1.R.ShouldBeEquivalentTo(expectedColor.R);
            color2.R.ShouldBeEquivalentTo(expectedColor.R);
            color1.G.ShouldBeEquivalentTo(expectedColor.G);
            color2.G.ShouldBeEquivalentTo(expectedColor.G);
            color1.B.ShouldBeEquivalentTo(expectedColor.B);
            color2.B.ShouldBeEquivalentTo(expectedColor.B);
            color1.ShouldBeEquivalentTo(expectedColor);
            color2.ShouldBeEquivalentTo(expectedColor);
        }

        [Fact]
        public async Task HexToRGB_SUCCESS_ValidInput()
        {
            // assemble
            var hex = "#800f0f";
            var expectedRGB = "rgb(128, 15, 15)".ToLower();

            // act
            var rgb = await new ColorService().ConvertHexToRGB(hex);

            // assert
            rgb.ShouldNotBeNullOrEmpty();
            rgb.ToLower().ShouldBeEquivalentTo(expectedRGB);
        }

        [Fact]
        public async Task HexToRGB_FAIL_InvalidInput()
        {
            // assemble
            var cs = new ColorService();
            var hex1 = "##FACND";
            var hex2 = string.Empty;

            // act
            var rgb1 = await cs.ConvertHexToRGB(hex1);
            var rgb2 = await cs.ConvertHexToRGB(hex2);

            // assert
            rgb1.ShouldBeNull();
            rgb2.ShouldBeNull();
        }

        [Fact]
        public async Task RGB_ToHex_SUCCESS_ValidInput()
        {
            // assemble
            var rgb = "rgb(128, 15, 15)";
            var expectedHex = "#800f0f".ToUpper();

            // act
            var hex = $"#{await new ColorService().ConvertRGB_ToHex(rgb)}".ToUpper();

            // assert
            hex.ShouldNotBeNullOrEmpty();
            hex.ShouldBeEquivalentTo(expectedHex);
        }

        [Fact]
        public async Task RGB_ToHex_FAIL_InvalidInput()
        {
            // assemble
            var cs = new ColorService();
            var rgb1 = "rgb1(128, 15, 15)";
            var rgb2 = "rgb(,128, 15, 15)";
            var rgb3 = "rgb(128, 15, 15]";
            var rgb4 = string.Empty;

            // act
            var hex1 = await cs.ConvertRGB_ToHex(rgb1);
            var hex2 = await cs.ConvertRGB_ToHex(rgb2);
            var hex3 = await cs.ConvertRGB_ToHex(rgb3);
            var hex4 = await cs.ConvertRGB_ToHex(rgb4);

            // assert
            hex1.ShouldBeNull();
            hex2.ShouldBeNull();
            hex3.ShouldBeNull();
            hex4.ShouldBeNull();
        }
    }
}