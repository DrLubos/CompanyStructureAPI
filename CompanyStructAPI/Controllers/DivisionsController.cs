using CompanyStructAPI.Contexts;
using CompanyStructAPI.Filters.CompanyFilters;
using CompanyStructAPI.Filters.DivisionFilters;
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
        public IActionResult GetDivisions()
        {
            return Ok(_context.Divisions.ToList());
        }

        [TypeFilter(typeof(DivisionIdValidationFilter))]
        [HttpGet("{id}")]
        public IActionResult GetDivisionById(int id)
        {
            return Ok(_context.Divisions.Find(id));
        }

        [TypeFilter(typeof(DivisionCreateValidationFilter))]
        [HttpPost]
        public IActionResult CreateDivision([FromBody] Division division)
        {
            _context.Divisions.Add(division);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDivisionById), new { id = division.id }, division);
        }

        [TypeFilter(typeof(DivisionIdValidationFilter))]
        [TypeFilter(typeof(DivisionUpdateValidationFilter))]
        [HttpPut("{id}")]
        public IActionResult UpdateDivision(int id, [FromBody] Division updatedDivision)
        {
            var division = _context.Divisions.Find(id);
            division.Name = updatedDivision.Name;
            division.Code = updatedDivision.Code;
            division.CompanyID = updatedDivision.CompanyID;
            division.DirectorID = updatedDivision.DirectorID;
            _context.SaveChanges();
            return Ok(division);
        }

        [TypeFilter(typeof(DivisionIdValidationFilter))]
        [TypeFilter(typeof(DivisionDeleteValidationFilter))]
        [HttpDelete("{id}")]
        public IActionResult DeleteDivision(int id)
        {
            var division = _context.Divisions.Find(id);
            _context.Divisions.Remove(division);
            _context.SaveChanges();
            return Ok(division);
        }
    }
}
