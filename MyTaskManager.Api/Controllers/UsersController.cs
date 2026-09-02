using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTaskManager.Api.Models;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Api.Models.Services;
using MyTaskManager.Common.Models;

namespace MyTaskManager.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;

        public UsersController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return await _db.Users.Select(u => u.ToUserDto()).ToArrayAsync();
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userModel)
        {
            if (userModel != null)
            {
                bool result = _userService.Create(userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userModel)
        {
            if (userModel != null)
            {
                bool result = _userService.Update(id, userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool result = _userService.Delete(id);
            return result ? Ok() : NotFound();
        }

        [HttpPost("all")]
        public async Task<IActionResult> CreateMultiplyUsers([FromBody] List<UserDto> users)
        {
            if (users != null && users.Count > 1)
            {
                bool result = _userService.CreateMultiplyUsers(users);
                return result ? Ok() : BadRequest();
            }
            return BadRequest();
        }
    }
}
