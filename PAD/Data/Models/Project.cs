using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Project : BaseModel
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string DisplayTitle { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        [GridSizeValidator]
        public string GridSize { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Data { get; set; }
    }
}
