namespace MyTaskManager.Common.Models
{
    public class ProjectDto : CommonDto
    {
        public int? AdminId { get; set; }
        public ProjectStatus Status { get; set; }
        public List<int> AllUsersIds { get; set; }
        public List<int> AllDesksIds { get; set; }
    }
}
