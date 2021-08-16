using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD.Shared.ViewModels
{
    public class PersonalGalleryViewModel : ViewModel
    {
        public string BaseImageUrl { get; set; }
        public List<PAD.Data.Models.Image> ImageList;
        public List<PAD.Data.Models.Project> ProjectList;
        public List<ImageProjectJoin> JoinList;
        public class ImageProjectJoin
        {
            public string Url { get; set; }
            public string DisplayTitle { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime? UpdateDate { get; set; }
        }

        public void JoinImagesAndProjects()
        {

            JoinList = new List<ImageProjectJoin>();

            if(ImageList != null && ProjectList != null)
            {
                var joinResult = from i in ImageList
                                  join p in ProjectList on i.ProjectId equals p.ProjectId
                                  select new
                                  {
                                      Url = i.Url,
                                      DisplayTitle = p.DisplayTitle,
                                      CreateDate = i.CreateDate,
                                      UpdateDate = i.UpdateDate
                                  };
                foreach (var row in joinResult)
                {
                    ImageProjectJoin currentJoinObject = new ImageProjectJoin();
                    currentJoinObject.Url = row.Url;
                    currentJoinObject.DisplayTitle = row.DisplayTitle;
                    currentJoinObject.CreateDate = row.CreateDate;
                    if (row.UpdateDate != null)
                    {
                        currentJoinObject.UpdateDate = row.UpdateDate;
                    }
                    JoinList.Add(currentJoinObject);
                }
            }
        }

    }


}
