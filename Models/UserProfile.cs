namespace NETELLUS.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        // 與 User 進行關聯
        public int UserId { get; set; }
        public User User { get; set; }

        // 額外的個人資訊欄位
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
