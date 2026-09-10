using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyTaskManager.Api.Models.Abstractions;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MyTaskManager.Api.Models.Services
{
    public class TasksService : AbstractionService, ICommonService<TaskDto>
    {
        private readonly ApplicationContext _db;

        public TasksService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(TaskDto model)
        {
            return DoAction(delegate
            {
                TaskModel newTask = new TaskModel(model);
                _db.Tasks.Add(newTask);
                _db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(delegate
            {
                TaskModel task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                _db.Tasks.Remove(task);
                _db.SaveChanges();
            });
        }

        public TaskDto Get(int id)
        {
            TaskModel task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            return task?.ToDto();
        }

        public IQueryable<TaskDto> GetTaskForUser(int userId)
        {
            return _db.Tasks.Where(t => t.CreatorId == userId || t.ExecutorId == userId).Select(t => t.ToDto());
        }

        public bool Update(int id, TaskDto model)
        {
            return DoAction(delegate
            {
                TaskModel task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                task.Name = model.Name;
                task.Description = model.Description;
                task.Photo = model.Photo;
                task.StartDate = model.StartDate;
                task.EndDate = model.EndDate;
                task.File = model.File;
                task.DeskId = model.DeskId;
                task.Column = model.Column;
                task.CreatorId = model.CreatorId;
                task.ExecutorId = model.ExecutorId;

                _db.Tasks.Update(task);
                _db.SaveChanges();
            });
        }

        public IQueryable<TaskDto> GetAll(int deskId)
        {
            return _db.Tasks.Where(t => t.DeskId == deskId).Select(t => t.ToShortDto());
        }
    }
}
