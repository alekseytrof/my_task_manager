using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Api.Models.Services;
using MyTaskManager.Common.Models;


namespace MyTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly DesksService _desksService;

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _desksService = new DesksService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<CommonDto>> GetDesksForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var desks = await _desksService.GetAll(user.Id).ToListAsync();
                return desks;
            }
            return Array.Empty<CommonDto>();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = _desksService.Get(id);
            return desk == null ? NotFound() : Ok(desk);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IEnumerable<CommonDto>> GetProjectDesks(int projectId)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                return await _desksService.GetProjectDesks(projectId, user.Id).ToArrayAsync();
            }
            return Array.Empty<CommonDto>();
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeskDto deskDto)
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskDto deskDto)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
