using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        [JsonIgnore]
        public List<Product> Products { get; set; } = new();

        // Mapped wird nicht in der db erstellt ist nur fur c# und wird immer neu berechnet
        [NotMapped]
        public decimal TotalPrice => Products.Sum(p => p.Price);

        // Berechnung uber controller und dan in db speichern
        public int TotalPieces { get; set; }
    }
}
