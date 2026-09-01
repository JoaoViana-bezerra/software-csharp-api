namespace TaskFlow.Application.DTOs.Users;

public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string Password
);