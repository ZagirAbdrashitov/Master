namespace DrugsManager.Models
{
    public class Drug
    {
        public int Id { get; set; }

        public string Ndc { get; set; }

        public string Name { get; set; }

        public int PackSize { get; set; }

        public Unit Unit { get; set; }

        public decimal Price { get; set; }
    }
}
