using Microsoft.AspNetCore.Mvc;
using NETELLUS.Extensions;
using NETELLUS.Models;

namespace NETELLUS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // 模擬用戶數據列表
        private static List<User> users = new List<User>();
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}
