using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Share.Entities;

namespace Sales.API.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _dataContext.Contries.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Post(Country country)
        {
            _dataContext.Add(country);
            await _dataContext.SaveChangesAsync(); 
            return Ok(country);    
        }

    }
}
