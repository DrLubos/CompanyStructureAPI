using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;


namespace CompanyStructAPI.Filters.CompanyFilters
{
    public class CompanyCreateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public CompanyCreateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("company"))
            {
                var company = context.ActionArguments["company"] as Company;
                if (company != null)
                {
                    if (!_context.EmployeeExists(company.CeoID))
                    {
                        context.ModelState.AddModelError("company", $"Employee with id: {company.CeoID} not found. Cannot create company.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (company.Name == null || company.Code == null)
                    {
                        context.ModelState.AddModelError("company", "Company is missing required fields.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                    if (company.Name.Length > 100 || company.Code.Length > 100)
                    {
                        context.ModelState.AddModelError("company", "Company fields are too long.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (_context.CompanyParametersExists(company.Name, company.Code))
                    {
                        context.ModelState.AddModelError("company", "Company already exists.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                }
            }
        }
    }
}
