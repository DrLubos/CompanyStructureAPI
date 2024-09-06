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
        public ActionResult<Company> CreateCompany([FromBody] Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }
        [HttpPut("{id}")]
        public ActionResult<Company> UpdateCompany(int id, [FromBody] Company updatedCompany)
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
            company.Name = updatedCompany.Name;
            company.Code = updatedCompany.Code;
            company.CeoID = updatedCompany.CeoID;
            _context.SaveChanges();
            return Ok(company);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            _context.Companies.Remove(company);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
