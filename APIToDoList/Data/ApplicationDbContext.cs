using Microsoft.EntityFrameworkCore;
using APIToDoList.Model;
namespace APIToDoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TodoItem> Products { get; set; }
    }
}
