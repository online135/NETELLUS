using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETELLUS.Extensions;
using NETELLUS.Models;

namespace NETELLUS.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        // 初始化假數據列表
        private static readonly List<Post> posts =
        [
            new() {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern, object-oriented programming language developed by Microsoft.",
                CreatedAt = DateTime.Now.AddDays(-10),
                UpdatedAt = DateTime.Now.AddDays(-10),
                Comments =
                [
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
                ],
                Likes =
                [
                    new Like { Id = 1, UserId = 1, TargetId = 1, TargetType = "Post" },
                    new Like { Id = 2, UserId = 2, TargetId = 1, TargetType = "Post" }
                ]
            },
            new() {
                Id = 2,
                Title = "Getting Started with ASP.NET Core",
                Content = "ASP.NET Core is a cross-platform framework for building modern, cloud-based web applications.",
                CreatedAt = DateTime.Now.AddDays(-8),
                UpdatedAt = DateTime.Now.AddDays(-8),
                Comments =
                [
                    new Comment {
                        Id = 3,
                        PostId = 2,
                        Content = "I struggled with setup, but this helped.",
                        CreatedAt = DateTime.Now.AddDays(-7)
                    }
                ],
                Likes =
                [
                    new Like { Id = 3, UserId = 3, TargetId = 2, TargetType = "Post" }
                ]
            }
        ];

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
            Post? post = posts.FirstOrDefault(p => p.Id == id);
            if (post.IsNull())
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


        #region 點讚
        // 為文章點讚或取消點讚
        [HttpPost("{postId}/like")]
        public IActionResult ToggleLikePost(int postId, [FromQuery] int userId)
        {
            Post? post = posts.FirstOrDefault(p => p.Id == postId);
            if (post.IsNull())
            {
                return NotFound();
            }

            Like? existingLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
            if (existingLike.IsNotNull())
            {
                // 如果已經點讚，則取消點讚
                post.Likes.Remove(existingLike);
            }
            else
            {
                // 如果未點讚，則添加讚
                Like newLike = new()
                {
                    Id = post.Likes.Count > 0 ? post.Likes.Max(l => l.Id) + 1 : 1,
                    UserId = userId,
                    TargetId = postId,
                    TargetType = "Post"
                };
                post.Likes.Add(newLike);
            }

            return Ok(new { LikesCount = post.Likes.Count, LikedBy = post.Likes.Select(l => l.UserId) });
        }

        // 為評論點讚或取消點讚
        [HttpPost("{postId}/comments/{commentId}/like")]
        public IActionResult ToggleLikeComment(int postId, int commentId, [FromQuery] int userId)
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

            Like? existingLike = comment.Likes.FirstOrDefault(l => l.UserId == userId);
            if (existingLike.IsNotNull())
            {
                // 如果已經點讚，則取消點讚
                comment.Likes.Remove(existingLike);
            }
            else
            {
                // 如果未點讚，則添加讚
                Like newLike = new()
                {
                    Id = comment.Likes.Count > 0 ? comment.Likes.Max(l => l.Id) + 1 : 1,
                    UserId = userId,
                    TargetId = commentId,
                    TargetType = "Comment"
                };
                comment.Likes.Add(newLike);
            }

            return Ok(new { LikesCount = comment.Likes.Count, LikedBy = comment.Likes.Select(l => l.UserId) });
        }
        #endregion
    }
}
