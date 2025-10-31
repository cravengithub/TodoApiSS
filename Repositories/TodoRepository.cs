using Microsoft.EntityFrameworkCore;
using TodoApiSS.Data; // Lokasi DbContext Anda
using TodoApiSS.Interfaces; // Lokasi Interface
using TodoApiSS.Models;
namespace TodoApiSS.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        public TodoRepository(TodoContext context) { _context = context; }
        public async Task<IEnumerable<TodoItem>> GetAllAsync(int userId)
        {
            // HANYA ambil TodoItems yang UserId-nya cocok
            return await _context.TodoItems
            .Where(t => t.UserId == userId)
            .ToListAsync();
        }
        public async Task<TodoItem?> GetByIdAsync(long id, int userId)
        {
            // HANYA ambil item jika ID DAN UserId cocok
            return await _context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }
        public async Task AddAsync(TodoItem todoItem)
        {
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(TodoItem todoItem)
        {
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }
    }
}