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
    [Route("api/states")]
    public class StatesController: ControllerBase
    {
        private readonly DataContext _dataContext;

        public StatesController(DataContext dataContext) => _dataContext = dataContext;

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _dataContext.States.Include(s => s.Cities).ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(State state)
        {
            try
            {
                _dataContext.Add(state);
                await _dataContext.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                    dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"La provincia {state.Name} ya existe.");
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
            var state = await _dataContext.States.Include(s => s.Cities).FirstOrDefaultAsync(x => x.Id == id);
            if (state is null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(State state)
        {
            try
            {
                _dataContext.Update(state);
                await _dataContext.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate") ||
                   dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest($"La provincia {state.Name} ya existe.");
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
            var state = await _dataContext.States.FirstOrDefaultAsync(x => x.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            _dataContext.Remove(state);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
