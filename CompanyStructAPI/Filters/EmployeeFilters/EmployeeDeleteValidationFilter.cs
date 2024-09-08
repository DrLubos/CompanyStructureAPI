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
    public class EmployeeSearchAndDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public EmployeeSearchAndDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var employees = _context.Employees.AsQueryable();
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as int?;
                if (id != null)
                {
                    employees = employees.Where(e => e.Id == id);
                }
            }
            if (context.ActionArguments.ContainsKey("title"))
            {
                var title = context.ActionArguments["title"] as string;
                if (!string.IsNullOrEmpty(title))
                {
                    employees = employees.Where(e => e.Title == title);
                }
            }
            if (context.ActionArguments.ContainsKey("firstname"))
            {
                var firstname = context.ActionArguments["firstname"] as string;
                if (!string.IsNullOrEmpty(firstname))
                {
                    employees = employees.Where(e => e.FirstName.StartsWith(firstname));
                }
            }
            if (context.ActionArguments.ContainsKey("lastname"))
            {
                var lastname = context.ActionArguments["lastname"] as string;
                if (!string.IsNullOrEmpty(lastname))
                {
                    employees = employees.Where(e => e.LastName.StartsWith(lastname));
                }
            }
            if (context.ActionArguments.ContainsKey("phone"))
            {
                var phone = context.ActionArguments["phone"] as string;
                if (!string.IsNullOrEmpty(phone))
                {
                    employees = employees.Where(e => e.Phone.StartsWith(phone));
                }
            }
            if (context.ActionArguments.ContainsKey("email"))
            {
                var email = context.ActionArguments["email"] as string;
                if (!string.IsNullOrEmpty(email))
                {
                    employees = employees.Where(e => e.Email.StartsWith(email));
                }
            }
            if (employees.Count() > 1)
            {
                context.ModelState.AddModelError("employee", "Multiple employees found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            if (employees.Count() == 0)
            {
                context.ModelState.AddModelError("employee", "Employee not found.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            var employee = employees.First();
            bool isReferenceInCompany = _context.Companies.Any(c => c.CeoID == employee.Id);
            bool isReferenceInDivision = _context.Divisions.Any(d => d.DirectorID == employee.Id);
            bool isReferenceInProject = _context.Projects.Any(p => p.ManagerID == employee.Id);
            bool isReferenceInDepartment = _context.Departments.Any(d => d.ManagerID == employee.Id);
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
                return;
            }
        }
    }
}
