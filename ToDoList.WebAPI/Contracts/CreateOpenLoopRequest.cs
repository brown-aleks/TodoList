namespace ToDoList.WebAPI.Contracts
{
    public class CreateOpenLoopRequest
    {
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset СompletDate { get; set; }
    }
}