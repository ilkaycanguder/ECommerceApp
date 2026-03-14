using AutoMapper;
using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Auth.Domain.Entities;
using ECommerceApp.Auth.Domain.Interfaces;
using ECommerceApp.Shared.Abstractions;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Features.Auth.Commands.Register
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(request.Email, cancellationToken);

            if (!isEmailUnique)
                return Result.Failure<UserDto>(Error.Create("Auth.Register", "Email already exists."));

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = User.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                passwordHash,
                request.Role);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(_mapper.Map<UserDto>(user));
        }
    }
}

