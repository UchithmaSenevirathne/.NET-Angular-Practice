namespace EmployeeApi.Application.DTOs;

public class DeleteEmployeeResponse
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

