﻿using Microsoft.AspNetCore.Identity;

namespace ToDoList.API.Models
{
    public class User : IdentityUser
    {
        public string? DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? RefreshToken { get; set; }
    }
}