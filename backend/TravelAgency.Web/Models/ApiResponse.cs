namespace TravelAgency.Web.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class PaginatedResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<T>? Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
