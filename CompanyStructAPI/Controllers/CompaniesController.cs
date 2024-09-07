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
    }
}
