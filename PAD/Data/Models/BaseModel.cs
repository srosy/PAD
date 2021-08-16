using System;
using System.ComponentModel.DataAnnotations;

namespace PAD.Data.Models
{
    public abstract class BaseModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeleteDate { get; set; }
    }
}
