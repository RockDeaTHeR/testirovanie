namespace testirovanie.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Term_Id { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public string commandName { get; set; }
        public int? Val1 { get; set; } = 0;
        public int? Val2 { get; set; } = 0;
        public int? Val3 { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
    }
}
