namespace TaskManager.Entities
{
    public class User
    {
        public int usrId { get; set; }
        public string nickname { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public byte[] passwordHash { get; set; } //hashed password
        public byte[] passwordSalt { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();//bir user ın birden fazla task ı olabilir
    }
}
