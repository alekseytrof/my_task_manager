using Microsoft.EntityFrameworkCore;
using MyTaskManager.Api.Models.Abstractions;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Common.Models;

namespace MyTaskManager.Api.Models.Services
{
    public class ProjectsService : AbstractionService, ICommonService<ProjectDto>
    {
        private readonly ApplicationContext _db;

        public ProjectsService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(ProjectDto model)
        {
            var result = DoAction(delegate
            {
                Project newProject = new Project(model);
                _db.Project.Add(newProject);
                _db.SaveChanges();

            });
            return result;
        }

        public bool Delete(int id)
        {
            var result = DoAction(delegate
            {
                Project project = _db.Project.FirstOrDefault(p => p.Id == id);
                _db.Project.Remove(project);
                _db.SaveChanges();

            });
            return result;
        }

        public bool Update(int id, ProjectDto model)
        {
            var result = DoAction(delegate
            {
                Project project = _db.Project.FirstOrDefault(p => p.Id == id);

                project.Name = model.Name;
                project.Description = model.Description;
                project.Status = model.Status;
                project.AdminId = model.AdminId;
                project.Photo = model.Photo;

                _db.Project.Update(project);
                _db.SaveChanges();
            });
            return result;
        }

        public ProjectDto Get(int id)
        {
            Project project = _db.Project.FirstOrDefault(p => p.Id == id);
            return project?.ToProjectDto();
        }

        public async Task<IEnumerable<ProjectDto>> GetByUserId(int userId)
        {
            List<ProjectDto> result = new List<ProjectDto>();
            var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == userId);
            if (admin != null)
            {
                var projectsForUser = await _db.Project.Where(p => p.AdminId == admin.Id).Select(p => p.ToProjectDto()).ToListAsync();
                result.AddRange(projectsForUser);
            }
            var projectForUser = await _db.Project.Include(p => p.AllUsers).Where(p => p.AllUsers.Any(u => u.Id == userId)).Select(p => p.ToProjectDto()).ToListAsync();
            result.AddRange(projectForUser);
            return result;
        }

        public IQueryable<ProjectDto> GetAll()
        {
            return _db.Project.Select(p => p.ToProjectDto());
        }

        public void AddUserToProject(int id, List<int> userIds)
        {
            Project project = _db.Project.FirstOrDefault(p => p.Id == id);
            foreach (var userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                project.AllUsers.Add(user);
            }
            _db.SaveChanges();
        }

        public void RemoveUsersFromProject(int id, List<int> userIds)
        {
            Project project = _db.Project.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);
            foreach (var userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.AllUsers.Contains(user))
                {
                    project.AllUsers.Remove(user);
                }
            }
            _db.SaveChanges();
        }
    }
}
