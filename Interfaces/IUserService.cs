using TodoApiSS.DTOs; // Menggunakan DTO yang Anda berikan
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApiSS.Interfaces
{
    /// <summary>
    /// Interface untuk Service Layer (Logika Bisnis) yang mengelola data user.
    /// Bekerja dengan 'UserDTO' untuk input dan output.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Mengambil semua data user sebagai DTO.
        /// </summary>
        /// <returns>Koleksi UserDTO.</returns>
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        /// <summary>
        /// Mengambil satu data user sebagai DTO berdasarkan ID.
        /// </summary>
        /// <param name="id">ID unik user.</param>
        /// <returns>UserDTO atau null jika tidak ditemukan.</returns>
        Task<UserDTO?> GetUserByIdAsync(int id);

        /// <summary>
        /// Membuat user baru berdasarkan data dari DTO.
        /// (Catatan: Ini untuk profil, AuthController menangani registrasi).
        /// </summary>
        /// <param name="userDto">DTO dengan data user baru.</param>
        /// <returns>UserDTO yang telah dibuat (termasuk ID baru).</returns>
        Task<UserDTO> CreateUserAsync(UserDTO userDto);

        /// <summary>
        /// Memperbarui data user berdasarkan ID.
        /// </summary>
        /// <param name="id">ID user yang akan diperbarui.</param>
        /// <param name="userDto">DTO dengan data baru.</param>
        /// <returns>True jika update berhasil, false jika user tidak ditemukan.</returns>
        Task<bool> UpdateUserAsync(int id, UserDTO userDto);

        /// <summary>
        /// Menghapus user berdasarkan ID.
        /// </summary>
        /// <param name="id">ID user yang akan dihapus.</param>
        /// <returns>True jika hapus berhasil, false jika user tidak ditemukan.</returns>
        Task<bool> DeleteUserAsync(int id);
    }
}