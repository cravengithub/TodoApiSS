using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApiSS.DTOs; // <-- Menggunakan ProfileDTO
using TodoApiSS.Interfaces; // <-- Menggunakan IProfileService
using System.Security.Claims; // <-- Untuk mengambil ID dari Token
using System.Threading.Tasks;
using System;
namespace TodoApiSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // HARUS [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        // Kita inject IProfileService (sesuai arsitektur Service Layer)
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        // Helper untuk mendapatkan ID pengguna dari token
        // (POLA YANG SAMA DENGAN TODOITEMSCONTROLLER.CS ANDA)
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Kita bisa yakin ini ada karena [Authorize]
            return int.Parse(userIdString!);
        }
        /// <summary>
        /// GET: api/Profile/me
        /// Mendapatkan data profil milik PENGGUNA YANG SEDANG LOGIN.
        /// </summary>
        [HttpGet("me")] // Rute yang aman (sesuai frontend)
        [ProducesResponseType(typeof(ProfileDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProfileDTO>> GetMyProfile()
        {
            var userId = GetCurrentUserId(); // ID aman dari Token
            var profileDto = await _profileService.GetUserByIdAsync(userId);
            if (profileDto == null)
            {
                return NotFound("Profil pengguna tidak ditemukan.");
            }
            return Ok(profileDto);
        }
        /// <summary>
        /// PUT: api/Profile/me
        /// Memperbarui data profil milik PENGGUNA YANG SEDANG LOGIN.
        /// </summary>
        [HttpPut("me")] // Rute yang aman (sesuai frontend)
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMyProfile(ProfileDTO profileDto)
        {
            var userId = GetCurrentUserId(); // ID aman dari Token
            try
            {
                // Service akan memvalidasi data terhadap ID yang aman
                var result = await _profileService.UpdateUserAsync(userId, profileDto);
                if (!result)
                {
                    return NotFound("Profil pengguna tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
                // Menangkap error "Username sudah digunakan" dari service
                return BadRequest(new { message = ex.Message });
            }
            return NoContent();
        }
        /// <summary>
        /// DELETE: api/Profile/me
        /// Menghapus akun milik PENGGUNA YANG SEDANG LOGIN.
        /// </summary>
        [HttpDelete("me")] // Rute yang aman (sesuai frontend)
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var userId = GetCurrentUserId(); // ID aman dari Token
            var result = await _profileService.DeleteUserAsync(userId);
            if (!result)
            {
                return NotFound("Profil pengguna tidak ditemukan.");
            }
            // (Opsional: Tambahkan logika untuk logout/mencabut token di sini)
            return NoContent();
        }
    }
}