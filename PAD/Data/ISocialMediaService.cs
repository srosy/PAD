using PAD.Data.Enums;
using PAD.Data.Models;
using System;
using System.Threading.Tasks;

namespace PAD.Data
{
    interface ISocialMediaService
    {
        public Task<Rating> RateImage(int acctId, ContentType conType, RatingType ratType, Guid itemId, Rating existing);
    }
}
