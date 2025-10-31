using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace TodoApiSS.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        // Kita tidak menyimpan password, tapi hash & salt-nya
        [Required]
        public byte[] PasswordHash { get; set; } = new byte[0];
        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[0];

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        public string NoTin { get; set; } = string.Empty;


        // --- TAMBAHKAN INI ---
        // JsonIgnore mencegah loop serialisasi saat mengambil data
        [JsonIgnore]
        public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        // --- BATAS TAMBAHAN ---
    }
}