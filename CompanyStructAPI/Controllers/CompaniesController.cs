using CompanyStructAPI.Contexts;
using CompanyStructAPI.Filters.CompanyFilters;
using CompanyStructAPI.Filters.EmployeeFilters;
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
        public IActionResult GetCompanies()
        {
            return Ok(_context.Companies.ToList());
        }

        [TypeFilter(typeof(CompanyIdValidationFilter))]
        [HttpGet("{id}")]
        public IActionResult GetCompanyById(int id)
        {
            return Ok(_context.Companies.Find(id));
        }

        [HttpGet("search")]
        public IActionResult GetCompanies([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? ceoId)
        {
            var companies = _context.Companies.AsQueryable();
            if (id != null)
            {
                companies = companies.Where(c => c.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                companies = companies.Where(c => c.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                companies = companies.Where(c => c.Code.StartsWith(code));
            }
            if (ceoId != null)
            {
                companies = companies.Where(c => c.CeoID == ceoId);
            }
            return Ok(companies.ToList());
        }

        [TypeFilter(typeof(CompanyCreateValidationFilter))]
        [HttpPost]
        public IActionResult CreateCompany([FromBody] Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [TypeFilter(typeof(CompanyIdValidationFilter))]
        [TypeFilter(typeof(CompanyUpdateValidationFilter))]
        [HttpPut("{id}")]
        public IActionResult UpdateCompany(int id, [FromBody] Company updatedCompany)
        {
            var company = _context.Companies.Find(id);
            company.Name = updatedCompany.Name;
            company.Code = updatedCompany.Code;
            company.CeoID = updatedCompany.CeoID;
            _context.SaveChanges();
            return Ok(company);
        }

        [TypeFilter(typeof(CompanyUpdateValidationFilter))]
        [HttpPut("search")]
        public IActionResult SearchAndUpdate([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? ceoId, [FromBody] Company updatedCompany)
        {
            var companies = _context.Companies.AsQueryable();
            if (id != null)
            {
                companies = companies.Where(c => c.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                companies = companies.Where(c => c.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                companies = companies.Where(c => c.Code.StartsWith(code));
            }
            if (ceoId != null)
            {
                companies = companies.Where(c => c.CeoID == ceoId);
            }
            var companyList = companies.ToList();
            if (companyList.Count > 1)
            {
                ModelState.AddModelError("company", "Multiple companies found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            }
            if (companyList.Count == 0)
            {
                ModelState.AddModelError("company", "Company not found.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return new BadRequestObjectResult(problemDetails);
            }
            return UpdateCompany(companyList.First().Id, updatedCompany);
        }

        [TypeFilter(typeof(CompanyIdValidationFilter))]
        [TypeFilter(typeof(CompanyDeleteValidationFilter))]
        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.Companies.Find(id);
            _context.Companies.Remove(company);
            _context.SaveChanges();
            return NoContent();
        }

        [TypeFilter(typeof(CompanySearchAndDeleteValidationFilter))]
        [HttpDelete("search")]
        public IActionResult SearchAndDelete([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? ceoId)
        {
            var companies = _context.Companies.AsQueryable();
            if (id != null)
            {
                companies = companies.Where(c => c.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                companies = companies.Where(c => c.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                companies = companies.Where(c => c.Code.StartsWith(code));
            }
            if (ceoId != null)
            {
                companies = companies.Where(c => c.CeoID == ceoId);
            }
            var companyList = companies.ToList();
            return DeleteCompany(companyList.First().Id);
        }
    }
}
