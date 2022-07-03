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
        User? user = _mapper.Map<User>(userRequest);
        user.Accounts = new List<Account>();
        user.Accounts.Add(new Account());
        _logger.LogInformation($"Creating user : {user.EmailAddress}");
        _userDb.Create(user);
        _userDb.SaveChanges();
    }
}
