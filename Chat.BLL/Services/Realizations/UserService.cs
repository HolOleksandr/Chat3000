using AutoMapper;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.UoW.Interface;

namespace Chat.BLL.Services.Realizations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
