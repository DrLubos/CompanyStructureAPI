using CompanyStructAPI.Contexts;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CompaniesController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Company>> GetCompanies()
        {
            var companies = _context.Companies.ToList();
            return Ok(companies);
        }


        [HttpGet("{id}")]
        public ActionResult<Company> GetCompanyById(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] Company company)
        {
            if (!EmployeeExists(company.CeoID))
            {
                return BadRequest($"Employee with id: {company.CeoID} not found. Cannot create company.");
            }
            _context.Companies.Add(company);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(int id, [FromBody] Company updatedCompany)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                Company newCompany = new Company
                {
                    Name = updatedCompany.Name,
                    Code = updatedCompany.Code,
                    CeoID = updatedCompany.CeoID
                };
                return CreateCompany(newCompany);
            }
            if (!EmployeeExists(updatedCompany.CeoID))
            {
                return BadRequest($"Employee with id: {updatedCompany.CeoID} not found. Cannot update company.");
            }
            company.Name = updatedCompany.Name;
            company.Code = updatedCompany.Code;
            company.CeoID = updatedCompany.CeoID;
            _context.SaveChanges();
            return Ok(company);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            if (_context.Divisions.Any(division => division.CompanyID == id))
            {
                return BadRequest("Cannot delete, company is referenced in Division");
            }
            _context.Companies.Remove(company);
            _context.SaveChanges();
            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            Employee employee = _context.Employees.Find(id);
            return employee != null;
        }
    }
}
