namespace EmployeeApi.Application.DTOs;

public class ApiResponse<T>
{
    public int Code { get; set; }           // HTTP status code
    public string Message { get; set; } = string.Empty;  // Response message
    public T Data { get; set; }             // Generic data (can be object, tuple, etc.)
}
