using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class TaskItem
{
    protected TaskItem()
    {
    }

    public TaskItem(
        Guid userId,
        string title,
        string? description,
        TaskPriority priority,
        DateTime? dueDate)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User ID cannot be empty.",
                nameof(userId)
            );

        Id = Guid.NewGuid();
        UserId = userId;

        SetTitle(title);
        SetDescription(description);

        Priority = priority;
        Status = Enums.TaskStatus.Pending;

        SetDueDate(dueDate);

        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Enums.TaskStatus Status { get; private set; }

    public TaskPriority Priority { get; private set; }

    public DateTime? DueDate { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public User? User { get; private set; }

    public void Update(
        string title,
        string? description,
        TaskPriority priority,
        DateTime? dueDate)
    {
        SetTitle(title);
        SetDescription(description);
        SetDueDate(dueDate);

        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (Status == Enums.TaskStatus.Completed)
            throw new InvalidOperationException(
                "A completed task cannot be started."
            );

        if (Status == Enums.TaskStatus.Cancelled)
            throw new InvalidOperationException(
                "A cancelled task cannot be started."
            );

        Status = Enums.TaskStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status == Enums.TaskStatus.Completed)
            return;

        if (Status == Enums.TaskStatus.Cancelled)
            throw new InvalidOperationException(
                "A cancelled task cannot be completed."
            );

        Status = Enums.TaskStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reopen()
    {
        if (Status != Enums.TaskStatus.Completed)
            throw new InvalidOperationException(
                "Only completed tasks can be reopened."
            );

        Status = Enums.TaskStatus.Pending;
        CompletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == Enums.TaskStatus.Completed)
            throw new InvalidOperationException(
                "A completed task cannot be cancelled."
            );

        Status = Enums.TaskStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePriority(TaskPriority priority)
    {
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Title cannot be empty.",
                nameof(title)
            );

        var normalizedTitle = title.Trim();

        if (normalizedTitle.Length < 3)
            throw new ArgumentException(
                "Title must contain at least 3 characters.",
                nameof(title)
            );

        if (normalizedTitle.Length > 150)
            throw new ArgumentException(
                "Title cannot exceed 150 characters.",
                nameof(title)
            );

        Title = normalizedTitle;
    }

    private void SetDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        var normalizedDescription = description.Trim();

        if (normalizedDescription.Length > 1000)
            throw new ArgumentException(
                "Description cannot exceed 1000 characters.",
                nameof(description)
            );

        Description = normalizedDescription;
    }

    private void SetDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue &&
            dueDate.Value.Kind != DateTimeKind.Utc)
        {
            dueDate = dueDate.Value.ToUniversalTime();
        }

        DueDate = dueDate;
    }
}