namespace EmployeeApi.Application.DTOs;

public class EmployeeResponse
{
    public EmployeeDto Employee { get; set; } = null!;
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

