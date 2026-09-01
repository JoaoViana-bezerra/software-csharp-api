using TaskFlow.Application.DTOs.Users;

namespace TaskFlow.Application.DTOs.Auth;

public sealed record AuthResponse(
    string Token,
    UserResponse User
);