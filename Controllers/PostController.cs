using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NETELLUS.Extensions;
using NETELLUS.Models;

namespace NETELLUS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        // 初始化假數據列表
        private static List<Post> posts = new List<Post>
        {
            new() {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern, object-oriented programming language developed by Microsoft.",
                CreatedAt = DateTime.Now.AddDays(-10),
                UpdatedAt = DateTime.Now.AddDays(-10),
                Comments = new List<Comment>
                {
                    new Comment { 
                        Id = 1, 
                        PostId = 1, 
                        Author = "Alice", 
                        Content = "Great article!", 
                        CreatedAt = DateTime.Now.AddDays(-9),
                        UpdatedAt = DateTime.Now.AddDays(-9)
                    },
                    new Comment { 
                        Id = 2, 
                        PostId = 1, 
                        Author = "Bob", 
                        Content = "Very informative.", 
                        CreatedAt = DateTime.Now.AddDays(-8) ,
                        UpdatedAt = DateTime.Now.AddDays(-8)
                    }
                }
            },
            new() {
                Id = 2,
                Title = "Getting Started with ASP.NET Core",
                Content = "ASP.NET Core is a cross-platform framework for building modern, cloud-based web applications.",
                CreatedAt = DateTime.Now.AddDays(-8),
                UpdatedAt = DateTime.Now.AddDays(-8),
                Comments = new List<Comment>
                {
                    new Comment { 
                        Id = 3, 
                        PostId = 2, 
                        Content = "I struggled with setup, but this helped.", 
                        CreatedAt = DateTime.Now.AddDays(-7) 
                    }
                }
            }
        };

        #region 文章
        // 獲取所有文章
        [HttpGet]
        public IEnumerable<Post> GetAll()
        {
            return posts;
        }

        // 獲取特定文章
        [HttpGet("{id}")]
        public ActionResult<Post> GetById(int id)
        {
            Post? post = posts.FirstOrDefault(p => p.Id == id);
            if (post.IsNull())
            {
                return NotFound();
            }
            return post;
        }

        // 創建新文章
        [HttpPost]
        public ActionResult<Post> Create([FromBody] Post newPost)
        {
            newPost.Id = posts.Count > 0 ? posts.Max(p => p.Id) + 1 : 1;
            newPost.CreatedAt = DateTime.Now;
            newPost.UpdatedAt = DateTime.Now;
            posts.Add(newPost);
            return CreatedAtAction(nameof(GetById), new { id = newPost.Id }, newPost);
        }

        // 更新文章
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Post updatedPost)
        {
            var post = posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            post.UpdatedAt = DateTime.Now;
            return NoContent();
        }

        // 刪除文章
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Post? post = posts.FirstOrDefault(p => p.Id == id);
            if (post.IsNull())
            {
                return NotFound();
            }
            posts.Remove(post);
            return NoContent();
        }
        #endregion


        #region 評論
        // 獲取文章的所有評論
        [HttpGet("{postId}/comments")]
        public ActionResult<IEnumerable<Comment>> GetComments(int postId)
        {
            var post = posts.FirstOrDefault(p => p.Id == postId);
            if (post.IsNull())
            {
                return NotFound();
            }

            return post.Comments;
        }

        // 為指定文章創建新評論
        [HttpPost("{postId}/comments")]
        public ActionResult<Comment> CreateComment(int postId, [FromBody] Comment newComment)
        {
            Post? post = posts.FirstOrDefault(p => p.Id == postId);
            if (post.IsNull())
            {
                return NotFound();
            }

            newComment.Id = post.Comments.Count > 0 ? post.Comments.Max(c => c.Id) + 1 : 1;
            newComment.PostId = postId;
            newComment.CreatedAt = DateTime.Now;
            newComment.UpdatedAt = DateTime.Now;

            post.Comments.Add(newComment);
            return CreatedAtAction(nameof(GetComments), new { postId = postId }, newComment);
        }

        // 更新指定評論
        [HttpPut("{postId}/comments/{commentId}")]
        public IActionResult UpdateComment(int postId, int commentId, [FromBody] Comment updatedComment)
        {
            var post = posts.FirstOrDefault(p => p.Id == postId);
            if (post.IsNull())
            {
                return NotFound();
            }

            var comment = post.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment.IsNull())
            {
                return NotFound();
            }

            comment.Content = updatedComment.Content;
            comment.UpdatedAt = DateTime.Now; // 這裡保持創建時間一致，設置新的更新時間
            return NoContent();
        }

        // 刪除指定評論
        [HttpDelete("{postId}/comments/{commentId}")]
        public IActionResult DeleteComment(int postId, int commentId)
        {
            Post? post = posts.FirstOrDefault(p => p.Id == postId);
            if (post.IsNull())
            {
                return NotFound();
            }

            Comment? comment = post.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment.IsNull())
            {
                return NotFound();
            }

            post.Comments.Remove(comment);
            return NoContent();
        }
        #endregion
    }
}
