using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<TokenDto>> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<TokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
