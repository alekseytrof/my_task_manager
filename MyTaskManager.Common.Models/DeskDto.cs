namespace MyTaskManager.Common.Models
{
    public class DeskDto : CommonDto
    {
        public bool IsPrivate { get; set; }
        public string[] Columns { get; set; }
        public int AdminId { get; set; }
        public int ProjectId { get; set; }
        public List<int>? TasksIds { get; set; }

        public DeskDto() { }

        public DeskDto(string name, string description, bool isPrivate, string[] columns, int adminId, int projectId)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;
            Columns = columns;
            AdminId = adminId;
            ProjectId = projectId;
        }
    }
}
