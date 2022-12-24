using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common;

public record AuthenticationResult(
    User User,
    string Token
);