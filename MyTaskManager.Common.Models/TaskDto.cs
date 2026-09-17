namespace MyTaskManager.Common.Models
{
    public class TaskDto : CommonDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[]? File { get; set; }
        public int DeskId { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public int? ExecutorId { get; set; }

        public TaskDto() { }

        public TaskDto(string name, string description, DateTime start, DateTime end, int deskId, string column, int creatorId)
        {
            Name = name;
            Description = description;
            StartDate = start;
            EndDate = end;
            DeskId = deskId;
            Column = column;
        }
    }
}
