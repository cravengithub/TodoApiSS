using System.ComponentModel.DataAnnotations;
namespace TodoApiSS.DTOs
{
    /// <summary>
    /// DTO untuk menampilkan dan mengedit data profil pengguna.
    /// DTO ini SESUAI dengan form di frontend Angular.
    /// </summary>
    public class ProfileDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        public string NoTin { get; set; } = string.Empty;
    }
}