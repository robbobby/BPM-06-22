namespace Server.Core.Exceptions; 

public class UserAlreadyExistsException : Exception {
    public UserAlreadyExistsException(string message) : base(message) { }
    public UserAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    public UserAlreadyExistsException() { }
}
