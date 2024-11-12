using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
                UpdatedAt = DateTime.Now.AddDays(-10)
            },
            new() {
                Id = 2,
                Title = "Getting Started with ASP.NET Core",
                Content = "ASP.NET Core is a cross-platform framework for building modern, cloud-based web applications.",
                CreatedAt = DateTime.Now.AddDays(-8),
                UpdatedAt = DateTime.Now.AddDays(-8)
            },
            new() {
                Id = 3,
                Title = "Understanding Dependency Injection",
                Content = "Dependency Injection (DI) is a design pattern used in ASP.NET Core for managing class dependencies.",
                CreatedAt = DateTime.Now.AddDays(-5),
                UpdatedAt = DateTime.Now.AddDays(-5)
            },
            new() {
                Id = 4,
                Title = "Exploring Entity Framework Core",
                Content = "Entity Framework Core is a modern Object-Relational Mapper (ORM) for .NET applications.",
                CreatedAt = DateTime.Now.AddDays(-3),
                UpdatedAt = DateTime.Now.AddDays(-2)
            },
            new() {
                Id = 5,
                Title = "Building REST APIs with ASP.NET Core",
                Content = "Learn to build robust and scalable REST APIs using ASP.NET Core Web API.",
                CreatedAt = DateTime.Now.AddDays(-1),
                UpdatedAt = DateTime.Now
            }
        };


        // 獲取所有文章
        [HttpGet]

        // 獲取特定文章
        [HttpGet("{id}")]

        // 創建新文章
        [HttpPost]

        // 更新文章
        [HttpPut("{id}")]

        // 刪除文章
        [HttpDelete("{id}")]
    }
}
