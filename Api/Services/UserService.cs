using Api.Controllers;
using Api.Interfaces;
using Api.Interfaces.Repository;
using Api.Models;
using Api.Models.DbModel;
using AutoMapper;
using Newtonsoft.Json;

namespace Api.Services;

public class UserService : IUserService {
    private readonly IUserRepository _userDb;
    private readonly ILogger<UserService> _logger;
    private Mapper _mapper;
    private IAccountService _accountService;

    public UserService(IUserRepository userDb, ILogger<UserService> logger, IAccountService accountService) {
        MapperConfiguration config = new MapperConfiguration(config => config.CreateMap<UserRequest, User>());
        _mapper = new Mapper(config);
        _userDb = userDb;
        _logger = logger;
        _accountService = accountService;
    }

    public User GetUser() { return new User(); }
    public async Task CreateUser(UserRequest userRequest) {
        var user = _mapper.Map<User>(userRequest);
        
        user.Salt = BCrypt.Net.BCrypt.GenerateSalt(8);
        user.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, user.Salt);
        
        user.AccountUsers = new List<AccountUser>();
        var account = new Account();
        user.AccountUsers.Add(new AccountUser {
            Account = account,
            User = user,
            Role = "Admin"
        });

        user.DefaultAccount = account.Id;
        
        _userDb.Create(user);
        _userDb.SaveChanges();
        
        user.DefaultAccount = account.Id;
        _userDb.Update(user);
        _userDb.SaveChanges();
    }
}
