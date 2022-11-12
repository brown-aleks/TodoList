namespace ToDoList.WebAPI.Models
{
    public class OpenLoop
    {
        public Guid Id { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? СompletDate { get; set; }
        public bool Сomplet { get; set; }
    }
}