using DemoCrudDotNet9.Data;
using DemoCrudDotNet9.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoCrudDotNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductDbContext context) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts ()
        {
            var products = await context.Products.AsNoTracking().ToListAsync();
            return products.Count != 0 ? Ok(products) : NotFound();
        }

        [HttpGet("single/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            return product is not null ? Ok(product) : NotFound();
        }

        [HttpPost("add")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            product.DateCreate = DateTime.Now;
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id =  product.Id }, product);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return Ok(product);
            //var _product = context.Products.Find(product.Id);
           // if (_product is not null)
           // {
           //     _product.Description = product.Description;
           //     _product.Name = product.Name;
            //}
            //return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }
    }
}
