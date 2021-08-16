using PAD.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD.Shared.ViewModels
{
    public class IndexViewModel : ViewModel
    {
        public string BaseImageUrl { get; set; }
        public bool HasProjects => ResumeProjId > 0 ? true : false;
        public int ResumeProjId { get; set; }

        public List<Image> ImageList;
        public List<Project> ProjectList;
        public List<Account> AccountList;
        public List<CarouselImageJoin> JoinList;
        public List<Rating> RatingList;
        public List<Comment> CommentList;
        public string ActiveImgUrl { get; set; }
        public Guid ActiveImageId { get; set; }

        public class CarouselImageJoin
        {
            public string Url { get; set; }
            public Guid ImageId { get; set; }
            public string DisplayTitle { get; set; }
            public DateTime CreateDate { get; set; }
            public string DisplayName { get; set; }
            public int Ratings { get; set; }
        }

        public class ImageProjectJoin
        {
            public string Url { get; set; }
            public Guid ImageId { get; set; }
            public string DisplayTitle { get; set; }
            public DateTime CreateDate { get; set; }
            public string DisplayName { get; set; }
        }

        public void JoinImagesAndProjects()
        {
            JoinList = new List<CarouselImageJoin>();

            if (ImageList != null && ProjectList != null)
            {
                var joinResult = from i in ImageList //something throws an exception here
                                 join p in ProjectList on new { i.ProjectId, i.AccountId } equals new { p.ProjectId, p.AccountId }
                                 join a in AccountList on p.AccountId equals a.AccountId
                                 where i.DeleteDate == null
                                 orderby i.CreateDate descending, i.UpdateDate descending
                                 select new ImageProjectJoin()
                                 {
                                     Url = i.Url,
                                     ImageId = i.ImageId,
                                     DisplayTitle = p.DisplayTitle,
                                     CreateDate = i.CreateDate,
                                     DisplayName = a.DisplayName
                                 };

                foreach (var row in joinResult)
                {
                    CarouselImageJoin currentJoinObject = new CarouselImageJoin();
                    currentJoinObject.Url = row.Url;
                    currentJoinObject.ImageId = row.ImageId;
                    currentJoinObject.DisplayTitle = row.DisplayTitle;
                    currentJoinObject.CreateDate = row.CreateDate;
                    currentJoinObject.DisplayName = string.IsNullOrEmpty(row.DisplayName) ? "Private User" : row.DisplayName;
                    JoinList.Add(currentJoinObject);
                }
            }
        }
    }
}
