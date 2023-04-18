﻿using AutoMapper;
using Chat.BLL.DTO;
using Chat.DAL.Entities;
using Chat.DAL.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserRegistrationModel>()
                .ForMember(u => u.FirstName, x => x.MapFrom(z => z.FirstName))
                .ForMember(u => u.LastName, x => x.MapFrom(z => z.LastName))
                .ReverseMap();

            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }

    }
}