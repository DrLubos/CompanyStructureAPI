using Microsoft.AspNetCore.Mvc.Filters;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Contexts;

namespace CompanyStructAPI.Filters.DepartmentFilters
{
    public class DepartmentCreateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DepartmentCreateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("department"))
            {
                var department = context.ActionArguments["department"] as Department;
                if (department != null)
                {
                    if (!_context.EmployeeExists(department.ManagerID))
                    {
                        context.ModelState.AddModelError("department", $"Employee with id: {department.ManagerID} not found. Cannot create department.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (!_context.ProjectExists(department.ProjectID))
                    {
                        context.ModelState.AddModelError("department", $"Project with id: {department.ProjectID} not found. Cannot create department.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (department.Name == null || department.Code == null)
                    {
                        context.ModelState.AddModelError("department", "Department is missing required fields.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                    if (department.Name.Length > 100 || department.Code.Length > 100)
                    {
                        context.ModelState.AddModelError("department", "Department fields are too long.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (_context.DepartmentParametersExists(department.Name, department.Code))
                    {
                        context.ModelState.AddModelError("department", "Department already exists.");
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
