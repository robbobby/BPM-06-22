using Api.Controllers;
using Api.Models;
using Api.Models.DbModel;

namespace Api.Interfaces;

public interface IUserService {
    User GetUser();
    Task CreateUser(UserRequest userRequest)
        ;
}
