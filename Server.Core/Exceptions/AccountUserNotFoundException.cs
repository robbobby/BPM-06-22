namespace Api.Services;

public class AccountUserNotFoundException : Exception {
    public AccountUserNotFoundException() : base("Account user not found") { }
    public AccountUserNotFoundException(string message) : base(message) { }
    public AccountUserNotFoundException(string message, Exception inner) : base(message, inner) { }
}
