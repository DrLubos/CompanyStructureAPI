using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructAPI.Filters.CompanyFilters
{
    public class CompanyUpdateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public CompanyUpdateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var updatedCompany = context.ActionArguments["updatedCompany"] as Company;
            if (updatedCompany == null)
            {
                context.ModelState.AddModelError("company", "Company is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                if (!_context.EmployeeExists(updatedCompany.CeoID))
                {
                    context.ModelState.AddModelError("company", $"Employee with id: {updatedCompany.CeoID} not found. Cannot update company.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (updatedCompany.Name == null || updatedCompany.Code == null)
                {
                    context.ModelState.AddModelError("company", "Company is missing required fields.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
                if (updatedCompany.Name.Length > 100 || updatedCompany.Code.Length > 100)
                {
                    context.ModelState.AddModelError("company", "Company fields are too long.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                var company = _context.GetCompanyByParameters(updatedCompany.Name, updatedCompany.Code);
                if (company != null)
                {
                    context.ModelState.AddModelError("company", "Company with entered name and code already exists.");
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
