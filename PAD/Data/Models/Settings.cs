using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Settings : BaseModel
    {
        [Key]
        public int SettingId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [DataType(DataType.Text)]
        public string? Theme { get; set; }
        [DataType(DataType.Text)]
        public string? AvatarUrl { get; set; }
        public bool NotificationsEnabled { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Biography { get; set; }
        [DataType(DataType.Text)]
        public string? SocialMediaLink1 { get; set; }
        [DataType(DataType.Text)]
        public string? SocialMediaLink2 { get; set; }
    }
}
