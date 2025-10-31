using TodoApiSS.DTOs; // <-- Menggunakan ProfileDTO
using TodoApiSS.Interfaces; // <-- Mengimplementasikan IProfileService
using TodoApiSS.Models; // <-- Menggunakan Model User (internal)
using System;
using System.Threading.Tasks;
namespace TodoApiSS.Services
{
    /// <summary>
    /// Implementasi Service Layer untuk User.
    /// Menangani logika bisnis dan pemetaan antara User (Model) dan ProfileDTO.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;
        // Inject repository
        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Mengambil satu data user sebagai DTO berdasarkan ID.
        /// </summary>
        public async Task<ProfileDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            // Map dari Model (User) ke DTO (ProfileDTO)
            return MapUserToProfileDto(user);
        }
        /// <summary>
        /// Memperbarui data user berdasarkan ID.
        /// </summary>
        public async Task<bool> UpdateUserAsync(int id, ProfileDTO profileDto)
        {
            // 1. Ambil model User yang ada dari database
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false; // User tidak ditemukan
            }
            // 2. Logika Bisnis: Cek duplikat username (jika username diubah)
            if (user.Username != profileDto.Username &&
            await _userRepository.GetByUsernameAsync(profileDto.Username) != null)
            {
                // Lempar exception yang akan ditangkap oleh Controller (dan dikirim sebagai BadRequest)
                throw new System.Exception("Username baru sudah digunakan oleh user lain.");
            }
            // 3. Map field dari DTO (ProfileDTO) ke Model (User) yang ada
            MapProfileDtoToUser(profileDto, user);
            // Catatan: Kita tidak menyentuh PasswordHash/Salt saat update profil
            // 4. Simpan perubahan ke database
            await _userRepository.UpdateAsync(user);
            return true;
        }
        /// <summary>
        /// Menghapus user berdasarkan ID.
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false; // User tidak ditemukan
            }
            // (Catatan: Seharusnya kita juga menghapus TodoItems terkait,
            // tapi kita asumsikan database di-set 'On Cascade Delete'
            // atau ditangani oleh repository jika diperlukan)
            await _userRepository.DeleteAsync(user);
            return true;
        }
        // --- Helper Pemetaan (Mapping) ---
        /// <summary>
        /// Helper pribadi untuk mengubah Model 'User' menjadi 'ProfileDTO'.
        /// </summary>
        private ProfileDTO MapUserToProfileDto(User user)
        {
            return new ProfileDTO
            {
                Username = user.Username,
                Name = user.Name ?? string.Empty,
                Address = user.Address ?? string.Empty,
                Phone = user.Phone ?? string.Empty,
                Email = user.Email ?? string.Empty,
                NoTin = user.NoTin ?? string.Empty
            };
        }
        /// <summary>
        /// Helper pribadi untuk memetakan data DARI 'ProfileDTO' KE Model 'User'.
        /// </summary>
        private void MapProfileDtoToUser(ProfileDTO dto, User user)
        {
            // Memperbarui properti pada objek 'user' yang sudah ada
            user.Username = dto.Username;
            user.Name = dto.Name;
            user.Address = dto.Address;
            user.Phone = dto.Phone;
            user.Email = dto.Email;
            user.NoTin = dto.NoTin;
        }
    }
}