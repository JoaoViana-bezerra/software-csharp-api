namespace TaskFlow.Application.DTOs.Users;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt
);