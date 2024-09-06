using System.ComponentModel.DataAnnotations;

namespace CompanyStructAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Phone]
        public required string Phone { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
    }
}
