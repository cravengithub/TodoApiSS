using TodoApiSS.DTOs;
using System.Threading.Tasks;
namespace TodoApiSS.Interfaces
{
    /// <summary>
    /// Interface untuk Service Layer (Logika Bisnis) yang mengelola data user.
    /// Bekerja dengan 'ProfileDTO'.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Mengambil satu data user sebagai DTO berdasarkan ID.
        /// </summary>
        Task<ProfileDTO?> GetUserByIdAsync(int id);
        /// <summary>
        /// Memperbarui data user berdasarkan ID.
        /// </summary>
        Task<bool> UpdateUserAsync(int id, ProfileDTO profileDto);
        /// <summary>
        /// Menghapus user berdasarkan ID.
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}