using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class Palette : BaseModel
    {
        [Key]
        public int PalletteId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [DataType(DataType.MultilineText)]
        public string HexCodes { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
