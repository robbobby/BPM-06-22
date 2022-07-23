namespace Server.Core.Exceptions; 

public class TokenStillValidException : Exception {
    public TokenStillValidException() { }
    public TokenStillValidException(string message) : base(message) { }
    public TokenStillValidException(string message, Exception inner) : base(message, inner) { }
}
