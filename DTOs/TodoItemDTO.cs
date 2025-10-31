using System.ComponentModel.DataAnnotations;

namespace TodoApiSS.DTOs
{
    /// <summary>
    /// Data Transfer Object untuk Todo Item
    /// </summary>
    public class TodoItemDTO
    {
        /// <summary>
        /// ID dari todo item
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Nama atau deskripsi dari todo item
        /// </summary>
        [Required(ErrorMessage = "Nama todo item harus diisi")]
        [StringLength(100, ErrorMessage = "Nama todo item tidak boleh lebih dari 100 karakter")]
        public string? Name { get; set; }

        /// <summary>
        /// Status penyelesaian todo item
        /// </summary>
        public bool IsComplete { get; set; }
    }
}