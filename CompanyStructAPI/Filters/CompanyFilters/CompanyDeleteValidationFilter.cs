using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.CompanyFilters
{
    public class CompanyDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public CompanyDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var id = context.ActionArguments["id"] as int?;
            if (_context.Divisions.Any(division => division.CompanyID == id))
            {
                context.ModelState.AddModelError("company", "Cannot delete, company is referenced in Division");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }

    public class CompanySearchAndDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public CompanySearchAndDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var companies = _context.Companies.AsQueryable();
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as int?;
                if (id != null)
                {
                    companies = companies.Where(c => c.Id == id);
                }
            }
            if (context.ActionArguments.ContainsKey("name"))
            {
                var name = context.ActionArguments["name"] as string;
                if (!string.IsNullOrEmpty(name))
                {
                    companies = companies.Where(c => c.Name.StartsWith(name));
                }
            }
            if (context.ActionArguments.ContainsKey("code"))
            {
                var code = context.ActionArguments["code"] as string;
                if (!string.IsNullOrEmpty(code))
                {
                    companies = companies.Where(c => c.Code.StartsWith(code));
                }
            }
            if (context.ActionArguments.ContainsKey("ceoId"))
            {
                var ceoId = context.ActionArguments["ceoId"] as int?;
                if (ceoId != null) {
                    companies = companies.Where(c => c.CeoID == ceoId);
                }
            }
            var companyList = companies.ToList();
            if (companyList.Count > 1)
            {
                context.ModelState.AddModelError("company", "Multiple companies found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            if (companyList.Count == 0)
            {
                context.ModelState.AddModelError("company", "Company not found.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
                return;
            }
            if (_context.Divisions.Any(division => division.CompanyID == companyList.First().Id))
            {
                context.ModelState.AddModelError("company", "Cannot delete, company is referenced in Division");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
