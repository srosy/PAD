using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAD.Data.Models
{
    public class Theme
    {
        [Required]
        public string Title { get; set; }
        public string Class { get; set; }
    }
}
