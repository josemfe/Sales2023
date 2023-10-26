using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Share.Entities;

namespace Sales.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CategoriesController(DataContext dataContext) => _dataContext = dataContext;

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _dataContext.Categories.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Category category)
        {
            try
            {
                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                    dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"La categoría {category.Name} ya existe.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Category category)
        {
            try
            {
                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                   dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"La categoría {category.Name} ya existe.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _dataContext.Remove(category);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }

    }

}
