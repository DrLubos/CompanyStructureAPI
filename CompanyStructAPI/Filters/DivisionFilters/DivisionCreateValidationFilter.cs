using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace CompanyStructAPI.Filters.DivisionFilters
{
    public class DivisionCreateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DivisionCreateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("division"))
            {
                var division = context.ActionArguments["division"] as Division;
                if (division != null)
                {
                    if (!_context.EmployeeExists(division.DirectorID))
                    {
                        context.ModelState.AddModelError("division", $"Employee with id: {division.DirectorID} not found. Cannot create division.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (!_context.CompanyExists(division.CompanyID))
                    {
                        context.ModelState.AddModelError("division", $"Company with id: {division.CompanyID} not found. Cannot create division.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (division.Name == null || division.Code == null)
                    {
                        context.ModelState.AddModelError("division", "Division is missing required fields.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                    if (division.Name.Length > 100 || division.Code.Length > 100)
                    {
                        context.ModelState.AddModelError("division", "Division fields are too long.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (_context.DivisionParametersExists(division.Name, division.Code))
                    {
                        context.ModelState.AddModelError("division", "Division already exists.");
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
