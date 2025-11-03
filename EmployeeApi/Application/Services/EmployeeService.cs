using System.Linq;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;

namespace EmployeeApi.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;

    public EmployeeService(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    public async Task UpsertEmployeeAsync(Employee emp) => await _repo.UpsertEmployeeAsync(emp);
    public async Task DeleteEmployeeAsync(int id) => await _repo.DeleteEmployeeAsync(id);

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _repo.GetAllEmployeesAsync();
        return employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Position = e.Position,
            Salary = e.Salary,
            HireDate = e.HireDate
        }).ToList();
    }
}


