using Microsoft.EntityFrameworkCore;

namespace ToDoList.API.Data
{
    public sealed class OpenLoopDbContext : DbContext
    {
        public DbSet<OpenLoop> OpenLoop { get; set; }
        public OpenLoopDbContext(DbContextOptions<OpenLoopDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}