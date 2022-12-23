using BuberDinner.Application.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new List<User> { };

    async public Task<User?> GetUserAsync(Guid id)
    {
        return await Task.FromResult(_users.SingleOrDefault(u => u.Id == id));
    }
    async public Task<User?> GetUserAsync(string email)
    {
        return await Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
    }
    async public Task<User?> GetUserAsync(string email, string password)
    {
        return await Task.FromResult(_users.FirstOrDefault(u => u.Email == email && u.Password == password));
    }
    async public Task<User> AddUserAsync(User user)
    {
        _users.Add(user);
        return await Task.FromResult(user);
    }
    async public Task<User> UpdateUserAsync(User user)
    {
        var index = _users.FindIndex(u => u.Id == user.Id);
        _users[index] = user;
        return await Task.FromResult(user);
    }
    async public Task DeleteUserAsync(Guid id)
    {
        var user = await GetUserAsync(id);
        if (user is not null)
        {
            _users.Remove(user);
        }

        await Task.CompletedTask;
    }
}