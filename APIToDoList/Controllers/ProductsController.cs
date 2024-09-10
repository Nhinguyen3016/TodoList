using APIToDoList.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using APIToDoList.Model; 

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Static list to hold ToDo items. In a real-world scenario, this would be replaced with a database.
        private static List<TodoItem> TodoItems = new List<TodoItem>();

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetAll()
        {
            return Ok(TodoItems);
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            var item = TodoItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] TodoItem newItem)
        {
            if (newItem == null)
                return BadRequest();

            newItem.Id = TodoItems.Count > 0 ? TodoItems.Max(x => x.Id) + 1 : 1;
            TodoItems.Add(newItem);
            return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TodoItem updatedItem)
        {
            if (updatedItem == null)
                return BadRequest();

            var existingItem = TodoItems.FirstOrDefault(x => x.Id == id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = updatedItem.Name;
            existingItem.IsComplete = updatedItem.IsComplete;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = TodoItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();

            TodoItems.Remove(item);
            return NoContent();
        }
    }
}
