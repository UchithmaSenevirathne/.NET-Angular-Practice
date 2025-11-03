using EmployeeApi.Application.DTOs;
using EmployeeApi.Domain.Entities;

namespace EmployeeApi.Application.Interfaces;

public interface IEmployeeService
{
    Task UpsertEmployeeAsync(Employee emp);
    Task DeleteEmployeeAsync(int id);
    Task<List<EmployeeDto>> GetAllEmployeesAsync();
}


