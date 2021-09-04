using System;
using System.ComponentModel.DataAnnotations;

namespace DrugsManager.Models
{
    public class Drug
    {
        private const string NdcFormat = "^[a-zA-Z0-9]*$";
        private const string PriceFormat = @"^\d{1,10}(\.|\,)\d{2}$";

        public int Id { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression(NdcFormat, ErrorMessage = "The value must be alpha-numeric")]
        public string Ndc { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(1, 2147483647)]
        public int PackSize { get; set; }

        [Required]
        [Range(0, 2)]
        public Unit Unit { get; set; }

        [Required]
        [Range(0.01, 9999999999.99)]
        [RegularExpression(PriceFormat, ErrorMessage = "Value must be non-negative number with two decimals")]
        public decimal Price { get; set; }
    }
}
