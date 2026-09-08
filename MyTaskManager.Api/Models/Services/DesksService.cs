using Microsoft.EntityFrameworkCore;
using MyTaskManager.Api.Models.Abstractions;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;

namespace MyTaskManager.Api.Models.Services
{
    public class DesksService : AbstractionService, ICommonService<DeskDto>
    {
        private readonly ApplicationContext _db;

        public DesksService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(DeskDto model)
        {
            return DoAction(delegate
            {
                Desk newDesk = new Desk(model);
                _db.Desks.Add(newDesk);
                _db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(delegate
            {
                Desk desk = _db.Desks.FirstOrDefault(d => d.Id == id);
                _db.Desks.Remove(desk);
                _db.SaveChanges();
            });
        }

        public DeskDto Get(int id)
        {
            Desk desk = _db.Desks.Include(d => d.Tasks).FirstOrDefault(d => d.Id == id);
            var deskDto = desk?.ToDeskDto();
            if (deskDto != null)
            {
                deskDto.TasksIds = desk.Tasks.Select(t => t.Id).ToList();
            }
            return deskDto;
        }

        public bool Update(int id, DeskDto model)
        {
            return DoAction(delegate
            {
                Desk desk = _db.Desks.FirstOrDefault(d => d.Id == id);
                desk.Name = model.Name;
                desk.Description = model.Description;
                desk.AdminId = model.AdminId;
                desk.Photo = model.Photo;
                desk.IsPrivate = model.IsPrivate;
                desk.Columns = JsonConvert.SerializeObject(model.Columns);
                _db.Desks.Update(desk);
                _db.SaveChanges();
            });
        }

        public IQueryable<CommonDto> GetAll(int userId)
        {
            return _db.Desks.Where(d => d.AdminId == userId).Select(d => d.ToDeskDto() as CommonDto);
        }

        public IQueryable<CommonDto> GetProjectDesks(int projectId, int userId)
        {
            return _db.Desks
                .Where(d => d.ProjectId == projectId && (d.IsPrivate == false || d.AdminId == userId))
                .Select(d => d.ToDeskDto() as CommonDto);
        }
    }
}
