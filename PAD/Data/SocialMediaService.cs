using PAD.Data.Enums;
using PAD.Data.Models;
using System;
using System.Threading.Tasks;

namespace PAD.Data
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly IDbService _db;
        public SocialMediaService(IDbService db)
        {
            _db = db;
        }
        public async Task<Rating> RateImage(int acctId, ContentType conType, RatingType ratType, Guid itemId, Rating existing)
        {        
            if (existing == null)
            {
                var rat = new Rating()
                {
                    AccountId = acctId,
                    ContentType = conType,
                    RatingType = ratType,
                    ItemId = itemId,
                    CreateDate = DateTime.Now
                };
                await _db.AddRating(rat);
                return rat;
            }
            else
            {
                existing.UpdateDate = DateTime.Now;
                existing.RatingType = ratType;
                await _db.UpdateRating(existing);
                return existing;
            }
        }
    }
}
