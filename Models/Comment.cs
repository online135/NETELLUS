namespace NETELLUS.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }  // 外鍵, 關聯 Post
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
