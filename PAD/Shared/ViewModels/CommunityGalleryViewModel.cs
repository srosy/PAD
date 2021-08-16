using System;
using System.Collections.Generic;
using System.Linq;
using PAD.Data.Models;

namespace PAD.Shared.ViewModels
{
    public class CommunityGalleryViewModel : ViewModel
    {
        public string BaseImageUrl { get; set; }
        public List<Image> ImageList;
        public List<Project> ProjectList;
        public List<Account> AccountList;
        public List<ImageProjectJoin> JoinList;
        public List<Rating> RatingList;
        public List<Comment> CommentList;
        public string ActiveImgUrl { get; set; }
        public Guid ActiveImageId { get; set; }
        public class ImageProjectJoin
        {
            public string Url { get; set; }
            public string DisplayTitle { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime? UpdateDate { get; set; }
            public DateTime? DeleteDate { get; set; }
            public string DisplayName { get; set; }
            public string ProfilePictureUri { get; set; }
            public Guid Id { get; set; }
        }

        public void JoinImagesAndProjects()
        {
            JoinList = new List<ImageProjectJoin>();

            if (ImageList != null && ProjectList != null)
            {
                var joinResult = from i in ImageList
                                 join p in ProjectList on new { i.ProjectId, i.AccountId } equals new { p.ProjectId, p.AccountId }
                                 join a in AccountList on p.AccountId equals a.AccountId
                                 where i.DeleteDate == null
                                 orderby i.CreateDate descending, i.UpdateDate descending
                                 select new
                                 {
                                     i.ImageId,
                                     i.Url,
                                     p.DisplayTitle,
                                     i.CreateDate,
                                     i.UpdateDate,
                                     i.DeleteDate,
                                     a.DisplayName,
                                     a.ProfilePictureUri
                                 };

                foreach (var row in joinResult)
                {
                    ImageProjectJoin currentJoinObject = new ImageProjectJoin();
                    currentJoinObject.Id = row.ImageId;
                    currentJoinObject.Url = row.Url;
                    currentJoinObject.DisplayTitle = row.DisplayTitle;
                    currentJoinObject.CreateDate = row.CreateDate;
                    currentJoinObject.UpdateDate = row.UpdateDate;
                    currentJoinObject.DisplayName = string.IsNullOrEmpty(row.DisplayName) ? "Private User" : row.DisplayName;
                    currentJoinObject.ProfilePictureUri = row.ProfilePictureUri ?? "/images/DefaultPFPTransparent.webp";
                    JoinList.Add(currentJoinObject);
                }
            }
        }
    }
}
