namespace ArtShop.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string UserId { get; set; } = null!;
    }
}
