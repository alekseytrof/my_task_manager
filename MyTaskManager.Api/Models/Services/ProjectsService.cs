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
    }
}
