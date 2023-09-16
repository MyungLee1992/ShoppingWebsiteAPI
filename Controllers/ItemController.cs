using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase {
        private readonly DataContext _context;

        public ItemController(DataContext context) {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Item>>> GetAllItems() {
        {
            return Ok(await _itemService.GetItemsAsync());
        }

        [HttpGet("find/{id}")]
        public async Task<ActionResult<Item>> GetItemById(int id) {
            var item = await _context.Items.FindAsync(id);
            return Ok(item);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Item>> AddItem(Item item) {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Item>> UpdateItem(Item item) {
            var existingItem = await _context.Items.FindAsync(item.Id);
            if (existingItem == null) {
                return BadRequest("Item not found.");
            }

            existingItem.Name = item.Name; 
            existingItem.Description = item.Description;
            existingItem.Type = item.Type;
            existingItem.Price = item.Price;
            existingItem.ImageUrl = item.ImageUrl;

            _context.Items.Update(existingItem);
            await _context.SaveChangesAsync();
            return Ok(existingItem);    
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id) {
            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem == null) {
                return BadRequest("Item not found.");
            }

            _context.Items.Remove(existingItem);    
            await _context.SaveChangesAsync();

            return Ok(existingItem);
        }
    }
}
