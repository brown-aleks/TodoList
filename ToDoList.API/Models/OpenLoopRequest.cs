namespace ToDoList.API.Models
{
    public class OpenLoopRequest
    {
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string СompletDate { get; set; } = DateTimeOffset.UtcNow.ToString();
        public bool Сomplet { get; set; }
    }
}