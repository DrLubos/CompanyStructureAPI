using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;

namespace CompanyStructAPI.Filters
{
    public class EmployeeUpdateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public EmployeeUpdateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var updatedEmployee = context.ActionArguments["updatedEmployee"] as Employee;
            if (updatedEmployee == null)
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
                var employee = _context.GetEmployeeByParameters(updatedEmployee.FirstName, updatedEmployee.LastName, updatedEmployee.Phone, updatedEmployee.Email);
                if (employee != null)
                {
                    context.ModelState.AddModelError("Employee", "Employee with entered first name, last name, phone and email already exists.");
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
