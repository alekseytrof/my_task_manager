using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyTaskManager.Api.Models;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Api.Models.Services;
using MyTaskManager.Common.Models;
using System.IdentityModel.Tokens.Jwt;

namespace MyTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _userService;

        public AccountController(ApplicationContext db)
        {
            _db = db;
            _userService = new UsersService(db);
        }

        [Authorize]
        [HttpGet("info")]
        public IActionResult GetCurrentuserInfo()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == userName);

            if (user != null)
            {
                return Ok(user.ToUserDto());
            }
            return NotFound();
        }

        [HttpPost("token")]
        public IActionResult GetToken()
        {
            var userDate = _userService.GetUserLoginPassFromBasicAuth(Request);
            var login = userDate.Item1;
            var pass = userDate.Item2;

            var identity = _userService.GetIdentity(login, pass);

            if (identity == null)
            {
                return BadRequest("Неправильный логин или пароль");
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("update")]
        public IActionResult UpdateUser([FromBody] UserDto userModel)
        {
            if (userModel != null)
            {
                string userName = HttpContext.User.Identity.Name;
                User userForUpdate = _db.Users.FirstOrDefault(u => u.Email == userName);

                if (userForUpdate != null)
                {
                    userForUpdate.Password = userModel.Password;
                    userForUpdate.Phone = userModel.Phone;
                    userForUpdate.FirstName = userModel.FirstName;
                    userForUpdate.LastName = userModel.LastName;
                    userForUpdate.Photo = userModel.Photo;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                    return Ok();
                }
                return NotFound("Пользователь не найден");
            }
            return BadRequest();
        }
    }
}
