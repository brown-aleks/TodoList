namespace ToDoList.API.Contracts
{
    public class UpdateOpenLoopRequest
    {
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset? СompletDate { get; set; }
        public bool Сomplet { get; set; }
    }
}