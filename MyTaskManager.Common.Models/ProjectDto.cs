namespace MyTaskManager.Common.Models
{
    public class ProjectDto : CommonDto
    {
        public int? AdminId { get; set; }
        public ProjectStatus Status { get; set; }
        public List<UserDto> AllUsers { get; set; } = new List<UserDto>();
        public List<DeskDto> AllDesks { get; set; } = new List<DeskDto>();
    }
}
