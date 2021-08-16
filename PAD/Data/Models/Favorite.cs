using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Favorite : BaseModel
    {
        [Key]
        public int FavoriteId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ImageId { get; set; }
    }
}
