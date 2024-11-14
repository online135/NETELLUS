using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NETELLUS.Extensions;
using NETELLUS.Models;
using System.Xml.Linq;

namespace NETELLUS.Controllers
{
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
                UpdatedAt = DateTime.Now.AddDays(-10)
            },
            new() {
                Id = 2,
                Title = "Getting Started with ASP.NET Core",
                Content = "ASP.NET Core is a cross-platform framework for building modern, cloud-based web applications.",
                CreatedAt = DateTime.Now.AddDays(-8),
                UpdatedAt = DateTime.Now.AddDays(-8)
            }
        ];


        // 以標準的 restfulAPI 而言, 不需要標明主題, 除非難以辨認或特別有需要
        // 所以 不是用 GetAllPost, 而是 GetAll, 以下都雷同
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
            // Where 是拉出多個的時候, 但PK不需要, 下面的 更新迴圈那裏概念亦同
            // 個人討厭 var
            Post? post = posts.FirstOrDefault(p => p.Id == id);

            // 這是 extension 用法, 可以看一下自寫小功能, 通常會小除錯一下
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
            newPost.Id = posts.Count > 0 ? posts.Max(p => p.Id) + 1 : 1; // 這行未來會刪掉, 改用DB來做到效果
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
    }
}