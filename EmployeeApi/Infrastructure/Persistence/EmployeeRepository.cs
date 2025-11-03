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
        try
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
        catch (SqlException ex)
        {
            throw new Exception("Database error while upserting employee.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Unexpected error while upserting employee.", ex);
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("DeleteEmployee", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while deleting employee with Id {id}.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while deleting employee with Id {id}.", ex);
        }
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        try
        {
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
        catch (SqlException ex)
        {
            throw new Exception("Database error while fetching employees.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Unexpected error while fetching employees.", ex);
        }
    }
}


