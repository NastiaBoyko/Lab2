using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLab.Data;
using WebApiLab.Models;

namespace WebApiLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly WebApiLabContext _context;

        public OrdersController(WebApiLabContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order
                .Include(c => c.Products).ThenInclude(c => c.Product).ThenInclude(c => c.Category)
                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order
                .Include(c => c.Products).ThenInclude(c => c.Product).ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
			_context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpPost("{id}/products")]
        public async Task<ActionResult> AddProductToOrder(int id, [FromBody] Product value)
        {
            var prod = _context.Product.FirstOrDefault(t => t.Name == value.Name);
            var ord = await _context.Order.Include(c => c.Products)
               .FirstOrDefaultAsync(x => x.Id == id);
            if (ord == null)
            {
                return NotFound();
            }
            _context.ProductOrder.Add(new ProductOrder { Order = ord, OrderId = id, Product = prod, ProductId = prod.Id });
            await _context.SaveChangesAsync();

            return Ok();

        }

        // DELETE: api/Orders/5
        [HttpDelete("{ordId}/products/{prodId}")]
        public async Task<ActionResult<ProductOrder>> DeleteOrder(int ordId, int prodId)
        {
            var productOrder = _context.ProductOrder
                .FirstOrDefault(x => x.OrderId == ordId && x.ProductId == prodId);
            if(productOrder == null)
			{
                return NotFound();
			}

            _context.ProductOrder.Remove(productOrder);
            await _context.SaveChangesAsync();

            return productOrder;
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
