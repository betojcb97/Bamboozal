﻿using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<AddUserDto, User>();

            CreateMap<User, ReadUserDto>()
                .ForMember(userDto => userDto.addressDto,
                opt => opt.MapFrom(user => user.address));

        }
    }
}
