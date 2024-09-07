using CompanyStructAPI.Contexts;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyStructAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DivisionsController : ControllerBase
    {
        private readonly CompanyContext _context;
        
        public DivisionsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Division>> GetDivisions()
        {
            var divisions = _context.Divisions.ToList();
            return Ok(divisions);
        }

        [HttpGet("{id}")]
        public ActionResult<Division> GetDivisionById(int id)
        {
            var division = _context.Divisions.Find(id);
            if (division == null)
            {
                return NotFound();
            }
            return Ok(division);
        }

        [HttpPost]
        public ActionResult<Division> CreateDivision([FromBody] Division division)
        {
            if (!_context.EmployeeExists(division.DirectorID))
            {
                return BadRequest($"Employee with id: {division.DirectorID} not found. Cannot create division.");
            }
            if (!_context.CompanyExists(division.CompanyID))
            {
                return BadRequest($"Company with id: {division.CompanyID} not found. Cannot create division.");
            }
            _context.Divisions.Add(division);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDivisionById), new { id = division.id }, division);
        }

        [HttpPut("{id}")]
        public ActionResult<Division> UpdateDivision(int id, [FromBody] Division updatedDivision)
        {
            var division = _context.Divisions.Find(id);
            if (division == null)
            {
                Division newDivision = new Division
                {
                    Name = updatedDivision.Name,
                    Code = updatedDivision.Code,
                    CompanyID = updatedDivision.CompanyID,
                    DirectorID = updatedDivision.DirectorID
                };
                return CreateDivision(newDivision);
            }
            if (!_context.EmployeeExists(updatedDivision.DirectorID))
            {
                return BadRequest($"Employee with id: {updatedDivision.DirectorID} not found. Cannot update division.");
            }
            if (!_context.CompanyExists(updatedDivision.CompanyID))
            {
                return BadRequest($"Company with id: {updatedDivision.CompanyID} not found. Cannot update division.");
            }
            division.Name = updatedDivision.Name;
            division.Code = updatedDivision.Code;
            division.CompanyID = updatedDivision.CompanyID;
            division.DirectorID = updatedDivision.DirectorID;
            _context.SaveChanges();
            return Ok(division);
        }

        [HttpDelete("{id}")]
        public ActionResult<Division> DeleteDivision(int id)
        {
            var division = _context.Divisions.Find(id);
            if (division == null)
            {
                return NotFound();
            }
            if (_context.Projects.Any(project => project.DivisionID == id))
            {
                return BadRequest("Division is referenced in a project. Cannot delete division.");
            }
            _context.Divisions.Remove(division);
            _context.SaveChanges();
            return Ok(division);
        }
    }
}
