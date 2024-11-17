using System.Xml.Linq;

namespace NETELLUS.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 一對多關係, 每篇文章有多個評論
        public List<Comment> Comments { get; set; } = [];

        // 文章的點讚列表
        public List<Like> Likes { get; set; } = [];
    }
}
