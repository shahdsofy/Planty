namespace Blog_Platform.Models
{
    public class Token
    {
        public string UserId { get; set; }
        public string token { get; set; }
        public bool IsRevoked { get; set; }
        public AppUser User { get; set; }
    }
}
