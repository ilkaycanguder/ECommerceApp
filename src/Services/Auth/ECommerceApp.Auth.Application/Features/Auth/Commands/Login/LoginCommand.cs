using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand : IRequest<Result<TokenDto>>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
