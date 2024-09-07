using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;
using CompanyStructAPI.Contexts;
namespace CompanyStructAPI.Filters
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
                context.ModelState.AddModelError("Employee", "Employee is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
               if (_context.EmployeeParametersExists(employee.FirstName, employee.LastName, employee.Phone, employee.Email))
                {
                    context.ModelState.AddModelError("Employee", "Employee already exists.");
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
