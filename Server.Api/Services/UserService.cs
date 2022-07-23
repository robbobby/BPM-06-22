using AutoMapper;
using Domain.Repository;
using Server.Core.Aggregates;
using Server.Core.Interfaces.Repository;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Api.Services;

public class UserService : IUserService {
    private readonly IUserRepository _userDb;
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly ILogger<UserService> _logger;
    private readonly Mapper _mapper;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userDb, IAccountUserRepository accountUserRepository, ILogger<UserService> logger, ITokenService tokenService) {
        MapperConfiguration config = new MapperConfiguration(config => config.CreateMap<UserRequest, User>());
        _mapper = new Mapper(config);
        _userDb = userDb;
        _accountUserRepository = accountUserRepository;
        _logger = logger;
        _tokenService = tokenService;
    }

    public User GetUser() { return new User(); }
    public async Task<TokenDto> CreateUser(UserRequest userRequest) {
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

        return await _tokenService.GetToken<TokenDto>(user);
    }

    public bool ValidateToken(string token) {
        return _tokenService.ValidateToken(token);
    }

    public Task<IQueryable<Guid>> GetAllUserAccountIds(string? userId) {
        return Task.FromResult(_accountUserRepository.GetAllUserAccountIds(userId));
    }
}
