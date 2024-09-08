using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;
using CompanyStructAPI.Contexts;

namespace CompanyStructAPI.Filters.DepartmentFilters
{
    public class DepartmentUpdateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DepartmentUpdateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var updatedDepartment = context.ActionArguments["updatedDepartment"] as Department;
            if (updatedDepartment == null)
            {
                context.ModelState.AddModelError("department", "Department is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                if (!_context.EmployeeExists(updatedDepartment.ManagerID))
                {
                    context.ModelState.AddModelError("department", $"Employee with id: {updatedDepartment.ManagerID} not found. Cannot update department.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (!_context.ProjectExists(updatedDepartment.ProjectID))
                {
                    context.ModelState.AddModelError("department", $"Project with id: {updatedDepartment.ProjectID} not found. Cannot update department.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (updatedDepartment.Name == null || updatedDepartment.Code == null)
                {
                    context.ModelState.AddModelError("department", "Department is missing required fields.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
                if (updatedDepartment.Name.Length > 100 || updatedDepartment.Code.Length > 100)
                {
                    context.ModelState.AddModelError("department", "Department fields are too long.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                var department = _context.GetDepartmentByParameters(updatedDepartment.Name, updatedDepartment.Code);
                if (department != null && department.ManagerID == updatedDepartment.ManagerID && department.ProjectID == updatedDepartment.ProjectID)
                {
                    context.ModelState.AddModelError("department", "Department with entered name, code, managerID and projectID already exists.");
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
