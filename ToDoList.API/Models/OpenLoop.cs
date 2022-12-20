using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ToDoList.API.Models
{
    public class OpenLoop
    {
        public Guid Id { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; }
        public DateTime СompletDateUtc { get; set; }
        public bool Сomplet { get; set; }
    }
}