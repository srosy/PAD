using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PAD_Tests
{
    public class ClientAPIControllerTests
    {

        private HttpClient _client;
        private string _validCredentials;

        public ClientAPIControllerTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://pixelartdesigner.azurewebsites.net/client/api/v1/");
            //encrypted credentials of JamesPleiadesHawkins@yahoo.com test account
            _validCredentials = "6C66790A5EA9AD591C2288228686DC85902D453616653F59EAD3506DCA3D5B6AF1D5BE8EA7B5A9A635E73C232457B4A423F4ACA0246F14760C2B84AFA46DD6B7CFF762A24D53D418CA8B8557D32B95270BE5431C536ED6187CB2CDB68B72ECB379955348A0B0A3D03650413B875009EB252ED98D5D5B7EAF611486188A03E25B";

        }

        [Fact]
        public async Task RequestToken_FAILURE_InvalidCredentials()
        {
            // assemble
            var url = "authenticate?credentials=thisIsNotAnEncryptedString";

            // act
            var result = await _client.GetAsync(url);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        [Fact]
        public async Task RequestToken_FAILURE_NullCredentials()
        {
            // assemble
            var url = "authenticate?";

            // act
            var result = await _client.GetAsync(url);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        [Fact]
        public async Task RequestToken_FAILURE_EmptyCredentials()
        {
            // assemble
            var url = "authenticate?credentials=      ";

            // act
            var result = await _client.GetAsync(url);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        [Fact]
        public async Task RequestToken_SUCCESS_ValidCredentials()
        {
            // assemble
            var url = "authenticate?credentials=" + _validCredentials;

            // act
            var result = await _client.GetAsync(url);

            //assert
            result.StatusCode.ToString().ShouldBe("OK");
        }

        [Fact]
        public async Task RequestRandomImage_FAILURE_InvalidToken()
        {
            // assemble
            var url = "image?token=NotARealToken";

            // act
            var result = await _client.GetAsync(url);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        [Fact]
        public async Task RequestRandomImage_FAILURE_NullToken()
        {
            // assemble
            var url = "image";

            // act
            var result = await _client.GetAsync(url);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        [Fact]
        public async Task RequestRandomImage_SUCCESS_ValidToken()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "image?token=";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token);

            //assert
            result.StatusCode.ToString().ShouldBe("OK");

        }

        [Fact]
        public async Task RequestAllImages_SUCCESS_ValidToken()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "images?token=";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token);

            //assert
            result.StatusCode.ToString().ShouldBe("OK");

        }

        [Fact]
        public async Task RequestAllUsersImages_SUCCESS_ValidTokenValidId()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "images?token=";
            var idUrl = "&&id=35";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token + idUrl);

            //assert
            result.StatusCode.ToString().ShouldBe("OK");

        }

        [Fact]
        public async Task RequestAllUsersImages_FAILURE_ValidTokenInvalidId()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "images?token=";
            var idUrl = "&&id=NotARealId";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token + idUrl);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        [Fact]
        public async Task RequestAllUsersImages_FAILURE_ValidTokenNoSuchUser()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "images?token=";
            var idUrl = "&&id=1";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token + idUrl);

            //assert
            result.StatusCode.ToString().ShouldBe("NotFound");

        }

        [Fact]
        public async Task RequestImageInfo_SUCCESS_ValidTokenValidId()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "image/info?token=";
            var idUrl = "&&id=E9E7BA77-1C78-4293-975C-7251AC4D269B";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token + idUrl);

            //assert
            result.StatusCode.ToString().ShouldBe("OK");

        }

        [Fact]
        public async Task RequestImageInfo_FAILURE_ValidTokenInvalidId()
        {

            // assemble
            var tokenUrl = "authenticate?credentials=" + _validCredentials;
            var getUrl = "image/info?token=";
            var idUrl = "&&id=ThisIsNotAGUID";

            // act
            var token = GetJTokenFromStream(await _client.GetStreamAsync(tokenUrl))["jwt"].ToString();
            var result = await _client.GetAsync(getUrl + token + idUrl);

            //assert
            result.StatusCode.ToString().ShouldBe("BadRequest");

        }

        /// <summary>
        /// Returns a JToken read from a stream, used to consume api JSON responses
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>JToken</returns>
        private JToken GetJTokenFromStream(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            var jsonReader = new JsonTextReader(streamReader);
            return JToken.ReadFrom(jsonReader);
        }

    }
}
