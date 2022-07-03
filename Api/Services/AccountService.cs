using Api.Interfaces;
using Api.Models.DbModel;

namespace Api.Services; 

public class AccountService : IAccountService {
    public Account GetNewAccount() {
        return new Account {
            AccountId = new Guid(),
            DateCreated = DateTime.Now,
            Plan = "free"
        };
    }
}
