using TodoApiSS.Models;
namespace TodoApiSS.Interfaces
{
    // Interface ini mendefinisikan kontrak untuk operasi data
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(int userId); // <-- Modifikasi
        Task<TodoItem?> GetByIdAsync(long id, int userId); // <-- Modifikasi
        Task AddAsync(TodoItem todoItem); // Add tidak berubah, karena item sudah berisi UserId
        Task UpdateAsync(TodoItem todoItem); // Update tidak berubah
        Task DeleteAsync(TodoItem todoItem); // Delete tidak berubah
        // Task<bool> ExistsAsync(long id); (Dihapus untuk kesederhanaan, kita cek di service)
    }
}