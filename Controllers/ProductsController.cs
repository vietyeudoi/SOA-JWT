using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOA.Data;
using SOA.Models;

namespace SOA.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /products - Lấy danh sách tất cả các sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET /products/{id} - Lấy thông tin chi tiết một sản phẩm
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = $"Không tìm thấy sản phẩm với id = {id}" });

            return Ok(product);
        }

        // POST /products - Thêm một sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            var product = new Product
            {
                Name        = request.Name,
                Description = request.Description,
                Price       = request.Price,
                Quantity    = request.Quantity,
                CreatedAt   = DateTime.UtcNow,
                UpdatedAt   = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT /products/{id} - Cập nhật thông tin sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = $"Không tìm thấy sản phẩm với id = {id}" });

            product.Name        = request.Name;
            product.Description = request.Description;
            product.Price       = request.Price;
            product.Quantity    = request.Quantity;
            product.UpdatedAt   = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // DELETE /products/{id} - Xóa một sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = $"Không tìm thấy sản phẩm với id = {id}" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã xóa sản phẩm id = {id} thành công" });
        }
    }

    public class ProductRequest
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
