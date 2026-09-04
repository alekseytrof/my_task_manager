using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTaskManager.Api.Models;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Api.Models.Services;
using MyTaskManager.Common.Models;

namespace MyTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly ProjectsService _projectsService;

        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _projectsService = new ProjectsService(db);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<ProjectDto>> Get(ProjectDto projectDto)
        {
            return await _db.Project.Select(p => p.ToProjectDto()).ToArrayAsync();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var project = _projectsService.Get(id);
            return project == null ? NoContent() : Ok(project);
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectDto>> Get()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user.Status == UserStatus.Admin)
            {
                return await _projectsService.GetAll().ToListAsync();
            }
            else
            {
                return await _projectsService.GetByUserId(user.Id);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectDto projectDto)
        {
            if (projectDto != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == user.Id);
                        if (admin == null)
                        {
                            admin = new ProjectAdmin(user);
                            _db.ProjectAdmins.Add(admin);
                        }
                        projectDto.AdminId = admin.Id;

                        bool result = _projectsService.Create(projectDto);
                        return result ? Ok() : NotFound();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectDto projectDto)
        {
            if (projectDto != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool result = _projectsService.Update(id, projectDto);
                        return result ? Ok() : NotFound();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                {
                    bool result = _projectsService.Delete(id);
                    return result ? Ok() : NotFound();
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUserToProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectsService.AddUserToProject(id, usersIds);
                        return Ok();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [HttpPatch("{id}/users/remove")]
        public IActionResult RemoveUsersFromProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectsService.RemoveUsersFromProject(id, usersIds);
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }
    }
}
