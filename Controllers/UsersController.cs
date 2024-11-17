using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NETELLUS.Extensions;
using NETELLUS.Models;
using NETELLUS.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NETELLUS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        // 模擬用戶數據列表
        private static readonly List<User> users = [];
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 用戶註冊
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto registerDto)
        {
            // 檢查用戶名稱或者信箱是否已被使用
            if (users.Any(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                return BadRequest("Username or email already exists.");
            }

            // 創建新用戶，密碼應經過加密處理
            var newUser = new User
            {
                Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1,
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password) // 使用Bcrupt 進行密碼 Hash
            };

            users.Add(newUser);
            return Ok(new { Message = "User registered successfully." });
        }

        // 用戶登陸
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = users.FirstOrDefault(u => u.Username == loginDto.Username);
            if (user.IsNull() || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // 生成 JWT Token
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // 生成 JWT Token
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Username),
                new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                new Claim(type: "UserId", value: user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
