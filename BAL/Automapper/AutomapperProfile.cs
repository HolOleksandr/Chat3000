using AutoMapper;
using BAL.DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }

    }
}
