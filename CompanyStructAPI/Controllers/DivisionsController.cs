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

        [HttpGet("search")]
        public IActionResult GetDivisions([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? companyID, [FromQuery] int? directorID)
        {
            var divisions = _context.Divisions.AsQueryable();
            if (id != null)
            {
                divisions = divisions.Where(d => d.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                divisions = divisions.Where(d => d.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                divisions = divisions.Where(d => d.Code.StartsWith(code));
            }
            if (companyID != null)
            {
                divisions = divisions.Where(d => d.CompanyID == companyID);
            }
            if (directorID != null)
            {
                divisions = divisions.Where(d => d.DirectorID == directorID);
            }
            return Ok(divisions.ToList());
        }

        [TypeFilter(typeof(DivisionCreateValidationFilter))]
        [HttpPost]
        public IActionResult CreateDivision([FromBody] Division division)
        {
            _context.Divisions.Add(division);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDivisionById), new { id = division.Id }, division);
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

        [TypeFilter(typeof(DivisionUpdateValidationFilter))]
        [HttpPut("search")]
        public IActionResult SearchAndUpdateDivision([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? companyID, [FromQuery] int? directorID, [FromBody] Division updatedDivision)
        {
            var divisions = _context.Divisions.AsQueryable();
            if (id != null)
            {
                divisions = divisions.Where(d => d.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                divisions = divisions.Where(d => d.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                divisions = divisions.Where(d => d.Code.StartsWith(code));
            }
            if (companyID != null)
            {
                divisions = divisions.Where(d => d.CompanyID == companyID);
            }
            if (directorID != null)
            {
                divisions = divisions.Where(d => d.DirectorID == directorID);
            }
            var divisionList = divisions.ToList();
            if (divisionList.Count > 1)
            {
                ModelState.AddModelError("division", "Multiple divisions found, please specify the search criteria");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            }
            if (divisionList.Count == 0)
            {
                ModelState.AddModelError("division", "Division not found.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return new BadRequestObjectResult(problemDetails);
            }
            return UpdateDivision(divisionList.First().Id, updatedDivision);
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

        [TypeFilter(typeof(DivisionSearchDeleteValidationFilter))]
        [HttpDelete("search")]
        public IActionResult SearchAndDeleteDivision([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? companyID, [FromQuery] int? directorID)
        {
            var divisions = _context.Divisions.AsQueryable();
            if (id != null)
            {
                divisions = divisions.Where(d => d.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                divisions = divisions.Where(d => d.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                divisions = divisions.Where(d => d.Code.StartsWith(code));
            }
            if (companyID != null)
            {
                divisions = divisions.Where(d => d.CompanyID == companyID);
            }
            if (directorID != null)
            {
                divisions = divisions.Where(d => d.DirectorID == directorID);
            }
            var divisionList = divisions.ToList();          
            return DeleteDivision(divisionList.First().Id);
        }
    }
}
