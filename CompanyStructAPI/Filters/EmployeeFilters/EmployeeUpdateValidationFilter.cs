﻿using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;

namespace CompanyStructAPI.Filters.EmployeeFilters
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
                context.ModelState.AddModelError("employee", "Employee is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                if (updatedEmployee.FirstName == null || updatedEmployee.LastName == null || updatedEmployee.Phone == null || updatedEmployee.Email == null)
                {
                    context.ModelState.AddModelError("employee", "Employee is missing required fields.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
                if (updatedEmployee.FirstName.Length > 100 || updatedEmployee.LastName.Length > 100 || updatedEmployee.Phone.Length > 100 || updatedEmployee.Email.Length > 100)
                {
                    context.ModelState.AddModelError("employee", "Employee fields are too long.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                var employee = _context.GetEmployeeByParameters(updatedEmployee.FirstName, updatedEmployee.LastName, updatedEmployee.Phone, updatedEmployee.Email);
                if (employee != null)
                {
                    context.ModelState.AddModelError("employee", "Employee with entered first name, last name, phone and email already exists.");
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
