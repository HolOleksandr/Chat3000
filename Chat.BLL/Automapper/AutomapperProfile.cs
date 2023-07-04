using AutoMapper;
using Chat.BLL.DTO;
using Chat.DAL.Entities;
using Chat.BLL.Models.Requests;
using Chat.BLL.Automapper.Resolvers;
using Chat.BLL.Services.Interfaces;

namespace Chat.BLL.Automapper
{
    public class AutomapperProfile : Profile
    {
        private IBlobManager _blobManager;
        public AutomapperProfile(IBlobManager blobManager)
        {
            _blobManager = blobManager;

            CreateMap<User, UserRegistrationModel>()
                .ForMember(u => u.FirstName, x => x.MapFrom(z => z.FirstName))
                .ForMember(u => u.LastName, x => x.MapFrom(z => z.LastName))
                .ReverseMap();

            CreateMap<Message, MessageDTO>().ReverseMap();

            CreateMap<User, UserDTO>()
                .ForMember(u => u.Avatar, x=> x.MapFrom(new AvatarResolver(_blobManager)))
                .ReverseMap();

            CreateMap<Group, GroupDTO>()
                .ForMember(m => m.UsersCount, g => g.MapFrom(u=>u.Users.AsEnumerable().Count()))                
                .ReverseMap();

            CreateMap<GroupInfoView, GroupInfoViewDTO>().ReverseMap();

            CreateMap<User, UserShortInfoDTO>().ReverseMap();

            CreateMap<CreateGroupRequest, Group>();
            
            CreateMap<PdfContract, PdfContractDTO>()
                .ForMember(u => u.FileUrl, x => x.MapFrom(new PdfContractResolver(_blobManager)))
                .ReverseMap();
        }
    }
}
