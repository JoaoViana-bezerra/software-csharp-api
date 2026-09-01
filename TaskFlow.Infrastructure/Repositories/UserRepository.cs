using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories;

public sealed class UserRepository
    : IUserRepository
{
    private readonly TaskFlowDbContext _context;

    public UserRepository(
        TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Id == id,
                cancellationToken
            );
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email
            .Trim()
            .ToLowerInvariant();

        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Email == normalizedEmail,
                cancellationToken
            );
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email
            .Trim()
            .ToLowerInvariant();

        return await _context.Users
            .AnyAsync(
                user => user.Email == normalizedEmail,
                cancellationToken
            );
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(
            user,
            cancellationToken
        );
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken
        );
    }
}