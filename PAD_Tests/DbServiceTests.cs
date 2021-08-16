using Shouldly;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using PAD.Data;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace PAD_Tests
{
    public class DbServiceTests
    {
        private readonly DbService _dbService;
        public DbServiceTests()
        {
            var mockConnStrings = new Mock<IConfigurationSection>(); // the conn strings section is an object made up of these conn strings
            mockConnStrings.SetupGet(m => m[It.Is<string>(s => s == "PAD_DB_CONN_RO")]).Returns("Server=tcp:gavispe.database.windows.net,1433;Initial Catalog=PAD;Persist Security Info=False;User ID=pad_ro;Password=WSU2021_CS4450_ro;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            mockConnStrings.SetupGet(m => m[It.Is<string>(s => s == "PAD_DB_CONN_RW")]).Returns("Server=tcp:gavispe.database.windows.net,1433;Initial Catalog=PAD;Persist Security Info=False;User ID=pad_rw;Password=WSU2021_CS4450_rw;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConnStrings.Object);

            _dbService = new DbService(mockConfiguration.Object);
        }

        [Fact]
        public async Task AccountTests()
        {
            #region Create
            // arrange
            var testAcct = new PAD.Data.Models.Account()
            {
                UserId = "e6b3e89f-740d-4703-8ae6-a9be072f4cda",
                FirstName = "Darth",
                LastName = "Tester",
                DisplayName = "BeastMaster64",
                Email = "gavinrosenvall@mail.weber.edu",
                CreateDate = new DateTime(2021, 01, 01, 0, 0, 0)
            };

            // act
            await _dbService.AddAccountAsync(testAcct);
            var acct = await _dbService.GetAccountAsync(testAcct.UserId);

            // assert
            acct.ShouldNotBeNull();
            acct.ShouldBeOfType(testAcct.GetType());
            acct.UserId.ShouldBe(testAcct.UserId);
            acct.FirstName.ShouldBe(testAcct.FirstName);
            acct.LastName.ShouldBe(testAcct.LastName);
            acct.DisplayName.ShouldBe(testAcct.DisplayName);
            acct.Email.ShouldBe(testAcct.Email);
            acct.CreateDate.ShouldBe(testAcct.CreateDate);
            #endregion

            #region Update
            // arrange
            acct.FirstName = "Master";
            acct.LastName = "Kenobi";
            acct.RegisterDate = DateTime.Now.Date;
            acct.UpdateDate = new DateTime(2021, 01, 01, 0, 0, 0);
            acct.DeleteDate = new DateTime(2021, 01, 01, 0, 0, 0);

            // act
            await _dbService.UpdateAccountAsync(acct);
            var updated = await _dbService.GetAccountByEmailAsync(testAcct.Email);

            // assert
            updated.ShouldNotBeNull();
            acct.ShouldBeOfType(updated.GetType());
            acct.UserId.ShouldBe(updated.UserId);
            acct.FirstName.ShouldBe(updated.FirstName);
            acct.LastName.ShouldBe(updated.LastName);
            acct.DisplayName.ShouldBe(updated.DisplayName);
            acct.Email.ShouldBe(updated.Email);
            acct.RegisterDate.ShouldBe(updated.RegisterDate);
            acct.UpdateDate.ShouldBe(updated.UpdateDate);
            #endregion

            #region Delete
            // act
            await _dbService.RemoveAccountAsync(acct.AccountId);
            acct = await _dbService.GetAccountAsync(testAcct.UserId);

            // assert
            acct.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task SettingsTests()
        {
            #region Create
            // arrange
            var testStng = new PAD.Data.Models.Settings()
            {
                AccountId = 43,
                DarkMode = false,
                NotificationsEnabled = false,
                CreateDate = new DateTime(2021, 01, 01, 0, 0, 0)
            };

            // act
            await _dbService.AddSettingsAsync(testStng);
            var stng = await _dbService.GetSettingsAsync(testStng.AccountId);

            // assert
            stng.ShouldNotBeNull();
            stng.ShouldBeOfType(testStng.GetType());
            stng.AccountId.ShouldBe(testStng.AccountId);
            stng.DarkMode.ShouldBe(testStng.DarkMode);
            stng.NotificationsEnabled.ShouldBe(testStng.NotificationsEnabled);
            stng.CreateDate.ShouldBe(testStng.CreateDate);
            #endregion

            #region Update
            // arrange
            stng.DarkMode = true;
            stng.AvatarUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/0debd689-2586-47cf-afdc-a03706f06d6b/de5r39v-41eaccb1-f73f-4d77-b5fc-83b075e1ce00.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzBkZWJkNjg5LTI1ODYtNDdjZi1hZmRjLWEwMzcwNmYwNmQ2YlwvZGU1cjM5di00MWVhY2NiMS1mNzNmLTRkNzctYjVmYy04M2IwNzVlMWNlMDAucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.57-xLxI8IyMHkscdYmLca71fvj5m4FPHDbdEPSshZ3E";
            stng.NotificationsEnabled = true;
            stng.Biography = "I catch da fish.";
            stng.SocialMediaLink1 = "https://github.com/Abyssalisk";
            stng.SocialMediaLink2 = "https://soundcloud.com/abyssalisk";
            stng.UpdateDate = DateTime.Now.Date;
            stng.DeleteDate = DateTime.Now.Date;

            // act
            await _dbService.UpdateSettingsAsync(stng);
            var updated = await _dbService.GetSettingsAsync(testStng.AccountId);

            // assert
            updated.ShouldNotBeNull();
            stng.DarkMode.ShouldBe(updated.DarkMode);
            stng.AvatarUrl.ShouldBe(updated.AvatarUrl);
            stng.NotificationsEnabled.ShouldBe(updated.NotificationsEnabled);
            stng.Biography.ShouldBe(updated.Biography);
            stng.SocialMediaLink1.ShouldBe(updated.SocialMediaLink1);
            stng.SocialMediaLink2.ShouldBe(updated.SocialMediaLink2);
            stng.UpdateDate.ShouldBe(updated.UpdateDate);
            stng.DeleteDate.ShouldBe(updated.DeleteDate);
            #endregion

            #region Delete
            // act
            await _dbService.RemoveSettingsAsync(stng.AccountId);
            stng = await _dbService.GetSettingsAsync(testStng.AccountId);

            // assert
            stng.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task ProjectTests()
        {
            #region Create
            // arrange
            var testProj = new PAD.Data.Models.Project
            {
                AccountId = 41,
                Name = "Good Shiba",
                DisplayTitle = "Doge",
                Description = "Much wow",
                GridSize = "64x64",
                Data = "TODO",
                CreateDate = DateTime.Now.Date
            };

            // act
            await _dbService.AddProjectAsync(testProj);
            var projList = await _dbService.GetProjectListAsync(testProj.AccountId);
            var proj = await _dbService.GetProjectAsync(projList.FirstOrDefault().ProjectId);

            // assert
            proj.ShouldNotBeNull();
            proj.ShouldBeOfType(testProj.GetType());
            proj.AccountId.ShouldBe(testProj.AccountId);
            proj.Name.ShouldBe(testProj.Name);
            proj.DisplayTitle.ShouldBe(testProj.DisplayTitle);
            proj.Description.ShouldBe(testProj.Description);
            proj.GridSize.ShouldBe(testProj.GridSize);
            proj.Data.ShouldBe(testProj.Data);
            proj.CreateDate.ShouldBe(testProj.CreateDate);
            #endregion

            #region Update
            // arrange
            proj.Name = "Bad Shiba";
            proj.DisplayTitle = "Woof woof mother fluffer";
            proj.Description = "Much Bad";
            proj.GridSize = "128x128";
            proj.Data = "It's coming... I promise.";
            proj.UpdateDate = DateTime.Now.Date;
            proj.DeleteDate = DateTime.Now.Date;

            // act
            await _dbService.UpdateProjectAsync(proj);
            var updatedList = await _dbService.GetProjectListAsync(testProj.AccountId);
            var updated = await _dbService.GetProjectAsync(updatedList.FirstOrDefault().ProjectId);

            // assert
            updated.ShouldNotBeNull();
            proj.Name.ShouldBe(updated.Name);
            proj.DisplayTitle.ShouldBe(updated.DisplayTitle);
            proj.Description.ShouldBe(updated.Description);
            proj.GridSize.ShouldBe(updated.GridSize);
            proj.Data.ShouldBe(updated.Data);
            proj.UpdateDate.ShouldBe(updated.UpdateDate);
            proj.DeleteDate.ShouldBe(updated.DeleteDate);
            #endregion

            #region Delete
            // act
            await _dbService.RemoveProjectAsync(proj.ProjectId);
            proj = await _dbService.GetProjectAsync(updated.ProjectId);

            // assert
            proj.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task ImageTests()
        {
            #region Create
            // arrange
            var testImg = new PAD.Data.Models.Image
            {
                AccountId = 43,
                Url = "https://previews.123rf.com/images/kmarfu/kmarfu1807/kmarfu180700049/115007286-pixel-art-shiba-inu-smiling-dog-vector-icon-.jpg",
                ProjectId = 27,
                CreateDate = DateTime.Now.Date
            };

            // act
            await _dbService.AddImageAsync(testImg);
            var imgList = await _dbService.GetImageListAsync(testImg.AccountId);
            var img = await _dbService.GetImageAsync(imgList.FirstOrDefault().ImageId);

            // assert
            img.ShouldNotBeNull();
            img.ShouldBeOfType(testImg.GetType());
            img.AccountId.ShouldBe(testImg.AccountId);
            img.Url.ShouldBe(testImg.Url);
            img.ProjectId.ShouldBe(testImg.ProjectId);
            img.CreateDate.ShouldBe(testImg.CreateDate);
            #endregion

            #region Update
            // arrange
            img.Url = "https://i.pinimg.com/originals/31/d1/5d/31d15d89f9ee091b462d4217dfc96dce.jpg";
            img.UpdateDate = DateTime.Now.Date;
            img.DeleteDate = DateTime.Now.Date;

            // act
            await _dbService.UpdateImageAsync(img);
            var updatedList = await _dbService.GetImageListAsync(testImg.AccountId);
            var updated = await _dbService.GetImageAsync(updatedList.FirstOrDefault().ImageId);

            // assert
            updated.ShouldNotBeNull();
            img.Url.ShouldBe(updated.Url);
            img.UpdateDate.ShouldBe(updated.UpdateDate);
            img.DeleteDate.ShouldBe(updated.DeleteDate);
            #endregion

            #region Delete
            // act
            await _dbService.RemoveImageAsync(img.ImageId);
            img = await _dbService.GetImageAsync(updated.ImageId);

            // assert
            img.ShouldBeNull();
            #endregion
        }

        [Fact]
        public async Task PaletteTests()
        {
            #region Create
            // arrange
            var testPal = new PAD.Data.Models.Palette()
            {
                PalletteId = 99,
                AccountId = 43,
                Name = "TestPalette",
                HexCodes = "#FFFFFF,#23BBAA,#FA9801,#000000,#5A938A,#FFAAFF",
                CreateDate = DateTime.Now.Date
            };

            // act
            await _dbService.AddPaletteAsync(testPal);
            var pal = await _dbService.GetPaletteAsync(testPal.PalletteId);

            // assert
            pal.ShouldNotBeNull();
            pal.ShouldBeOfType(testPal.GetType());
            pal.AccountId.ShouldBe(testPal.AccountId);
            pal.Name.ShouldBe(testPal.Name);
            pal.HexCodes.ShouldBe(testPal.HexCodes);
            pal.CreateDate.ShouldBe(testPal.CreateDate);
            #endregion

            #region Update
            // arrange
            pal.Name = "TestPalette Updated";
            pal.UpdateDate = DateTime.Now.Date;
            pal.DeleteDate = DateTime.Now.Date;

            // act
            await _dbService.UpdatePaletteAsync(pal);
            var updated = await _dbService.GetPaletteAsync(pal.PalletteId);

            // assert
            updated.ShouldNotBeNull();
            pal.Name.ShouldBe(updated.Name);
            pal.UpdateDate.ShouldBe(updated.UpdateDate);
            pal.DeleteDate.ShouldBe(updated.DeleteDate);
            #endregion

            #region Delete
            // act
            await _dbService.RemovePaletteAsync(pal.PalletteId);
            pal = await _dbService.GetPaletteAsync(updated.PalletteId);

            // assert
            pal.ShouldBeNull();
            #endregion
        }
    }
}