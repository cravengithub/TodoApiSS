using Microsoft.EntityFrameworkCore;
using TodoApiSS.Models;

namespace TodoApiSS.Data
{
    // Warisi (inherit) dari DbContext
    public class TodoContext : DbContext
    {
        // Constructor ini wajib ada untuk Dependency Injection
        public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
        {
        }
        // Properti DbSet ini akan dipetakan ke tabel bernama "TodoItems"
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        // Konfigurasi model dan relasi disarankan ditempatkan di sini
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurasi relasi TodoItem -> User
            modelBuilder.Entity<TodoItem>(entity =>
            {
                // Set up foreign key relationship: TodoItem.UserId -> User.Id
                entity.HasOne(t => t.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(t => t.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
