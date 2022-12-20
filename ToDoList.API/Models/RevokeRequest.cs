using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    public class RevokeRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }

}