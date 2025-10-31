using TodoApiSS.Interfaces; // Menggunakan interface repository
using TodoApiSS.Models;
namespace TodoApiSS.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        public TodoService(ITodoRepository todoRepository) { _todoRepository = todoRepository; }
        public async Task<IEnumerable<TodoItem>> GetTodosAsync(int userId)
        {
            return await _todoRepository.GetAllAsync(userId);
        }
        public async Task<TodoItem?> GetTodoByIdAsync(long id, int userId)
        {
            // Kueri sudah aman (difilter oleh repo)
            return await _todoRepository.GetByIdAsync(id, userId);
        }
        public async Task<TodoItem> CreateTodoAsync(TodoItem todoItem, int userId)
        {
            // ... (Logika bisnis validasi nama, dll.) ...
            // === KEAMANAN PENTING ===
            // Tetapkan kepemilikan item berdasarkan pengguna yang login
            todoItem.UserId = userId;
            // =========================
            await _todoRepository.AddAsync(todoItem);
            return todoItem;
        }
        public async Task<bool> UpdateTodoAsync(long id, TodoItem todoItem, int userId)
        {
            if (id != todoItem.Id)
            {
                return false; // Bad Request
            }
            // === KEAMANAN PENTING ===
            // Cek apakah item ini ada DAN milik pengguna ini
            var existingItem = await _todoRepository.GetByIdAsync(id, userId);
            if (existingItem == null)
            {
                return false; // Not Found (atau Not Authorized)
            }
            // =========================
            existingItem.Name = todoItem.Name;
            existingItem.IsComplete = todoItem.IsComplete;
            await _todoRepository.UpdateAsync(existingItem);
            return true;
        }
        public async Task<bool> DeleteTodoAsync(long id, int userId)
        {
            // === KEAMANAN PENTING ===
            // Cek apakah item ini ada DAN milik pengguna ini
            var todoItem = await _todoRepository.GetByIdAsync(id, userId);
            if (todoItem == null)
            {
                return false; // Not Found (atau Not Authorized)
            }
            // =========================
            await _todoRepository.DeleteAsync(todoItem);
            return true;
        }
    }
}