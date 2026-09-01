using TaskFlow.Application.DTOs.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using DomainTaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.Services;

public sealed class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponse> CreateAsync(
        Guid userId,
        CreateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        var task = new TaskItem(
            userId,
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate
        );

        await _taskRepository.AddAsync(
            task,
            cancellationToken
        );

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );

        return Map(task);
    }

    public async Task<IReadOnlyCollection<TaskResponse>> GetAllAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetAllByUserAsync(
            userId,
            cancellationToken
        );

        return tasks
            .Select(Map)
            .ToList()
            .AsReadOnly();
    }

    public async Task<TaskResponse> GetByIdAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        return Map(task);
    }

    public async Task<TaskResponse> UpdateAsync(
        Guid userId,
        Guid taskId,
        UpdateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        task.Update(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate
        );

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );

        return Map(task);
    }

    public async Task<TaskResponse> StartAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        task.Start();

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );

        return Map(task);
    }

    public async Task<TaskResponse> CompleteAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        task.Complete();

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );

        return Map(task);
    }

    public async Task<TaskResponse> ReopenAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        task.Reopen();

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );

        return Map(task);
    }

    public async Task<TaskResponse> CancelAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        task.Cancel();

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );

        return Map(task);
    }

    public async Task DeleteAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await GetTaskOrThrowAsync(
            taskId,
            userId,
            cancellationToken
        );

        _taskRepository.Remove(task);

        await _taskRepository.SaveChangesAsync(
            cancellationToken
        );
    }

    private async Task<TaskItem> GetTaskOrThrowAsync(
        Guid taskId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(
            taskId,
            userId,
            cancellationToken
        );

        if (task is null)
        {
            throw new KeyNotFoundException(
                "Task not found."
            );
        }

        return task;
    }

    private static TaskResponse Map(TaskItem task)
    {
        return new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.Priority,
            task.DueDate,
            task.CreatedAt,
            task.UpdatedAt,
            task.CompletedAt
        );
    }
}