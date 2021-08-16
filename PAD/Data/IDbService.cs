using PAD.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAD.Data
{
    public interface IDbService
    {
        // account
        public Task<bool> AddAccountAsync(Account acc);
        public Task<List<Account>> GetAccountListAsync();
        public Task<Account> GetAccountAsync(string usrId);
        public Task<Account> GetAccountAsync(int accId);
        public Task<Account> GetAccountByEmailAsync(string email);
        public Task<bool> UpdateAccountAsync(Account acc);
        public Task RemoveAccountAsync(int accId);

        // settings
        public Task<bool> AddSettingsAsync(Settings set);
        public Task<Settings> GetSettingsAsync(int accId);
        public Task<bool> UpdateSettingsAsync(Settings set);
        public Task RemoveSettingsAsync(int accId);
        public Task<List<Theme>> GetThemes();

        // project
        public Task<bool> AddProjectAsync(Project proj);
        public Task<List<Project>> GetAllProjectListAsync();
        public Task<List<Project>> GetProjectListAsync(int accId);
        public Task<Project> GetProjectAsync(int projId);
        public Task<bool> UpdateProjectAsync(Project proj);
        public Task RemoveProjectAsync(int projId);

        // image
        public Task<bool> AddImageAsync(Image img);
        public Task<List<Image>> GetAllImageListAsync();
        public Task<List<Image>> GetImageListAsync(int accId);
        public Task<Image> GetImageAsync(Guid imgId);
        public Task<bool> UpdateImageAsync(Image img);
        public Task RemoveImageAsync(Guid imgId);
        public Task<List<Image>> GetCarouselImagesListAsync();
        public Task<int> GetRatingCountByItemId(Guid itemId);

        // palette
        public Task<bool> AddPaletteAsync(Palette pal);
        public Task<List<Palette>> GetPaletteListAsync(int accId);
        public Task<Palette> GetPaletteAsync(int palId);
        public Task UpdatePaletteAsync(Palette pal);
        public Task RemovePaletteAsync(int palId);

        // favorite
        public Task<bool> AddFavoriteAsync(Favorite fav);
        public Task<List<Favorite>> GetFavoriteListAsync(int accId);
        public Task<Favorite> GetFavoriteAsync(int favId);
        public Task UpdateFavoriteAsync(Favorite fav);
        public Task RemoveFavoriteAsync(int favId);

        // rating
        public Task<bool> AddRating(Rating rat);
        public Task<List<Rating>> GetRatingListAsync();
        public Task<Rating> GetRatingAsync(int ratId);
        public Task<Rating> GetRatingByItemAsync(int accId, Guid itemId);
        public Task<int> GetTotalUserRatingsAsync(int accId);
        public  Task UpdateRating(Rating rat);
        public Task RemoveRatingAsync(int ratId);

        // comment
        public Task<bool> AddComment(Comment com);
        public Task<List<Comment>> GetCommentListAsync(Guid imgId);
        public Task<Comment> GetCommentAsync(int comId);
        public Task<List<Comment>> GetAllCommentsAsync();
        public Task UpdateComment(Comment com);
        public Task RemoveCommentAsync(Guid comId);

        // tests
        public Task<List<Test>> GetTestsAsync();
        public Task<int> GetTestCountAsync();
        public Task<bool> AddTestAsync(Test Test);
        public Task UpdateTestAsync(Test Test);
        public Task RemoveTestAsync(int Testid);

        // scripts
        public Task<bool> CleanDB();
    }
}