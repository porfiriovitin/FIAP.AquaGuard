namespace FIAP.AquaGuard.API.Models;

public record PayloadResponse<T>
{
    public string Status { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
}
