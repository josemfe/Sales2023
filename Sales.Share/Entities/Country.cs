using System.ComponentModel.DataAnnotations;

namespace Sales.Share.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name="Pais")]
        [MaxLength(100,ErrorMessage ="El campo {0} debe tener un máximo de {1} carateres")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public string Name { get; set; }    
    }
}
