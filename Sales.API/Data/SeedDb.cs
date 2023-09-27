using Sales.Share.Entities;

namespace Sales.API.Data
{

    public class SeedDb
    {
        private readonly DataContext _dataContext;

        public SeedDb(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_dataContext.Countries.Any())
            {
                _dataContext.Countries.Add(new Country { Name = "España" });
                _dataContext.Countries.Add(new Country { Name = "Portugal" });
            }

            await _dataContext.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_dataContext.Categories.Any())
            {
                _dataContext.Categories.Add(new Category { Name = "Categoría 1" }) ;
                _dataContext.Categories.Add(new Category { Name = "Categoría 2" });
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
