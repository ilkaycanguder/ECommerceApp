using AutoMapper;
using ECommerceApp.Auth.Application.DTOs;
using ECommerceApp.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Application.Mappings
{
    public sealed class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
