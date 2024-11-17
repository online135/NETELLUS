namespace NETELLUS.Models
{
    public class Like
    {
        public int Id { get; set; }       // Like 的唯一標識
        public int UserId { get; set; }   // 點讚的用戶 id
        public int TargetId { get; set; } // 點讚目標的ID (Post 或 Comment) 
        public string TargetType { get; set; } // 點讚目標的類型 ("Post" 或 "Comment")
    }
}
