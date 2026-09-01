using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.DTOs.Users;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

public sealed class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<UserResponse> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email
            .Trim()
            .ToLowerInvariant();

        if (await _userRepository.EmailExistsAsync(
                normalizedEmail,
                cancellationToken))
        {
            throw new InvalidOperationException(
                "A user with this email already exists."
            );
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException(
                "Password cannot be empty.",
                nameof(request.Password)
            );
        }

        if (request.Password.Length < 8)
        {
            throw new ArgumentException(
                "Password must contain at least 8 characters.",
                nameof(request.Password)
            );
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.Name,
            normalizedEmail,
            passwordHash
        );

        await _userRepository.AddAsync(
            user,
            cancellationToken
        );

        await _userRepository.SaveChangesAsync(
            cancellationToken
        );

        return MapUser(user);
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(
            normalizedEmail,
            cancellationToken
        );

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password."
            );
        }

        var passwordIsValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!passwordIsValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password."
            );
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse(
            token,
            MapUser(user)
        );
    }

    private static UserResponse MapUser(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAt
        );
    }
}