using Microsoft.EntityFrameworkCore;
using TodoApiSS.Data;
using TodoApiSS.Interfaces;
using TodoApiSS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApiSS.Repositories
{
    /// <summary>
    /// Implementasi Repository untuk entitas User.
    /// Bertanggung jawab atas semua interaksi database (CRUD) untuk User.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext _context;

        public UserRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            // Gunakan FirstOrDefaultAsync untuk mencari berdasarkan username
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}

