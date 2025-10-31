using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApiSS.DTOs;
using TodoApiSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApiSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // --- PERINGATAN KEAMANAN PENTING ---
    // [Authorize] saja berarti SETIAP user yang login bisa menghapus user LAIN.
    // Ini seharusnya HANYA untuk Admin.
    //
    // Setelah Anda mengimplementasikan Roles (Peran), ganti ini menjadi:
    // [Authorize(Roles = "Admin")]
    //
    // Untuk saat ini, kita gunakan [Authorize] agar endpoint-nya terlindungi.
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GET: api/Users
        /// Mendapatkan semua data user (Hanya untuk Admin).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// GET: api/Users/5
        /// Mendapatkan data user spesifik (Hanya untuk Admin).
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);

            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        /// <summary>
        /// PUT: api/Users/5
        /// Memperbarui data user spesifik (Hanya untuk Admin).
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutUser(int id, UserDTO userDto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, userDto);
                if (!result)
                {
                    // User tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Menangkap error "Username sudah digunakan" dari service
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// POST: api/Users
        /// Membuat user baru (Hanya untuk Admin).
        /// Catatan: User DTO ini tidak menyertakan password.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 200)] // Seharusnya 201, tapi DTO tidak punya ID
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            try
            {
                var createdUserDto = await _userService.CreateUserAsync(userDto);

                // CATATAN:
                // Kita tidak bisa menggunakan CreatedAtAction(nameof(GetUser), ...)
                // karena UserDTO (dari prompt Anda) tidak memiliki field 'Id'.
                // Jadi, kita kembalikan 200 OK saja.
                return Ok(createdUserDto);
            }
            catch (Exception ex)
            {
                // Menangkap error "Username sudah digunakan" dari service
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// DELETE: api/Users/5
        /// Menghapus user (Hanya untuk Admin).
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                // User tidak ditemukan
                return NotFound();
            }

            return NoContent();
        }
    }
}
