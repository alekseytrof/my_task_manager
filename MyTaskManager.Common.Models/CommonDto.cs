namespace MyTaskManager.Common.Models
{
    public abstract class CommonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[]? Photo { get; set; }

        public CommonDto()
        {
            CreationDate = DateTime.Now;
        }
    }
}
