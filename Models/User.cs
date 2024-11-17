namespace NETELLUS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // 儲存加密後的密碼

        // 對應到 UserProfile
        public UserProfile Profile { get; set; }
    }
}
