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
        public async Task<IEnumerable<ProjectDto>> Get(ProjectDto projectDto)
        {
            return await _db.Project.Select(p => p.ToProjectDto()).ToArrayAsync();
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectDto projectDto)
        {
            if (projectDto != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var admin = _db.ProjectAdmins.FirstOrDefault(a => a.Id == user.Id);
                    if (admin == null)
                    {
                        admin = new ProjectAdmin(user);
                        _db.ProjectAdmins.Add(admin);
                    }
                    projectDto.Id = admin.Id;
                }

                bool result = _projectsService.Create(projectDto);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectDto projectDto)
        {
            if (projectDto != null)
            {
                bool result = _projectsService.Update(id, projectDto);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _projectsService.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}
