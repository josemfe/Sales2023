using Sales.Share.Entities;

namespace Sales.Share.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }
    }

}
