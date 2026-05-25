using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string Password { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}
