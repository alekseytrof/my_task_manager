namespace MyTaskManager.Common.Models
{
    public class DeskDto : CommonDto
    {
        public bool IsPrivate { get; set; }
        public string[] Colums { get; set; }
        public int AdminId { get; set; }
        public int ProjectId { get; set; }
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}
