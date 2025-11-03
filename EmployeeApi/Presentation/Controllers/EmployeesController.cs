using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;
using System.Net;

namespace EmployeeApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    //GET: api/employees
    [HttpGet]
    public async Task<ActionResult<ApiResponse<EmployeeListResponse>>> GetEmployees()
    {
        try
        {
            var employees = await _service.GetAllEmployeesAsync();
            var response = new ApiResponse<EmployeeListResponse>
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Employees retrieved successfully",
                Data = new EmployeeListResponse
                {
                    Employees = employees,
                    Count = employees.Count,
                    Timestamp = DateTime.UtcNow
                }
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResponse<EmployeeListResponse>
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Error retrieving employees: {ex.Message}",
                Data = new EmployeeListResponse
                {
                    Employees = new List<EmployeeDto>(),
                    Count = 0,
                    Timestamp = DateTime.UtcNow
                }
            };
            return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
        }
    }

    //POST: api/employees
    [HttpPost]
    public async Task<ActionResult<ApiResponse<(Employee employee, string status, DateTime timestamp)>>> CreateEmployee(Employee employee)
    {
        try
        {
            //for new employee Id should be 0 or omitted
            employee.Id = 0;
            await _service.UpsertEmployeeAsync(employee);
            
            var response = new ApiResponse<(Employee, string, DateTime)>
            {
                Code = (int)HttpStatusCode.Created,
                Message = "Employee created successfully",
                Data = (employee, "Created", DateTime.UtcNow)
            };
            return StatusCode((int)HttpStatusCode.Created, response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResponse<(Employee, string, DateTime)>
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Error creating employee: {ex.Message}",
                Data = (employee, "Failed", DateTime.UtcNow)
            };
            return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
        }
    }

    //PUT: api/employee/2
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<(Employee employee, string status, DateTime timestamp)>>> UpdateEmployee(int id, Employee employee)
    {
        try
        {
            if (id != employee.Id)
            {
                var badRequestResponse = new ApiResponse<(Employee, string, DateTime)>
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Employee ID mismatch",
                    Data = (employee, "Validation Error", DateTime.UtcNow)
                };
                return BadRequest(badRequestResponse);
            }

            await _service.UpsertEmployeeAsync(employee);
            
            var response = new ApiResponse<(Employee, string, DateTime)>
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Employee updated successfully",
                Data = (employee, "Updated", DateTime.UtcNow)
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResponse<(Employee, string, DateTime)>
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Error updating employee: {ex.Message}",
                Data = (employee, "Failed", DateTime.UtcNow)
            };
            return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
        }
    }

    //DELETE: api/employee/2
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<(int id, string status, DateTime timestamp)>>> DeleteEmployee(int id)
    {
        try
        {
            await _service.DeleteEmployeeAsync(id);
            
            var response = new ApiResponse<(int, string, DateTime)>
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Employee deleted successfully",
                Data = (id, "Deleted", DateTime.UtcNow)
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResponse<(int, string, DateTime)>
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Error deleting employee: {ex.Message}",
                Data = (id, "Failed", DateTime.UtcNow)
            };
            return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
        }
    }
}


