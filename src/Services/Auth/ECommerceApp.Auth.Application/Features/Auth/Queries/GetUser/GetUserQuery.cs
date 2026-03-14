using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Features.Auth.Queries.GetUser
{
    public sealed record GetUserQuery : IRequest<Result<UserDto>>
    {
        public Guid UserId { get; init; }
    }
}
