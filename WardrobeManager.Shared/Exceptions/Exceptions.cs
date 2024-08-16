namespace WardrobeManager.Shared.Exceptions;

[Serializable]
public class UserNotFoundException : Exception {
    public UserNotFoundException() { }
    public UserNotFoundException(string message): base(message) { }
    public UserNotFoundException(string message, Exception inner): base(message, inner) { }
}
[Serializable]
public class ClothingItemNotFoundException : Exception {
    public ClothingItemNotFoundException() { }
    public ClothingItemNotFoundException(string message) : base(message) { }
    public ClothingItemNotFoundException(string message, Exception inner): base(message, inner) { }
}
