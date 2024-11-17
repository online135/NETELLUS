using Microsoft.AspNetCore.Mvc;
using NETELLUS.Models;
using System.Security.Claims;

namespace NETELLUS.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 獲取目前使用者的 Profile 資訊
        [HttpGet("Profile")]
        public IActionResult GetMyProfile()
        {
            // 假設有取得目前使用者 ID 的方法
            // int userId = GetCurrentUserId();
            //var profile = await _context.UserProfiles
            //                            .Include(p => p.User)
            //                            .FirstOrDefaultAsync(p => p.UserId == userId);

            UserProfile userProfile = new UserProfile
            {
                UserId = 1,
                PhoneNumber = "0937338506",
                BirthDate = DateTime.Now
            };

            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        //// 更新 Profile 資訊
        //[HttpPut("me")]
        //public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfile updatedProfile)
        //{
        //    int userId = GetCurrentUserId();
        //    var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

        //    if (profile == null)
        //        return NotFound();

        //    // 更新資料
        //    profile.BirthDate = updatedProfile.BirthDate;
        //    profile.PhoneNumber = updatedProfile.PhoneNumber;
        //    profile.Address = updatedProfile.Address;
        //    profile.ProfilePictureUrl = updatedProfile.ProfilePictureUrl;

        //    _context.UserProfiles.Update(profile);
        //    await _context.SaveChangesAsync();

        //    return Ok(profile);
        //}

        // Helper 方法來獲取目前使用者 ID
        private int GetCurrentUserId()
        {
            return int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
