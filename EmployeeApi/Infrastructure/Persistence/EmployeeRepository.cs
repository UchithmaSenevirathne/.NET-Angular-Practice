using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Application.Interfaces;

namespace EmployeeApi.Infrastructure.Persistence;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    // Uses same SP for Insert/Update
    public async Task UpsertEmployeeAsync(Employee emp)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("UpsertEmployee", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", emp.Id);
        command.Parameters.AddWithValue("@Name", emp.Name);
        command.Parameters.AddWithValue("@Email", emp.Email);
        command.Parameters.AddWithValue("@Position", emp.Position);
        command.Parameters.AddWithValue("@Salary", emp.Salary);
        command.Parameters.AddWithValue("@HireDate", emp.HireDate);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("DeleteEmployee", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        //ko mewai try catch bloak controoler ekei mekei dekama dnna ona exection ekk ewath ara model ekatamai data passe status code,message data
        var employees = new List<Employee>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("GetAllEmployees", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            employees.Add(new Employee
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString()!,
                Email = reader["Email"].ToString()!,
                Position = reader["Position"].ToString()!,
                Salary = (decimal)reader["Salary"],
                HireDate = (DateTime)reader["HireDate"]
            });
        }
        return employees;
    }
}


