using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CompanyStructAPI.Filters.DivisionFilters
{
    public class DivisionDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DivisionDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var id = context.ActionArguments["id"] as int?;
            if (_context.Projects.Any(project => project.DivisionID == id))
            {
                context.ModelState.AddModelError("company", "Cannot delete, division is referenced in Project");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }

    public class DivisionSearchDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DivisionSearchDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var divisions = _context.Divisions.AsQueryable();
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as int?;
                if (id != null)
                {
                    divisions = divisions.Where(d => d.Id == id);
                }
            }
            if (context.ActionArguments.ContainsKey("name"))
            {
                var name = context.ActionArguments["name"] as string;
                if (name != null) {
                    divisions = divisions.Where(d => d.Name.StartsWith(name));
                }
            }
            if (context.ActionArguments.ContainsKey("code"))
            {
                var code = context.ActionArguments["code"] as string;
                if (code != null)
                {
                    divisions = divisions.Where(d => d.Code.StartsWith(code));
                }
            }
            if (context.ActionArguments.ContainsKey("companyID"))
            {
                var companyID = context.ActionArguments["companyID"] as int?;
                if (companyID != null)
                {
                    divisions = divisions.Where(d => d.CompanyID == companyID);
                }
            }
            if (context.ActionArguments.ContainsKey("directorID"))
            {
                var directorID = context.ActionArguments["directorID"] as int?;
                if (directorID != null)
                {
                    divisions = divisions.Where(d => d.DirectorID == directorID);
                }
            }       
            var divisionList = divisions.ToList();
            if (divisionList.Count > 1)
            {
                context.ModelState.AddModelError("division", "Multiple divisions found, please specify the search criteria");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            if (divisionList.Count == 0)
            {
                context.ModelState.AddModelError("division", "Division not found.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            if (_context.Projects.Any(project => project.DivisionID == divisionList.First().Id))
            {
                context.ModelState.AddModelError("division", "Cannot delete, division is referenced in Project");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
