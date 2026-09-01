using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default
    );

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default
    );
}