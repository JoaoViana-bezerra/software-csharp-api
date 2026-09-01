using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Infrastructure.Authentication;

public sealed class BCryptPasswordHasher
    : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "Password cannot be empty.",
                nameof(password)
            );
        }

        return BCrypt.Net.BCrypt.HashPassword(
            password,
            workFactor: WorkFactor
        );
    }

    public bool Verify(
        string password,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (string.IsNullOrWhiteSpace(passwordHash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(
                password,
                passwordHash
            );
        }
        catch
        {
            return false;
        }
    }
}