using Shouldly;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PAD_Tests
{
    public class StorageTests
    {
        [Fact]
        public async Task AddImage_SUCCESS_ValidIdLength()
        {
            // assemble
            var originalFileBytes = File.ReadAllBytes(@"E:\School\Summer 2021\CS4450\PAD\PAD_Tests\Images\charmander.webp");

            // act
            var url = await PAD.Data.AzureStorage.UploadFile("charmander.webp", originalFileBytes, Guid.NewGuid());
            using WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(url), @"C:\Users\seros\Downloads\temp.webp");
            var tempFileBytes = File.ReadAllBytes(@"C:\Users\seros\Downloads\temp.webp");

            // assert
            originalFileBytes.ShouldBeEquivalentTo(tempFileBytes);
        }

        [Fact]
        public async Task AddImage_FAILURE_IdLengthTooShort()
        {
            // assemble
            var originalFileBytes = File.ReadAllBytes(@"E:\School\Summer 2021\CS4450\PAD\PAD_Tests\Images\charmander.webp");
            
            // act

            // assert
            await Assert.ThrowsAsync<Azure.RequestFailedException>(async () => await PAD.Data.AzureStorage.UploadFile("charmander.webp", originalFileBytes, 0));
            await Assert.ThrowsAsync<Azure.RequestFailedException>(async () => await PAD.Data.AzureStorage.UploadFile("charmander.webp", originalFileBytes, 18));
        }
    }
}