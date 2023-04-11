using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;


namespace Shop.Controller
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase{
       [HttpGet]
       [Route("")] 
       [AllowAnonymous]
       [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30 )]
       // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,NoStore = true)]
       public async Task<ActionResult<List<Category>>> Get(
         [FromServices] DataContext context
       ){
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
       }

       [HttpGet] 
       [Route("{id:int}")] 
       [AllowAnonymous]
       public async Task<ActionResult<Category>> GetById(
         int id,
         [FromServices] DataContext context){
           var categories = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
           return Ok(categories);
       }

       [HttpPost] 
       [Route("")]
       [Authorize (Roles = "employee")]
       public async Task<ActionResult<Category>> Post(
          [FromBody]Category model,
          [FromServices] DataContext context
          ){
               if(!ModelState.IsValid)
               return BadRequest(ModelState);
               try
               {
                  context.Categories.Add(model);
                  await context.SaveChangesAsync();
                  // return Ok(model);
                     return Ok(new {message = "Dados registrados com sucesso!"});

               }
               catch
               {
                  return BadRequest(new {message = "Não foi possivel carregar a categoria"});
               }  
       }

       [HttpPut] 
       [Route("{id:int}")] 
       [Authorize (Roles = "employee")]
       public async Task<ActionResult<Category>> Put(
         int id,
         [FromBody]Category model,
         [FromServices] DataContext context){
          if(id != model.Id)
          return NotFound(new {message = "Categoria não encontrada"});
          if(!ModelState.IsValid)
          return BadRequest(ModelState);
          try
          {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
          }
          catch (DbUpdateConcurrencyException) // Erro de registro atualizado ao mesmo tempo. Concorrencia.
          {
            return BadRequest(new {message = "Este registro já foi atualizado"});
          }
          catch(Exception){
               return BadRequest(new {message = "Não foi possivel atualizar a categoria"});
          }
          
       }
       
       [HttpDelete] 
       [Route("{id:int}")] 
       [Authorize (Roles = "employee")]
       public async Task<ActionResult<Category>> Delete(
         int id,
         [FromServices] DataContext context
       ){

         var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
         if (category == null)
            return NotFound(new {message = "Categoria não encontrada."});
         try
         {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            // return Ok(category);
            return Ok(new {message = "Categoria removida com sucesso!"});
         }
         catch
         {
            return BadRequest(new {message = "Não foi possivel remover a categoria."});
         }
       }
    }
}