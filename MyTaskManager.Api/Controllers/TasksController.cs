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
    public class TasksController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly TasksService _tasksService;

        public TasksController(ApplicationContext db)
        {
            _usersService = new UsersService(db);
            _tasksService = new TasksService(db);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByDesk(int deskId)
        {
            var result = await _tasksService.GetAll(deskId).ToListAsync();
            return result == null ? NoContent() : Ok(result);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var result = await _tasksService.GetTaskForUser(user.Id).ToListAsync();
                return result == null ? NoContent() : Ok(result);
            }
            return Unauthorized(Array.Empty<TaskDto>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = _tasksService.Get(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskDto taskDto)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (taskDto != null)
                {
                    taskDto.CreatorId = user.Id;
                    bool result = _tasksService.Create(taskDto);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskDto taskDto)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (taskDto != null)
                {
                    bool result = _tasksService.Update(id, taskDto);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _tasksService.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}
