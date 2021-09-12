using DrugsManager.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DrugsManager.Models
{
    public class Drug
    {
        public int Id { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [AlphaNumeric(ErrorMessage = "The value must be alpha-numeric")]
        public string Ndc { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? PackSize { get; set; }

        [Required]
        [Range(0, 2)]
        public Unit? Unit { get; set; }

        [Required]
        [DecimalWithCustomization(1, 10, 2, 2, false)]
        public decimal? Price { get; set; }
    }
}
