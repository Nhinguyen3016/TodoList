using APIToDoList.Data;
using APIToDoList.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu xác thực cho tất cả các actions trong controller này
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
        {
            // Chỉ người dùng đã xác thực mới có thể truy cập endpoint này
            return Ok(await _context.Products.ToListAsync());
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            var todo = await _context.Products.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // POST: api/Todo
        [HttpPost]
        [Authorize(Roles = "Admin")] // Chỉ admin mới có thể tạo mới
        public async Task<ActionResult<TodoItem>> PostTodo([FromBody] TodoItem todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo object is null");
            }

            _context.Products.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ admin mới có thể cập nhật
        public async Task<IActionResult> PutTodo(int id, [FromBody] TodoItem todo)
        {
            if (id != todo.Id)
            {
                return BadRequest("Todo ID mismatch");
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ admin mới có thể xóa
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Products.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Products.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
