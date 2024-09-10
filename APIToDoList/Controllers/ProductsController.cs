using APIToDoList.Data;  
using APIToDoList.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        {
            return Ok(await _context.Products.ToListAsync());
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(int id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Post([FromBody] TodoItem newItem)
        {
            if (newItem == null)
                return BadRequest();

            _context.Products.Add(newItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
        }

      
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TodoItem updatedItem)
        {
            if (updatedItem == null)
                return BadRequest();

            var existingItem = await _context.Products.FindAsync(id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = updatedItem.Name;
            existingItem.IsComplete = updatedItem.IsComplete;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a To-Do item
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Products.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
