using EmployeeApi.Domain.Entities;

namespace EmployeeApi.Application.Interfaces;

public interface IEmployeeRepository
{
    Task UpsertEmployeeAsync(Employee emp);
    Task DeleteEmployeeAsync(int id);
    Task<List<Employee>> GetAllEmployeesAsync();
}


