using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserApi.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public int PcsInStock { get; set; }
        public decimal Price { get; set; }

        public string? ProductName { get; set; }

        public int? OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
