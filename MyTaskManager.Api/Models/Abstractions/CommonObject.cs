namespace MyTaskManager.Api.Models
{
    public abstract class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Photo { get; set; }

        public CommonObject()
        {
            CreationDate = DateTime.Now;
        }
    }
}
