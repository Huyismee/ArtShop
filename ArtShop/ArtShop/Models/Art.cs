namespace ArtShop.Models
{
    public class Art
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Image { get; set; } = null!;

        public string? Description { get; set; }

        public double Price { get; set; }
    }
}
