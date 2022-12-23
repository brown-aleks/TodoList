using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    /// <summary>
    /// Модель запроса на обновление токинов.
    /// </summary>
    public class RefreshRequest
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}