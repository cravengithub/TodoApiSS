using System.ComponentModel.DataAnnotations;
namespace TodoApiSS.DTOs
{
    // DTO untuk registrasi dan login
    public class UserDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NoTin { get; set; } = string.Empty;
    }
}
