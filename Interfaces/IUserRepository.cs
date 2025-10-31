using TodoApiSS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApiSS.Interfaces
{
    /// <summary>
    /// Interface untuk Data Access Layer (Repository) yang mengelola entitas User.
    /// Bekerja langsung dengan model 'User'.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Mendapatkan satu user berdasarkan ID.
        /// </summary>
        /// <param name="id">ID unik user.</param>
        /// <returns>Model 'User' atau null jika tidak ditemukan.</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Mendapatkan satu user berdasarkan Username.
        /// </summary>
        /// <param name="username">Username unik user.</param>
        /// <returns>Model 'User' atau null jika tidak ditemukan.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Mendapatkan semua user dari database.
        /// </summary>
        /// <returns>Koleksi 'User'.</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Menambahkan user baru ke database.
        /// </summary>
        /// <param name="user">Model 'User' yang akan ditambahkan.</param>
        Task AddAsync(User user);

        /// <summary>
        /// Memperbarui data user yang ada di database.
        /// </summary>
        /// <param name="user">Model 'User' dengan data yang sudah diperbarui.</param>
        Task UpdateAsync(User user);

        /// <summary>
        /// Menghapus user dari database.
        /// </summary>
        /// <param name="user">Model 'User' yang akan dihapus.</param>
        Task DeleteAsync(User user);

        /// <summary>
        /// Memeriksa apakah user ada berdasarkan ID.
        /// </summary>
        /// <param name="id">ID unik user.</param>
        /// <returns>True jika user ada.</returns>
        Task<bool> ExistsAsync(int id);
    }
}