using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    /// <summary>
    /// Модель для отзыва токена
    /// </summary>
    public class RevokeRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }

}