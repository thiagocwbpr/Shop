using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controller
{
    [Route("Products")]
    public class ProductController : ControllerBase{
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context){

            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id){
            var products = await context
            .Products.Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            return products;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id){
            var products = await context
            .Products.Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.CategoryId == id)
            .ToListAsync();
            return products;
        }
        
        [HttpPost]
        [Route("")]
        [Authorize (Roles = "employee")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] Product model
            ){
                if (!ModelState.IsValid)
                   BadRequest(ModelState);
                try
                {
                    context.Products.Add(model);
                    await context.SaveChangesAsync(); // Salva as mudanças no banco.
                    return Ok(model);
                }
                catch
                {
                    return BadRequest(new {message = "Categoria não encontrada."});
                }
        }
    }
}

