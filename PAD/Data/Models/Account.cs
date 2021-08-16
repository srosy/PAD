using System;
using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Account : BaseModel
    {
        [Key]
        [Required]
        public int AccountId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string DisplayName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ProfilePictureUri { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? RegisterDate { get; set; }
    }
}
