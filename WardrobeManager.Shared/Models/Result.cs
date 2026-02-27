namespace WardrobeManager.Shared.Models;

public record Result<T>(T Data, bool Success, string Message = "")
{
    public T Data { get; set; } = Data;
    public bool Success { get; set; } = Success;
    public string Message { get; set; } = Message;
}