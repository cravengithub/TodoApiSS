using TodoApiSS.DTOs;
using TodoApiSS.Interfaces;
using TodoApiSS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApiSS.Services
{
    /// <summary>
    /// Implementasi Service Layer untuk User.
    /// Menangani logika bisnis dan pemetaan antara User (Model) dan UserDTO.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            // Ubah setiap 'User' (model) menjadi 'UserDTO'
            return users.Select(user => MapUserToDto(user));
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return MapUserToDto(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto)
        {
            // PERINGATAN: 'UserDTO' dari prompt Anda tidak memiliki 'Password'.
            // User yang dibuat dengan metode ini tidak akan bisa login.
            // Metode ini lebih cocok untuk panel Admin di mana password
            // di-set secara terpisah atau default.
            // Pendaftaran (register) harus tetap ditangani oleh AuthController
            // yang dapat melakukan hashing password.

            // 1. Periksa duplikat username
            if (await _userRepository.GetByUsernameAsync(userDto.Username) != null)
            {
                throw new System.Exception("Username sudah digunakan.");
            }

            // 2. Map DTO ke Model
            var user = new User
            {
                Username = userDto.Username,
                Name = userDto.Name,
                Address = userDto.Address,
                Phone = userDto.Phone,
                Email = userDto.Email,
                NoTin = userDto.NoTin
                // PasswordHash dan PasswordSalt akan kosong (null)
            };

            // 3. Simpan ke database
            await _userRepository.AddAsync(user);

            // 4. Kembalikan DTO asli (karena DTO tidak memiliki ID)
            // 'user.Id' sekarang terisi oleh EF Core, tapi DTO tidak bisa menampungnya.
            return userDto;
        }

        public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false; // User tidak ditemukan
            }

            // Periksa duplikat username (jika username diubah)
            if (user.Username != userDto.Username &&
                await _userRepository.GetByUsernameAsync(userDto.Username) != null)
            {
                throw new System.Exception("Username baru sudah digunakan oleh user lain.");
            }

            // Map field dari DTO ke model yang ada
            user.Username = userDto.Username;
            user.Name = userDto.Name;
            user.Address = userDto.Address;
            user.Phone = userDto.Phone;
            user.Email = userDto.Email;
            user.NoTin = userDto.NoTin;
            // Kita tidak menyentuh PasswordHash/Salt

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false; // User tidak ditemukan
            }

            await _userRepository.DeleteAsync(user);
            return true;
        }

        // --- Helper Pemetaan (Mapping) ---

        /// <summary>
        /// Helper untuk mengubah Model 'User' menjadi 'UserDTO'.
        /// </summary>
        private UserDTO MapUserToDto(User user)
        {
            return new UserDTO
            {
                // DTO (sesuai prompt) tidak memiliki ID,
                // jadi kita hanya memetakan field yang ada di DTO.
                Username = user.Username,
                Name = user.Name,
                Address = user.Address,
                Phone = user.Phone,
                Email = user.Email,
                NoTin = user.NoTin
            };
        }
    }
}
