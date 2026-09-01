using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}