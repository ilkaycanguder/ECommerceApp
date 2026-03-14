using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Auth.Domain.Interfaces;
using ECommerceApp.Shared.Abstractions;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Features.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenDto>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);

            if (refreshToken is null || !refreshToken.IsActive)
                return Result.Failure<TokenDto>(Error.Create("Auth.RefreshToken", "Invalid or expired refresh token."));

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return Result.Failure<TokenDto>(Error.Create("Auth.RefreshToken", "User not found."));

            refreshToken.Revoke();

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();

            var newRefreshToken = Domain.Entities.RefreshToken.Create(
                user.Id,
                newRefreshTokenValue,
                DateTime.UtcNow.AddDays(7));

            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}
