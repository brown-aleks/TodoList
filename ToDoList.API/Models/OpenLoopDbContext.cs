using Microsoft.EntityFrameworkCore;

namespace ToDoList.API.Models
{
    public class OpenLoopDbContext : DbContext
    {
        public DbSet<OpenLoop> OpenLoop { get; set; }
        public OpenLoopDbContext(DbContextOptions<OpenLoopDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}