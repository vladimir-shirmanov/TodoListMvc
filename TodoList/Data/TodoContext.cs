using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext()
        {
        }
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public virtual DbSet<TodoItem> TodoItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
