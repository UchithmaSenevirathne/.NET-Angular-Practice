namespace EmployeeApi.Application.DTOs;

public class EmployeeListResponse
{
    public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    public int Count { get; set; }
    public DateTime Timestamp { get; set; }
}

