using System.ComponentModel.DataAnnotations;

namespace CompanyStructAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Code { get; set; }
        [Required]
        public required int CeoID { get; set; }
    }
}
