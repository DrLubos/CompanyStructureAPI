using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Contexts;

namespace CompanyStructAPI.Filters.DepartmentFilters
{
    public class DepartmentIdValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DepartmentIdValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var departmentId = context.ActionArguments["id"] as int?;
                if (departmentId != null && departmentId.HasValue)
                {
                    if (departmentId.Value < 0)
                    {
                        context.ModelState.AddModelError("id", "Department ID is invalid.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    else if (!_context.DepartmentExists(departmentId.Value))
                    {
                        context.ModelState.AddModelError("id", "Department ID does not exist.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                }
            }
        }
    }
}
