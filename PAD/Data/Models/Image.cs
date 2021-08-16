using System;
using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Image : BaseModel
    {
        [Key]
        public Guid ImageId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Url { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
