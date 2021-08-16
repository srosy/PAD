using System;
using System.ComponentModel.DataAnnotations;
using PAD.Data.Enums;

namespace PAD.Data.Models
{
    public class Rating : BaseModel
    {
        [Key]
        public int RatingId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public RatingType RatingType { get; set; }
    }
}
