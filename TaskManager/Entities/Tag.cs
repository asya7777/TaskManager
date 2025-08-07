namespace TaskManager.Entities
{
    public class Tag
    {
        public int tagId { get; set; }
        public string tagName { get; set; }

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
