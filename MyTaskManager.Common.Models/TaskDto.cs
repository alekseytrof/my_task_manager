namespace MyTaskManager.Common.Models
{
    public class TaskDto : CommonDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] File { get; set; }
        public int DeskId { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public int? ExecutorId { get; set; }
    }
}
