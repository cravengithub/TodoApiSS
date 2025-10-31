using System.ComponentModel.DataAnnotations.Schema;
namespace TodoApiSS.Models
{
    public class TodoItem
    {
        public long Id { get; set; } // EF Core akan otomatis mengenali 'Id' sebagai Primary Key
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        // --- TAMBAHKAN INI ---
        public int UserId { get; set; } // Foreign Key
        [ForeignKey("UserId")]
        public virtual User? User { get; set; } // Properti Navigasi
        // --- BATAS TAMBAHAN ---
    }
}
