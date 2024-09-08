using System.ComponentModel.DataAnnotations;

namespace CompanyStructAPI.Models
{
    public class Division
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public int CompanyID { get; set; }
        public int DirectorID { get; set; }
    }
}
