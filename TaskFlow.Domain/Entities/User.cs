namespace TaskFlow.Domain.Entities;

public class User
{
    private readonly List<TaskItem> _tasks = [];

    protected User()
    {
    }

    public User(
        string name,
        string email,
        string passwordHash)
    {
        Id = Guid.NewGuid();

        SetName(name);
        SetEmail(email);

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException(
                "Password hash cannot be empty.",
                nameof(passwordHash)
            );

        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    public void UpdateProfile(
        string name,
        string email)
    {
        SetName(name);
        SetEmail(email);

        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException(
                "Password hash cannot be empty.",
                nameof(newPasswordHash)
            );

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name cannot be empty.",
                nameof(name)
            );

        if (name.Trim().Length < 3)
            throw new ArgumentException(
                "Name must contain at least 3 characters.",
                nameof(name)
            );

        if (name.Trim().Length > 120)
            throw new ArgumentException(
                "Name cannot exceed 120 characters.",
                nameof(name)
            );

        Name = name.Trim();
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(
                "Email cannot be empty.",
                nameof(email)
            );

        var normalizedEmail = email
            .Trim()
            .ToLowerInvariant();

        if (!normalizedEmail.Contains('@'))
            throw new ArgumentException(
                "Invalid email address.",
                nameof(email)
            );

        if (normalizedEmail.Length > 180)
            throw new ArgumentException(
                "Email cannot exceed 180 characters.",
                nameof(email)
            );

        Email = normalizedEmail;
    }
}