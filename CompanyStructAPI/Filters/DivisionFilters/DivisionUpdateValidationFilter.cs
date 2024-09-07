using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;

namespace CompanyStructAPI.Filters.DivisionFilters
{
    public class DivisionUpdateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DivisionUpdateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var updatedDivision = context.ActionArguments["updatedDivision"] as Division;
            if (updatedDivision == null)
            {
                context.ModelState.AddModelError("division", "Division is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                if (!_context.EmployeeExists(updatedDivision.DirectorID))
                {
                    context.ModelState.AddModelError("division", $"Employee with id: {updatedDivision.DirectorID} not found. Cannot update division.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (!_context.CompanyExists(updatedDivision.CompanyID))
                {
                    context.ModelState.AddModelError("division", $"Company with id: {updatedDivision.CompanyID} not found. Cannot update division.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (updatedDivision.Name == null || updatedDivision.Code == null)
                {
                    context.ModelState.AddModelError("division", "Division is missing required fields.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
                if (updatedDivision.Name.Length > 100 || updatedDivision.Code.Length > 100)
                {
                    context.ModelState.AddModelError("division", "Division fields are too long.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                var division = _context.GetDivisionByParameters(updatedDivision.Name, updatedDivision.Code);
                if (division != null && division.CompanyID == updatedDivision.CompanyID && division.DirectorID == updatedDivision.DirectorID)
                {
                    context.ModelState.AddModelError("division", "Division with entered name, code, directorID and companyID already exists.");
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
