using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.EmployeeFilters
{
    public class EmployeeDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public EmployeeDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as int?;
                bool isReferenceInCompany = _context.Companies.Any(c => c.CeoID == id);
                bool isReferenceInDivision = _context.Divisions.Any(d => d.DirectorID == id);
                bool isReferenceInProject = _context.Projects.Any(p => p.ManagerID == id);
                bool isReferenceInDepartment = _context.Departments.Any(d => d.ManagerID == id);
                bool foundError = isReferenceInCompany || isReferenceInDivision || isReferenceInProject || isReferenceInDepartment;
                if (isReferenceInCompany)
                {
                    context.ModelState.AddModelError("employee", "Cannot delete, employee is a CEO of a company.");
                }
                if (isReferenceInDivision)
                {
                    context.ModelState.AddModelError("employee", "Cannot delete, employee is a director of a division.");
                }
                if (isReferenceInProject)
                {
                    context.ModelState.AddModelError("employee", "Cannot delete, employee is a manager of a project.");
                }
                if (isReferenceInDepartment)
                {
                    context.ModelState.AddModelError("employee", "Cannot delete, employee is a manager of a department.");
                }
                if (foundError)
                {
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
