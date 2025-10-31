using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApiSS.Data;

public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
{
    public TodoContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TodoDb;User Id=sa;Password=PasswordR4hasia;TrustServerCertificate=True");

        return new TodoContext(optionsBuilder.Options);
    }
}
