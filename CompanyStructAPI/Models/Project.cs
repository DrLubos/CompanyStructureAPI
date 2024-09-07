using System.ComponentModel.DataAnnotations;

namespace CompanyStructAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public int DivisionID { get; set; }
        public int ManagerID { get; set; }
    }
}
