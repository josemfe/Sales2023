using Sales.Share.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sales.Share.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Provincia")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;



    }
}
