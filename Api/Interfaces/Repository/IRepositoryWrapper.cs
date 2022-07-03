using Api.Interfaces.Repository;
public interface IRepositoryWrapper {
    IUserRepository Owner { get; }
    IAccountRepository Account { get; }
    void Save();
}
