using Api.Controllers;
using Api.Models;
using Api.Models.DbModel;

namespace Api.Interfaces;

public interface IUserService {
    User GetUser();
    Task<string> CreateUser(UserRequest userRequest);
    bool ValidateToken(string token);
}
