using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApiSS.Interfaces;
using TodoApiSS.Models;
using System.Security.Claims; // <-- TAMBAHKAN
 
namespace TodoApiSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Semua metode di sini memerlukan login
    public class TodoItemsController : ControllerBase{
        private readonly ITodoService _todoService;
 
        public TodoItemsController(ITodoService todoService)
        {
            _todoService = todoService;
        }
 
        // Helper untuk mendapatkan ID pengguna dari token
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Kita bisa yakin ini ada karena [Authorize]
            return int.Parse(userIdString!); 
        }
 
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var userId = GetCurrentUserId();
            var todos = await _todoService.GetTodosAsync(userId);
            return Ok(todos);
        }
 
        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var userId = GetCurrentUserId();
            var todoItem = await _todoService.GetTodoByIdAsync(id, userId);
 
            if (todoItem == null)
            {
                return NotFound(); // Item tidak ada ATAU bukan milik Anda
            }
 
            return Ok(todoItem);
        }
 
        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            var userId = GetCurrentUserId();
            
            try
            {
                // Service akan menetapkan UserId secara paksa
                var createdItem = await _todoService.CreateTodoAsync(todoItem, userId); 
                return CreatedAtAction(nameof(GetTodoItem), new { id = createdItem.Id }, createdItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
 
        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            var userId = GetCurrentUserId();
            var result = await _todoService.UpdateTodoAsync(id, todoItem, userId);
 
            if (!result)
            {
                if (id != todoItem.Id) return BadRequest();
                return NotFound(); // Item tidak ditemukan ATAU bukan milik Anda
            }
 
            return NoContent();
        }
 
        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var userId = GetCurrentUserId();
            var result = await _todoService.DeleteTodoAsync(id, userId);
 
            if (!result)
            {
                return NotFound(); // Item tidak ditemukan ATAU bukan milik Anda
            }
 
            return NoContent();
        }
    }
}
