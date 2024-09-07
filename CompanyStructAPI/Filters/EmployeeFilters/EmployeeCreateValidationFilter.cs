using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;
using CompanyStructAPI.Contexts;
namespace CompanyStructAPI.Filters.EmployeeFilters
{
    public class EmployeeCreateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public EmployeeCreateValidationFilter(CompanyContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var employee = context.ActionArguments["employee"] as Employee;
            if (employee == null)
            {
                context.ModelState.AddModelError("employee", "Employee is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                if (employee.FirstName == null || employee.LastName == null || employee.Phone == null || employee.Email == null)
                {
                    context.ModelState.AddModelError("employee", "Employee is missing required fields.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
                if (employee.FirstName.Length > 100 || employee.LastName.Length > 100 || employee.Phone.Length > 100 || employee.Email.Length > 100)
                {
                    context.ModelState.AddModelError("employee", "Employee fields are too long.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (_context.EmployeeParametersExists(employee.FirstName, employee.LastName, employee.Phone, employee.Email))
                {
                    context.ModelState.AddModelError("employee", "Employee already exists.");
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
