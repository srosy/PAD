using System;
using System.ComponentModel.DataAnnotations;

namespace PAD.Data.Models
{
    public class GridSizeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var narray = value.ToString().Replace(" ", String.Empty).ToUpper().Split('X');

            if (narray.Length == 2 && int.TryParse(narray[0], out var n) && int.TryParse(narray[1], out var n2))
            {
                if (n == n2 && n >= 5 && n <= 100 && n % 5 == 0)
                {
                    return null;
                }
            }

            return new ValidationResult("Invalid Grid Size. Acceptable inputs: [5x5, 25x25, 50x50, ... 100x100]", new[] { validationContext.MemberName });
        }
    }
}
