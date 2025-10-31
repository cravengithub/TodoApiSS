using TodoApiSS.Models;
namespace TodoApiSS.Interfaces
{
    // Interface ini mendefinisikan kontrak untuk logika bisnis
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetTodosAsync(int userId); // <-- Modifikasi
        Task<TodoItem?> GetTodoByIdAsync(long id, int userId); // <-- Modifikasi
        Task<TodoItem> CreateTodoAsync(TodoItem todoItem, int userId); // <-- Modifikasi
        Task<bool> UpdateTodoAsync(long id, TodoItem todoItem, int userId); // <-- Modifikasi
        Task<bool> DeleteTodoAsync(long id, int userId); // <-- Modifikasi
    }
}