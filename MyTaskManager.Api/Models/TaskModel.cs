using MyTaskManager.Common.Models;

namespace MyTaskManager.Api.Models
{
    public class TaskModel : CommonObject
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[]? File { get; set; }
        public int DeskId { get; set; }
        public Desk Desk { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public User Creator { get; set; }
        public int? ExecutorId { get; set; }

        public TaskModel() { }

        public TaskModel(TaskDto taskDto) : base(taskDto)
        {
            Id = taskDto.Id;
            StartDate = taskDto.StartDate;
            EndDate = taskDto.EndDate;
            File = taskDto.File;
            DeskId = taskDto.DeskId;
            Column = taskDto.Column;
            CreatorId = taskDto.CreatorId;
            ExecutorId = taskDto.ExecutorId;
        }

        public TaskDto ToDto()
        {
            return new TaskDto()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                File = this.File,
                DeskId = this.DeskId,
                Column = this.Column,
                CreatorId = this.CreatorId,
                ExecutorId = this.ExecutorId
            };
        }
    }
}
