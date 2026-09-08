using MyTaskManager.Common.Models;
using Newtonsoft.Json;

namespace MyTaskManager.Api.Models
{
    public class Desk : CommonObject
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        public int AdminId { get; set; }
        public User Admin { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<TaskModel>? Tasks { get; set; } = new List<TaskModel>();

        public Desk() { }

        public Desk(DeskDto dto) : base(dto)
        {
            Id = dto.Id;
            AdminId = dto.AdminId;
            IsPrivate = dto.IsPrivate;
            AdminId = dto.AdminId;
            ProjectId = dto.ProjectId;
            if (dto.Columns.Any())
            {
                Columns = JsonConvert.SerializeObject(dto.Columns);
            }
        }

        public DeskDto ToDeskDto()
        {
            return new DeskDto()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                AdminId = this.AdminId,
                Photo = this.Photo,
                IsPrivate = this.IsPrivate,
                Columns = JsonConvert.DeserializeObject<string[]>(this.Columns),
                ProjectId = this.ProjectId
            };
        }
    }
}
