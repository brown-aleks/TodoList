namespace TodoList
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "NoName";
        public string? Email { get; set; }
    }
}