using System;
using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Comment : BaseModel
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string CommentText { get; set; }
        [Required]
        public Guid ImageId { get; set; }
    }
}
