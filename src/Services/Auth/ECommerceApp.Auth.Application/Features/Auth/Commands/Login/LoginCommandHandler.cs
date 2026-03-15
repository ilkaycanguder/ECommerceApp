using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Auth.Domain.Entities;
using ECommerceApp.Auth.Domain.Interfaces;
using ECommerceApp.Shared.Abstractions;
using ECommerceApp.Shared.Models;
using MediatR;
using DomainEntities = ECommerceApp.Auth.Domain.Entities;


namespace ECommerceApp.Auth.Application.Features.Auth.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Result.Failure<TokenDto>(Error.Create("Auth.Login", "Invalid email or password."));

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();

            var refreshToken = DomainEntities.RefreshToken.Create(
                 user.Id,
                 refreshTokenValue,
                 DateTime.UtcNow.AddDays(7));

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}
