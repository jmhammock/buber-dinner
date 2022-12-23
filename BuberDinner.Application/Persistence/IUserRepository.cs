using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid id);
    Task<User?> GetUserAsync(string email);
    Task<User?> GetUserAsync(string email, string password);
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
}