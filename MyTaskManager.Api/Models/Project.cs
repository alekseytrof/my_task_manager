using MyTaskManager.Common.Models;

namespace MyTaskManager.Api.Models
{
    public class Project : CommonObject
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public ProjectAdmin Admin { get; set; }
        public ProjectStatus Status { get; set; }
        public List<User> AllUsers { get; set; } = new List<User>();
        public List<Desk> AllDesks { get; set; } = new List<Desk>();

        public Project() { }

        public Project(ProjectDto dto) : base(dto)
        {
            Id = dto.Id;
            AdminId = dto.AdminId;
            Status = dto.Status;
        }

        public ProjectDto ToProjectDto()
        {
            return new ProjectDto()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                Status = this.Status,
                AdminId = this.AdminId,
                Photo = this.Photo
            };
        }
    }
}