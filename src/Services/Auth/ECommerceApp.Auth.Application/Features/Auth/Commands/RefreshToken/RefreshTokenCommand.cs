using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand : IRequest<Result<TokenDto>>
    {
        public string Token { get; init; } = string.Empty;
    }
}
