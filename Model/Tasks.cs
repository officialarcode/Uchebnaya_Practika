namespace API_UP2.Model
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public DateTime DateExecute { get; set; }
        public string Comment { get; set; }
        public bool Done { get; set; }
    }
}
