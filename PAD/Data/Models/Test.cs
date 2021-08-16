using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Test : BaseModel
    {
        [Key]
        public int TestId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
