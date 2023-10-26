using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Share.DTOs;
using Sales.Share.Entities;

namespace Sales.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        //public CountriesController(DataContext dataContext)
        //{
        //    _dataContext = dataContext;
        //}
        public CountriesController(DataContext dataContext) => _dataContext = dataContext;

        //[HttpGet]
        //public async Task<ActionResult> GetAsync()
        //{
        //    return Ok(await _dataContext.Countries.Include(s=> s.States).ToListAsync());
        //}

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var country = await _dataContext.Countries.Include(s => s.States).ThenInclude(c => c.Cities).FirstOrDefaultAsync(x => x.Id == id);
            if (country is null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dataContext.Countries
                .Include(x => x.States)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _dataContext.Countries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(Country country)
        {
            try
            {
                _dataContext.Add(country);
                await _dataContext.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") || 
                    dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"El país {country.Name} ya existe.");
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

        
        [HttpPut]
        public async Task<ActionResult> PutAsync(Country country)
        {
            try
            {
                _dataContext.Update(country);
                await _dataContext.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                   dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"El país {country.Name} ya existe.");
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
            var country = await _dataContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            _dataContext.Remove(country);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
