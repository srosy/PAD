using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PAD.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAD.Data
{
    public class DbService : IDbService
    {
        private readonly IConfiguration _configuration;
        private const string CONN_STR_RW = "PAD_DB_CONN_RW";
        private const string CONN_STR_RO = "PAD_DB_CONN_RO";

        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Account Queries
        public async Task<bool> AddAccountAsync(Account acc)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Accounts (UserId, FirstName, LastName, DisplayName, Email, ProfilePictureUri, CreateDate) " +
                "VALUES(@UserId, @FirstName, @LastName, @DisplayName, @Email, @ProfilePictureUri, @CreateDate)", acc) > 0;
            return added;
        }

        public async Task<List<Account>> GetAccountListAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Account>("SELECT * FROM Accounts;");
            return result.ToList();
        }

        public async Task<Account> GetAccountAsync(string usrId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Account>("SELECT * FROM Accounts WHERE UserId=@UserId;", new { UserId = usrId });
            var account = result.FirstOrDefault();
            return account;
        }

        public async Task<Account> GetAccountAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Account>("SELECT * FROM Accounts WHERE AccountId=@AccountId;", new { AccountId = accId });
            var account = result.FirstOrDefault();
            return account;
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Account>("SELECT * FROM Accounts WHERE Email=@Email;", new { Email = email });
            var account = result.FirstOrDefault();
            return account;
        }

        public async Task<bool> UpdateAccountAsync(Account acc)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Accounts SET FirstName=@FirstName, LastName=@LastName, DisplayName=@DisplayName, Email=@Email, " +
                "ProfilePictureUri=@ProfilePictureUri, RegisterDate=@RegisterDate, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate WHERE AccountId=@AccountId", acc) > 0;
            return updated;
        }

        public async Task RemoveAccountAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Accounts WHERE AccountId=@AccountId", new { AccountId = accId }) > 0;
        }
        #endregion

        #region Settings Queries
        public async Task<bool> AddSettingsAsync(Settings set)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Settings (AccountId, Theme, NotificationsEnabled, Biography, CreateDate) " +
                "VALUES(@AccountId, @Theme, @NotificationsEnabled, @Biography, @CreateDate)", set) > 0;
            return added;
        }

        public async Task<Settings> GetSettingsAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Settings>("SELECT * FROM Settings WHERE AccountId=@AccountId;", new { AccountId = accId });
            var setting = result.FirstOrDefault();
            return setting;
        }

        public async Task<bool> UpdateSettingsAsync(Settings set)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Settings SET Theme=@Theme, AvatarUrl=@AvatarUrl, NotificationsEnabled=@NotificationsEnabled, Biography=@Biography, " +
                "SocialMediaLink1=@SocialMediaLink1, SocialMediaLink2=@SocialMediaLink2, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate WHERE SettingID=@SettingId", set) > 0;
            return updated;
        }

        public async Task RemoveSettingsAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Settings WHERE AccountId=@AccountId", new { AccountId = accId }) > 0;
        }

        public async Task<List<Theme>> GetThemes()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Theme>("SELECT * FROM Themes");
            return result.ToList();
        }
        #endregion

        #region Project Queries
        public async Task<bool> AddProjectAsync(Project proj)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Projects (AccountId, Name, DisplayTitle, Description, GridSize, Data, CreateDate) " +
                "VALUES(@AccountId, @Name, @DisplayTitle, @Description, @GridSize, @Data, @CreateDate)", proj) > 0;
            return added;
        }

        public async Task<List<Project>> GetAllProjectListAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Project>("SELECT * FROM Projects");
            return result.ToList();
        }

        public async Task<List<Project>> GetProjectListAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Project>("SELECT * FROM Projects WHERE AccountId=@AccountId", new { AccountId = accId });
            return result.ToList();
        }

        public async Task<Project> GetProjectAsync(int projId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Project>("SELECT * FROM Projects WHERE ProjectId=@ProjectId", new { ProjectId = projId });
            var project = result.FirstOrDefault();
            return project;
        }

        public async Task<bool> UpdateProjectAsync(Project proj)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Projects SET Name=@Name, DisplayTitle=@DisplayTitle, Description=@Description, GridSize=@GridSize, " +
                "Data=@Data, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate where ProjectId=@ProjectId", proj) > 0;

            return updated;
        }

        public async Task RemoveProjectAsync(int projId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Projects WHERE ProjectId=@ProjectId", new { ProjectId = projId }) > 0;
        }
        #endregion

        #region Image Queries
        public async Task<bool> AddImageAsync(Image img)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Images (AccountId, Url, ProjectId, CreateDate) VALUES(@AccountId, @Url, @ProjectId, @CreateDate)", img) > 0;
            return added;
        }

        public async Task<List<Image>> GetAllImageListAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Image>("SELECT * FROM Images");
            return result.ToList();
        }

        public async Task<List<Image>> GetImageListAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Image>("SELECT * FROM Images WHERE AccountId=@AccountId", new { AccountId = accId });
            return result.ToList();
        }

        public async Task<Image> GetImageAsync(Guid imgId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Image>("SELECT * FROM Images WHERE ImageId=@ImageId", new { ImageId = imgId });
            var image = result.FirstOrDefault();
            return image;
        }

        public async Task<bool> UpdateImageAsync(Image img)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Images SET Url=@Url, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate where ImageId=@ImageId", img) > 0;
            return updated;
        }

        public async Task RemoveImageAsync(Guid imgId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Images WHERE ImageId=@ImageId", new { ImageId = imgId }) > 0;
        }
        #endregion

        #region Palette Queries
        public async Task<bool> AddPaletteAsync(Palette pal)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Pallettes (AccountId, HexCodes, Name, CreateDate) values(@AccountId, @HexCodes, @Name, @CreateDate)", pal) > 0;
            return added;
        }

        public async Task<List<Palette>> GetPaletteListAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Palette>("SELECT * FROM Pallettes WHERE AccountId=@AccountId AND DeleteDate IS NULL", new { AccountId = accId });
            return result.ToList();
        }

        public async Task<Palette> GetPaletteAsync(int palId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Palette>("SELECT * FROM Pallettes WHERE PalletteId=@PalletteId", new { PalletteId = palId });
            var palette = result.FirstOrDefault();
            return palette;
        }

        public async Task UpdatePaletteAsync(Palette pal)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Pallettes SET HexCodes=@HexCodes, Name=@Name, UpdateDate=@UpdateDate, " +
                "DeleteDate=@DeleteDate where PalletteId=@PalletteId", pal) > 0;
        }

        public async Task RemovePaletteAsync(int palId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Pallettes WHERE PalletteId=@PaletteId", new { PaletteId = palId }) > 0;
        }
        #endregion

        #region Favorite Queries
        public async Task<bool> AddFavoriteAsync(Favorite fav)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Favorites (AccountId, ImageId, CreateDate) values=(@AccountId, @ImageId, @CreateDate)", fav) > 0;
            return added;
        }

        public async Task<List<Favorite>> GetFavoriteListAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Favorite>("SELECT * FROM Favorites WHERE AccountId=@AccountId", new { AccountId = accId });
            return result.ToList();
        }

        public async Task<Favorite> GetFavoriteAsync(int favId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Favorite>("SELECT * FROM Favorites WHERE FavoriteId=@FavoriteId", new { FavoriteId = favId });
            var fav = result.FirstOrDefault();
            return fav;
        }

        public async Task UpdateFavoriteAsync(Favorite fav)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Favorites SET ImageId=@ImageId, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate " +
                "where FavoriteId=@FavoriteId", fav) > 0;
        }

        public async Task RemoveFavoriteAsync(int favId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Favorites WHERE FavoriteId=@FavoriteId", new { FavoriteId = favId }) > 0;
        }
        #endregion

        #region Rating Queries
        public async Task<bool> AddRating(Rating rat)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var contentType = (int)rat.ContentType;
            var ratingType = (int)rat.RatingType;
            var added = await db.ExecuteAsync($"INSERT INTO Ratings (AccountId, ContentType, ItemId, CreateDate, RatingType) VALUES(@AccountId, {contentType}, @ItemId, @CreateDate, {ratingType})", rat) > 0;
            return added;
        }

        public async Task<List<Rating>> GetRatingListAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Rating>("SELECT * FROM Ratings");
            return result.ToList();
        }

        public async Task<Rating> GetRatingAsync(int ratId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Rating>("SELECT * FROM Ratings WHERE RatingId=@RatingId", new { RatingId = ratId });
            var rat = result.FirstOrDefault();
            return rat;
        }

        public async Task<Rating> GetRatingByItemAsync(int accId, Guid itemId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Rating>("SELECT * FROM Ratings WHERE AccountId=@AccountId AND ItemId=@ItemId", new { AccountId = accId, ItemId = itemId });
            var rat = result.FirstOrDefault();
            return rat;
        }

        public async Task<int> GetRatingCountByItemId(Guid itemId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<int>("SELECT COUNT(ItemId) FROM Ratings WHERE ItemId=@ItemId", new { ItemId = itemId });
            var rat = result.FirstOrDefault();
            return rat;
        }

        public async Task<int> GetTotalUserRatingsAsync(int accId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<int>("SELECT COUNT(i.AccountId) FROM Ratings r INNER JOIN Images i ON i.ImageId = r.ItemId WHERE i.AccountId=@AccountId", new { AccountId = accId });
            var karma = result.FirstOrDefault();
            return karma;
        }

        public async Task UpdateRating(Rating rat)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var ratingType = (int)rat.RatingType;
            var added = await db.ExecuteAsync($"UPDATE Ratings SET ItemId=@ItemId, RatingType={ratingType}, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate where RatingId=@RatingId", rat) > 0;
        }

        public async Task RemoveRatingAsync(int ratId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Ratings WHERE RatingId=@RatingId", new { RatingId = ratId }) > 0;
        }
        #endregion

        #region Comment Queries
        public async Task<bool> AddComment(Comment com)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Comments (AccountId, ImageId, CommentText, CreateDate) VALUES(@AccountId, @ImageId, @CommentText, " +
                "@CreateDate)", com) > 0;
            return added;
        }

        public async Task<List<Comment>> GetCommentListAsync(Guid imgId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Comment>("SELECT * FROM Comments WHERE ImageId=@ImageId", new { ImageId = imgId });
            return result.ToList();
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Comment>("SELECT * FROM Comments");
            return result.ToList();
        }

        public async Task<Comment> GetCommentAsync(int comId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Comment>("SELECT * FROM Comments WHERE CommentId=@CommentId", new { CommentId = comId });
            var com = result.FirstOrDefault();
            return com;
        }

        public async Task UpdateComment(Comment com)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Comments SET ImageId=@ImageId, CommentText=@CommentText, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate " +
                "where CommentId=@CommentId", com) > 0;
        }

        public async Task RemoveCommentAsync(Guid comId)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var deleted = await db.ExecuteAsync("DELETE FROM Comments WHERE CommentId=@CommentId", new { CommentId = comId }) > 0;
        }
        #endregion

        #region Test Queries
        public async Task<List<Test>> GetTestsAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Test>("SELECT * FROM Tests;");
            return result.ToList();
        }

        public async Task<int> GetTestCountAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            int result = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) from Tests");
            return result;
        }

        public async Task<bool> AddTestAsync(Test Test)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var added = await db.ExecuteAsync("INSERT INTO Tests (Message, Email, CreateDate) " +
                "values (@Message, @Email, @CreateDate)", Test) > 0;
            return added;
        }

        public async Task UpdateTestAsync(Test Test)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            var updated = await db.ExecuteAsync("UPDATE Tests SET Message=@Message, Email=@Email, UpdateDate=@UpdateDate, DeleteDate=@DeleteDate where TestId=@TestId", Test) > 0;
        }

        public async Task RemoveTestAsync(int Testid)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            db.Open();
            await db.ExecuteAsync("DELETE FROM Tests WHERE TestId=@TestId", new { TestId = Testid });
        }
        #endregion

        public async Task<bool> CleanDB()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RW));
            var sql = "exec [clean_up_deleted_accounts]";
            try
            {
                db.Query(sql, null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Carousel Queries

        /// <summary>
        /// Get top 5 ranking images
        /// </summary>
        /// <returns>Task<List<Image>> Images</returns>
        public async Task<List<Image>> GetCarouselImagesListAsync()
        {
            using var db = new SqlConnection(_configuration.GetConnectionString(CONN_STR_RO));
            db.Open();
            var result = await db.QueryAsync<Image>(
                "SELECT TOP (5) img.ImageId," +
                " img.AccountId," +
                " img.CreateDate," +
                " img.UpdateDate," +
                " img.DeleteDate," +
                " img.Url," +
                " img.ProjectId" +
                " FROM Images img " +
                "INNER JOIN Ratings rtng ON img.ImageId = rtng.ItemId " +
                "ORDER BY rtng.RatingId DESC");
            return result.ToList();
        }

        #endregion
    }
}

