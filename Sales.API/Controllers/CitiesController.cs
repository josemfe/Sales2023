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
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CitiesController(DataContext dataContext) => _dataContext = dataContext;

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _dataContext.Cities.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(City city)
        {
            try
            {
                _dataContext.Add(city);
                await _dataContext.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                    dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"La ciudad {city.Name} ya existe.");
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
            var city = await _dataContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city is null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(City city)
        {
            try
            {
                _dataContext.Update(city);
                await _dataContext.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                   dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"La ciudad {city.Name} ya existe.");
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
            var city = await _dataContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            _dataContext.Remove(city);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
