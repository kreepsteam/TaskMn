namespace TaskMn.Models
{
    public class ChangeHistory
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string ChangeType { get; set; }
        public DateTime ChangeTime { get; set; } = DateTime.Now;
    }
}
