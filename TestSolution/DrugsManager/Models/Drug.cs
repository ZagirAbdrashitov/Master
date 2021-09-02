using System.ComponentModel.DataAnnotations;

namespace DrugsManager.Models
{
    public class Drug
    {
        public int Id { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Ndc { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public int PackSize { get; set; }

        [Required]
        public Unit Unit { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
