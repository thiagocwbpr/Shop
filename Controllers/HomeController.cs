using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Backoffice.Controllers
{
    [Route("v1")]
    public class HomeController : ControllerBase{
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get(
            [FromServices] DataContext context){

                var employee = new User {Id = 1, Username = "Jessica" , Password = "jessica", Role = "employee"};
                var manager = new User {Id = 2, Username = "Thiago", Password = "thiaguito" , Role = "manager"};
                var category = new Category {Id = 1, Title = "Informatica"};
                var Product = new Product {Id = 1 , Category = category, Title = "Mouse", Price = 299, Description = "Mouse gamer"};
                context.Users.Add(employee);
                context.Users.Add(manager);
                context.Categories.Add(category);
                context.Products.Add(Product);
                await context.SaveChangesAsync();

                return Ok(new {
                    message = "Dados configurados! :)"});
            }
    }
}