using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<TaskItem>> GetAllByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default
    );

    void Remove(TaskItem task);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default
    );
}