using System.ComponentModel.DataAnnotations;

namespace CompanyStructAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
